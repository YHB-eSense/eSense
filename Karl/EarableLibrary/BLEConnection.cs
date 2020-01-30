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
	public class BLEConnection
	{
		private readonly IDevice _device;

		private readonly Dictionary<Guid, ICharacteristic> _characteristics = new Dictionary<Guid, ICharacteristic>();

		private readonly Dictionary<Guid, List<SubscriptionHandler>> _subscriptions = new Dictionary<Guid, List<SubscriptionHandler>>();

		private readonly CancellationTokenSource _connectionToken = new CancellationTokenSource();

		private readonly CancellationTokenSource _operationToken = new CancellationTokenSource();

		private BlockingCollection<BLEOperation> _operationQueue;

		private Thread _messageDispatcher;

		public delegate void SubscriptionHandler(byte[] data);

		/// <summary>
		/// Whether this connection is currently open or not.
		/// </summary>
        public bool Established { get; private set; }

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
			try
			{
				if (_device.State != DeviceState.Disconnected) return false;
				// Establish connection
				var parameters = new ConnectParameters(forceBleTransport: true);
				await CrossBluetoothLE.Current.Adapter.ConnectToDeviceAsync(_device, parameters, _connectionToken.Token);
				// Start up message dispatcher
				_operationQueue = new BlockingCollection<BLEOperation>();
				_messageDispatcher = new Thread(new ThreadStart(DispatchOperation))
				{
					IsBackground = true
				};
				Established = true;
				_messageDispatcher.Start();
				return true;
			}
			catch (Exception e)
			{
				Debug.WriteLine("Connection could not be opened: {0}", e.Message);
				return false;
			}
		}

		/// <summary>
		/// Asynchronously close the connection.
		/// By default this method will wait for already scheduled operations to complete.
		/// </summary>
		/// <param name="force">Do not wait for scheduled operations to complete</param>
		/// <returns></returns>
		public async Task<bool> Close(bool force = false)
		{
			if (_device.State == DeviceState.Connecting)
			{
				_connectionToken.Cancel(); // cancel current connection process
			}
			if (_operationQueue != null && _messageDispatcher != null) {
				_operationQueue.CompleteAdding();
				if (force)
				{
					_operationQueue.Dispose();
					_operationToken.Cancel();
					_messageDispatcher.Abort();
				}
				else
				{
					_messageDispatcher.Join();
				}
				_operationQueue = null;
				_messageDispatcher = null;
			}
			try
			{
				if (_device.State == DeviceState.Connected) await CrossBluetoothLE.Current.Adapter.DisconnectDeviceAsync(_device);
				Established = false;
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}

		public bool Write(Guid charId, byte[] msg)
		{
			return EnqueueOperation(new WriteOperation(_characteristics[charId], msg));
		}

		public bool Read(Guid charId, SubscriptionHandler handler)
		{
			return EnqueueOperation(new ReadOperation(_characteristics[charId], handler));
		}

		/// <summary>
		/// Try adding to the operation queue and return false if it fails.
		/// This way no lock is required on the queue.
		/// </summary>
		private bool EnqueueOperation(BLEOperation op)
		{
			Debug.WriteLine("Enqueing operation ({0})", op.GetType());
			try
			{
				_operationQueue.Add(op);
				return true;
			}
			catch(NullReferenceException)
			{
				return false;
			}
			catch (InvalidOperationException)
			{
				return false;
			}
		}

		public async Task<bool> Initialize(Guid[] serviceIds)
		{
			_subscriptions.Clear();
			_characteristics.Clear();
			try
			{
				foreach (Guid serviceId in serviceIds)
				{
					var service = await _device.GetServiceAsync(serviceId);
					if (service == null) return false;
					var characteristics = await service.GetCharacteristicsAsync();
					foreach (var c in characteristics) _characteristics.Add(c.Id, c);
				}
				return true;
			}
			catch (Exception e)
			{
				Debug.WriteLine("Connection could not be initialized: {0}", e.Message);
				return false;
			}
		}

		private async void DispatchOperation()
		{
			foreach (BLEOperation operation in _operationQueue.GetConsumingEnumerable())
			{
				Debug.WriteLine("Dispatching operation ({0})", operation.GetType());
				try
				{
					await operation.Execute(_operationToken.Token);
				}
				catch(ThreadAbortException)
				{
					return;
				}
				catch(Exception)
				{
					// TODO: retry / notify operation-source / etc.
					continue;
				}
			}
		}

		internal void UpdateState()
		{
			Established = _device.State == DeviceState.Connected;
		}

		public void Subscribe(Guid charId, SubscriptionHandler handler)
		{
			lock (_subscriptions)
			{
				if (!_subscriptions.ContainsKey(charId))
				{
					_subscriptions[charId] = new List<SubscriptionHandler>();
					var characteristic = _characteristics[charId];
					characteristic.ValueUpdated += CharacteristicValueUpdated;
					new SubscriptionOperation(characteristic, subscribe: true);
				}
			}
		}

		public void Unsubscribe(Guid charId, SubscriptionHandler handler)
		{
			lock (_subscriptions)
			{
				if (_subscriptions.ContainsKey(charId))
				{
					_subscriptions[charId].Remove(handler);
					if (_subscriptions.Count == 0)
					{
						var characteristic = _characteristics[charId];
						characteristic.ValueUpdated += CharacteristicValueUpdated;
						new SubscriptionOperation(characteristic, subscribe: true);
					}
				}
			}
		}

		private void CharacteristicValueUpdated(object sender, CharacteristicUpdatedEventArgs e)
		{
			lock (_subscriptions)
			{
				if (_subscriptions.ContainsKey(e.Characteristic.Id))
				{
					foreach (var handler in _subscriptions[e.Characteristic.Id])
					{
						handler(e.Characteristic.Value);
					}
				}
			}
		}
	}

	internal interface BLEOperation
	{
		Task Execute(CancellationToken token);
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
		public async Task Execute(CancellationToken token)
		{
			await Characteristic.WriteAsync(Message, token);
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
		public async Task Execute(CancellationToken token)
		{
			if (Subscribe) await Characteristic.StartUpdatesAsync();
			else await Characteristic.StopUpdatesAsync();
		}
	}

	internal class ReadOperation : BLEOperation
	{
		public ICharacteristic Characteristic;
		private BLEConnection.SubscriptionHandler handler;

		public ReadOperation(ICharacteristic characteristic, BLEConnection.SubscriptionHandler handler)
		{
			Characteristic = characteristic;
			this.handler = handler;
		}

		public async Task Execute(CancellationToken token)
		{
			var data = await Characteristic.ReadAsync();
			handler(data);
		}
	}
}
