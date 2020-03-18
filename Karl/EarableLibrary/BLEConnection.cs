using Plugin.BLE;
using Plugin.BLE.Abstractions;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.EventArgs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace EarableLibrary
{
	/// <summary>
	/// This class is used for communication with an BLE device.
	/// </summary>
	public class BLEConnection : IDeviceConnection
	{
		private readonly IDevice _device;

		public event EventHandler ConnectionLost;

		private readonly Dictionary<Guid, ICharacteristic> _characteristics = new Dictionary<Guid, ICharacteristic>();

		private readonly Dictionary<Guid, List<DataReceiver>> _subscriptions = new Dictionary<Guid, List<DataReceiver>>();

		/// <summary>
		/// Whether this connection is open, closed or in a state inbetween.
		/// </summary>
		public ConnectionState State
		{
			get
			{
				if (_device == null) return ConnectionState.Disconnected;
				switch (_device.State)
				{
					case DeviceState.Connected: return ConnectionState.Connected;
					case DeviceState.Connecting: return ConnectionState.Connecting;
					default: return ConnectionState.Disconnected;
				}
			}
		}

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
			CrossBluetoothLE.Current.Adapter.DeviceConnectionLost += OnDeviceConnectionLost;
		}

		private void OnDeviceConnectionLost(object sender, DeviceErrorEventArgs e)
		{
			if (_device.Equals(e.Device))
			{
				Debug.WriteLine("Device connection lost!");
				_characteristics.Clear();
				ConnectionLost.Invoke(this, null);
			}
		}

		/// <summary>
		/// Asynchronously open the connection.
		/// </summary>
		/// <returns>Whether the connection could be opened or not</returns>
		public async Task<bool> Open()
		{
			if (State != ConnectionState.Disconnected) return false;
			try
			{
				var parameters = new ConnectParameters(forceBleTransport: true);
				await CrossBluetoothLE.Current.Adapter.ConnectToDeviceAsync(_device, parameters);
				await ReloadCharacteristics();
				return true;
			}
			catch (Exception e)
			{
				Debug.WriteLine("Connection could not be opened: {0}", args: e.Message);
				if (State != ConnectionState.Disconnected)
				{
					await Close();
				}
				return false;
			}
		}

		/// <summary>
		/// Asynchronously close the connection.
		/// </summary>
		/// <returns>Whether the connection was closed or not</returns>
		public async Task<bool> Close()
		{
			if (State != ConnectionState.Connected) return false;
			try
			{
				// Unsubscribe everything
				await UnsubscribeAll();
				await CrossBluetoothLE.Current.Adapter.DisconnectDeviceAsync(_device);
				_characteristics.Clear();
				_subscriptions.Clear();
				return true;
			}
			catch (Exception e)
			{
				Debug.WriteLine("Could not disconnect: {0}", args: e.Message);
				return false;
			}
		}

		/// <summary>
		/// Write to a characteristic.
		/// </summary>
		/// <param name="charId">Characteristic id</param>
		/// <param name="val">Value to write</param>
		/// <returns>false if the connection doesn't allow write-operations right now</returns>
		public async Task<bool> WriteAsync(Guid charId, byte[] val)
		{
			var completitionSource = new TaskCompletionSource<bool>();
			Device.BeginInvokeOnMainThread(async () =>
			{
				try
				{
					completitionSource.SetResult(await _characteristics[charId].WriteAsync(val));
				}
				catch (Exception)
				{
					//completitionSource.SetException(e);
					completitionSource.SetResult(false);
				}
			});
			return await completitionSource.Task;
		}

		/// <summary>
		/// Reading from a characteristic.
		/// </summary>
		/// <param name="charId">Characteristic id</param>
		/// <returns>Current value of the characteristic</returns>
		public async Task<byte[]> ReadAsync(Guid charId)
		{
			try
			{
				return await _characteristics[charId].ReadAsync();
			}
			catch(Exception)
			{
				return null;
			}
		}

		/// <summary>
		/// Subscribe to a characteristic.
		/// </summary>
		/// <param name="charId">Characteristic id</param>
		/// <param name="handler">Handler to be notified when an update occurs</param>
		public async Task<bool> SubscribeAsync(Guid charId, DataReceiver handler)
		{
			try
			{
				if (!_subscriptions.ContainsKey(charId))
				{
					_subscriptions[charId] = new List<DataReceiver>();
					var characteristic = _characteristics[charId];
					characteristic.ValueUpdated += CharacteristicValueUpdated;
					await characteristic.StartUpdatesAsync();
				}
				_subscriptions[charId].Add(handler);
				return true;
			}
			catch(Exception)
			{
				return false;
			}
		}

		/// <summary>
		/// Unsubscribe from a characteristic.
		/// </summary>
		/// <param name="charId">Characteristic id</param>
		/// <param name="handler">Handler that has been previously subscribed</param>
		/// <exception cref="KeyNotFoundException">If a characteristic with the given id has not been loaded</exception>
		public async Task<bool> UnsubscribeAsync(Guid charId, DataReceiver handler)
		{
			try
			{
				if (_subscriptions.ContainsKey(charId))
				{
					_subscriptions[charId].Remove(handler);
					if (_subscriptions[charId].Count == 0)
					{
						_subscriptions.Remove(charId);
						var characteristic = _characteristics[charId];
						characteristic.ValueUpdated -= CharacteristicValueUpdated;
						await characteristic.StopUpdatesAsync();
					}
				}
				return true;
			}
			catch (Exception)
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

		private async Task ReloadCharacteristics()
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
			if (_subscriptions.ContainsKey(e.Characteristic.Id))
			{
				foreach (var handler in _subscriptions[e.Characteristic.Id])
				{
					handler(e.Characteristic.Value);
				}
			}
		}

		/// <summary>
		/// Validate this connection based on whether the given services are available or not.
		/// </summary>
		/// <param name="serviceIds">Service ids to check for</param>
		/// <returns>true if all given services are available, false otherwise</returns>
		public async Task<bool> Validate(Guid[] serviceIds)
		{
			try
			{
				foreach (Guid serviceId in serviceIds)
				{
					var service = await _device.GetServiceAsync(serviceId);
					if (service == null) return false;
				}
				return true;
			}
			catch(Exception)
			{
				return false;
			}
		}
	}
}
