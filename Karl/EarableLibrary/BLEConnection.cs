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

		private readonly ConcurrentDictionary<Guid, ICharacteristic> _characteristics = new ConcurrentDictionary<Guid, ICharacteristic>();

		private readonly ConcurrentDictionary<Guid, List<SubscriptionHandler>> _subscriptions = new ConcurrentDictionary<Guid, List<SubscriptionHandler>>();

		private readonly CancellationTokenSource _connectionToken = new CancellationTokenSource();

		private readonly CancellationTokenSource _operationToken = new CancellationTokenSource();

		private BlockingCollection<BLEOperation> _operationQueue;

		private Thread _messageDispatcher;

		public delegate void SubscriptionHandler(byte[] data);

		/// <summary>
		/// Whether this connection is currently open or not.
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
			if (State != ConnectionState.Disconnected) return false;
			State = ConnectionState.Connecting;
			try
			{
				_operationQueue = new BlockingCollection<BLEOperation>();
				_messageDispatcher = new Thread(new ThreadStart(DispatchOperation))
				{
					IsBackground = true
				};
				var parameters = new ConnectParameters(forceBleTransport: true);
				await CrossBluetoothLE.Current.Adapter.ConnectToDeviceAsync(_device, parameters, _connectionToken.Token);
				State = ConnectionState.Connected;
				_messageDispatcher.Start();
				return true;
			}
			catch (Exception e)
			{
				State = ConnectionState.Disconnected;
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
			if (State == ConnectionState.Connecting)
			{
				_connectionToken.Cancel(); // cancel current connection process
			}
			State = ConnectionState.Disconnecting;
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
			// Cancel subscriptions
			foreach (var subscription in _subscriptions)
			{
				if (subscription.Value.Count == 0) continue;
				var characteristic = _characteristics[subscription.Key];
				characteristic.ValueUpdated -= CharacteristicValueUpdated;
				await characteristic.StopUpdatesAsync();
			}
			try
			{
				await CrossBluetoothLE.Current.Adapter.DisconnectDeviceAsync(_device);
				State = ConnectionState.Disconnected;
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
					foreach (var c in characteristics) _characteristics[c.Id] = c;
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
			switch (_device.State)
			{
				case DeviceState.Connected:
					State = ConnectionState.Connected;
					break;
				case DeviceState.Connecting:
					State = ConnectionState.Connecting;
					break;
				default:
					State = ConnectionState.Disconnected;
					break;
			}
		}

		public void Subscribe(Guid charId, SubscriptionHandler handler)
		{
			if (!_subscriptions.ContainsKey(charId))
			{
				_subscriptions[charId] = new List<SubscriptionHandler>();
				var characteristic = _characteristics[charId];
				characteristic.ValueUpdated += CharacteristicValueUpdated;
				EnqueueOperation(new SubscriptionOperation(characteristic, subscribe: true));
			}
			_subscriptions[charId].Add(handler);
		}

		public void Unsubscribe(Guid charId, SubscriptionHandler handler)
		{
			if (_subscriptions.ContainsKey(charId))
			{
				_subscriptions[charId].Remove(handler);
				if (_subscriptions.Count == 0)
				{
					var characteristic = _characteristics[charId];
					characteristic.ValueUpdated += CharacteristicValueUpdated;
					EnqueueOperation(new SubscriptionOperation(characteristic, subscribe: false));
				}
			}
		}

		private void CharacteristicValueUpdated(object sender, CharacteristicUpdatedEventArgs e)
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
