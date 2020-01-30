using Plugin.BLE;
using Plugin.BLE.Abstractions;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.EventArgs;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace EarableLibrary
{
	/// <summary>
	/// This class is used for safe communication with an BLE device.
	/// It keeps an internal queue of read- and write operations, to ensure serial operation-execution.
	/// </summary>
	public class BLEConnection
	{
		private readonly IDevice _device;

		private readonly ConcurrentDictionary<Guid, ICharacteristic> _characteristics = new ConcurrentDictionary<Guid, ICharacteristic>();

		private readonly Dictionary<Guid, List<DataReceiver>> _subscriptions = new Dictionary<Guid, List<DataReceiver>>();

		private readonly object _connectionStateLock = new object();

		private CancellationTokenSource _connectionToken;

		private BlockingCollection<BLEOperation> _operationQueue;

		private Thread _messageDispatcher;

		/// <summary>
		/// Delegate for accepting data that comes from the BLE device.
		/// </summary>
		/// <param name="data">Binary data</param>
		public delegate void DataReceiver(byte[] data);

		/// <summary>
		/// Whether this connection is open, closed or in a state inbetween.
		/// </summary>
		public ConnectionState State { get; private set; } = ConnectionState.Disconnected;

		/// <summary>
		/// Unique id of the connected device.
		/// </summary>
        public Guid Id => _device.Id;

		/// <summary>
		/// Construct a new BLEConnection.
		/// </summary>
		/// <param name="device">Device to communicate with</param>
		public BLEConnection(IDevice device)
		{
			_device = device;
		}

		/// <summary>
		/// Asynchronously open the connection.
		/// </summary>
		/// <returns>Whether the connection could be opened or not</returns>
		public async Task<bool> Open()
		{
			lock (_connectionStateLock)
			{
				if (State != ConnectionState.Disconnected) return false;
				State = ConnectionState.Connecting;
				_connectionToken = new CancellationTokenSource();
			}
			try
			{
				var parameters = new ConnectParameters(forceBleTransport: true);
				await CrossBluetoothLE.Current.Adapter.ConnectToDeviceAsync(_device, parameters, _connectionToken.Token);
				lock (_connectionStateLock)
				{
					if (_connectionToken.IsCancellationRequested)
					{
						State = ConnectionState.Disconnected;
						return false;
					}
					_connectionToken = null;
				}
				await LoadCharacteristics();
				_operationQueue = new BlockingCollection<BLEOperation>();
				_messageDispatcher = new Thread(new ThreadStart(DispatchOperation))
				{
					IsBackground = true
				};
				_messageDispatcher.Start();
				State = ConnectionState.Connected;
				return true;
			}
			catch (Exception e)
			{
				Debug.WriteLine("Connection could not be opened: {0}", args: e.Message);
				State = ConnectionState.Disconnected;
				return false;
			}
		}

		/// <summary>
		/// Validate this connection based on whether the given services are available or not.
		/// </summary>
		/// <param name="serviceIds">Service ids to check for</param>
		/// <returns>true if all given services are available, false otherwise</returns>
		public async Task<bool> Validate(Guid[] serviceIds)
		{
			foreach (Guid serviceId in serviceIds)
			{
				var service = await _device.GetServiceAsync(serviceId);
				if (service == null) return false;
			}
			return true;
		}

		/// <summary>
		/// Asynchronously close the connection.
		/// By default this method will wait for already scheduled operations to complete.
		/// </summary>
		/// <param name="force">Do not wait for scheduled operations to complete</param>
		/// <returns>Whether the connection was closed or not</returns>
		public async Task<bool> Close(bool force = false)
		{
			lock (_connectionStateLock)
			{
				if (State == ConnectionState.Connecting && _connectionToken != null)
				{
					_connectionToken.Cancel();
					return true;
				}
				else if (State != ConnectionState.Connected) return false;
				State = ConnectionState.Disconnecting;
			}
			// Shut down message dispatcher
			_operationQueue.CompleteAdding();
			if (force)
			{
				_operationQueue.Dispose();
				_messageDispatcher.Abort();
			}
			else
			{
				_messageDispatcher.Join();
			}
			try
			{
				// Unsubscribe everything
				await UnsubscribeAll();
				await CrossBluetoothLE.Current.Adapter.DisconnectDeviceAsync(_device);
				_characteristics.Clear();
				State = ConnectionState.Disconnected;
				return true;
			}
			catch (Exception e)
			{
				Debug.WriteLine("Could not disconnect: {0}", args: e.Message);
				State = ConnectionState.Connected;
				return false;
			}
		}

		/// <summary>
		/// Write to a characteristic.
		/// </summary>
		/// <param name="charId">Characteristic id</param>
		/// <param name="val">Value to write</param>
		/// <returns>false if the connection doesn't allow write-operations right now</returns>
		/// <exception cref="KeyNotFoundException">If a characteristic with the given id has not been loaded</exception>
		public bool Write(Guid charId, byte[] val)
		{
			return EnqueueOperation(new WriteOperation(_characteristics[charId], val));
		}

		/// <summary>
		/// Start reading from a characteristic and invoke a handler, when the value is available.
		/// </summary>
		/// <param name="charId">Characteristic id</param>
		/// <param name="handler">Handler to receive the value once available</param>
		/// <returns>false if the connection doesn't allow read-operations right now</returns>
		/// <exception cref="KeyNotFoundException">If a characteristic with the given id has not been loaded</exception>
		public bool Read(Guid charId, DataReceiver handler)
		{
			return EnqueueOperation(new ReadOperation(_characteristics[charId], handler));
		}

		/// <summary>
		/// Subscribe to a characteristic.
		/// </summary>
		/// <param name="charId">Characteristic id</param>
		/// <param name="handler">Handler to be notified when an update occurs</param>
		/// <exception cref="KeyNotFoundException">If a characteristic with the given id has not been loaded</exception>
		public void Subscribe(Guid charId, DataReceiver handler)
		{
			if (!_subscriptions.ContainsKey(charId))
			{
				_subscriptions[charId] = new List<DataReceiver>();
				var characteristic = _characteristics[charId];
				characteristic.ValueUpdated += CharacteristicValueUpdated;
				EnqueueOperation(new SubscriptionOperation(characteristic, subscribe: true));
			}
			_subscriptions[charId].Add(handler);
		}

		/// <summary>
		/// Unsubscribe from an characteristic.
		/// </summary>
		/// <param name="charId">Characteristic id</param>
		/// <param name="handler">Handler that has been previously subscribed</param>
		/// <exception cref="KeyNotFoundException">If a characteristic with the given id has not been loaded</exception>
		public void Unsubscribe(Guid charId, DataReceiver handler)
		{
			if (_subscriptions.ContainsKey(charId))
			{
				_subscriptions[charId].Remove(handler);
				if (_subscriptions[charId].Count == 0)
				{
					_subscriptions.Remove(charId);
					var characteristic = _characteristics[charId];
					characteristic.ValueUpdated -= CharacteristicValueUpdated;
					EnqueueOperation(new SubscriptionOperation(characteristic, subscribe: false));
				}
			}
		}

		/// <summary>
		/// Can be called to indicate, that the device state has changed due to outer circumstances.
		/// </summary>
		internal void UpdateState()
		{
			if (State == ConnectionState.Connected && _device.State == DeviceState.Disconnected)
			{
				State = ConnectionState.Disconnected;
			}
			else if (State == ConnectionState.Disconnected && _device.State == DeviceState.Connected)
			{
				State = ConnectionState.Connected;
			}
		}

		/// <summary>
		/// Try adding to the operation queue and return false if it fails.
		/// This way no lock is required on the queue.
		/// </summary>
		private bool EnqueueOperation(BLEOperation op)
		{
			Debug.WriteLine("Enqueing operation ({0})", args: op.GetType());
			try
			{
				_operationQueue.Add(op);
				return true;
			}
			catch (NullReferenceException)
			{
				return false;
			}
			catch (InvalidOperationException)
			{
				return false;
			}
		}

		/// <summary>
		/// Unsubscribe from all Characteristics.
		/// </summary>
		private async Task UnsubscribeAll()
		{
			foreach (var subscription in _subscriptions)
			{
				var characteristic = _characteristics[subscription.Key];
				characteristic.ValueUpdated -= CharacteristicValueUpdated;
				await characteristic.StopUpdatesAsync();
			}
		}

		private async Task LoadCharacteristics()
		{
			_characteristics.Clear();
			var services = await _device.GetServicesAsync();
			foreach (var service in services)
			{
				var characteristics = await service.GetCharacteristicsAsync();
				foreach (var c in characteristics) _characteristics[c.Id] = c;
			}
		}

		private void CharacteristicValueUpdated(object sender, CharacteristicUpdatedEventArgs e)
		{
			Debug.WriteLine("A value of {0} has been updated", args: e.Characteristic.Uuid);
			if (_subscriptions.ContainsKey(e.Characteristic.Id))
			{
				foreach (var handler in _subscriptions[e.Characteristic.Id])
				{
					handler(e.Characteristic.Value);
				}
			}
		}

		private async void DispatchOperation()
		{
			foreach (BLEOperation operation in _operationQueue.GetConsumingEnumerable())
			{
				Debug.WriteLine("Dispatching operation ({0})", args: operation.GetType());
				try
				{
					await operation.Execute();
				}
				catch (ThreadAbortException)
				{
					return;
				}
				catch (Exception)
				{
					// TODO: retry / notify operation-source / etc.
					continue;
				}
			}
		}
	}

	internal interface BLEOperation
	{
		Task Execute();
	}

	internal class WriteOperation : BLEOperation
	{
		public ICharacteristic Characteristic;
		public byte[] Message;
		public WriteOperation(ICharacteristic characteristic, byte[] message)
		{
			Characteristic = characteristic;
			Message = message;
		}
		public async Task Execute()
		{
			await Characteristic.WriteAsync(Message);
		}
	}

	internal class SubscriptionOperation : BLEOperation
	{
		public ICharacteristic Characteristic;
		public bool Subscribe;
		public SubscriptionOperation(ICharacteristic characteristic, bool subscribe)
		{
			Characteristic = characteristic;
			Subscribe = subscribe;
		}
		public async Task Execute()
		{
			if (Subscribe) await Characteristic.StartUpdatesAsync();
			else await Characteristic.StopUpdatesAsync();
		}
	}

	internal class ReadOperation : BLEOperation
	{
		public ICharacteristic Characteristic;
		private BLEConnection.DataReceiver handler;

		public ReadOperation(ICharacteristic characteristic, BLEConnection.DataReceiver handler)
		{
			Characteristic = characteristic;
			this.handler = handler;
		}

		public async Task Execute()
		{
			var data = await Characteristic.ReadAsync();
			handler(data);
		}
	}
}
