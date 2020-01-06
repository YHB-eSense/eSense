using Plugin.BLE;
using Plugin.BLE.Abstractions;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.Exceptions;
using System.Collections.ObjectModel;
using Plugin.BLE.Abstractions.Extensions;
using System;

namespace EarableLibrary
{
	public class ESense : IEarable
	{
		private readonly IDevice _device;
		private readonly IAudioStream _audioStream;

		private string _name;

		public ESense(IDevice device)
		{
			this._device = device;
		}

		public string Name
		{
			get
			{
				if (_name == null) return _device.Name;
				return _name;
			}
			set
			{
				_name = value;
				//updateCharacteristic();
			}
		}

		private async System.Threading.Tasks.Task<ICharacteristic> getCharacteristicAsync(int service, int characteristic)
		{
			var s = await _device.GetServiceAsync(GuidExtension.UuidFromPartial(service));
			if (s == null) return null;
			return await s.GetCharacteristicAsync(GuidExtension.UuidFromPartial(characteristic));
		}

		public Guid Id => _device.Id;

		public IAudioStream AudioStream => _audioStream;

		public readonly ReadOnlyCollection<ISensor<Object>> Sensors;

		public async System.Threading.Tasks.Task<bool> ConnectAsync()
		{
			try
			{
				await CrossBluetoothLE.Current.Adapter.ConnectToDeviceAsync(_device);
			}
			catch(DeviceConnectionException)
			{
				return false;
			}
			catch(Exception)
			{
				return false;
			}
			return true;
		}

		public async System.Threading.Tasks.Task<bool> ValidateServicesAsync()
		{
			ICharacteristic c;
			c = await getCharacteristicAsync(0x1800, 0x2A00);
			if (c != null) _name = c.StringValue;
			//else return false;
			return true;
		}

		public async System.Threading.Tasks.Task<bool> DisconnectAsync()
		{
			if (!IsConnected()) return false;
			await CrossBluetoothLE.Current.Adapter.DisconnectDeviceAsync(_device);
			return true;
		}

		public bool IsConnected()
		{	
			return _device.State == DeviceState.Connected;
		}
	}
}
