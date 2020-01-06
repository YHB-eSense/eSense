using Plugin.BLE;
using Plugin.BLE.Abstractions;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace EarableLibrary
{
	public class ESense : IEarable
	{
		private readonly IDevice _device;
		private readonly IAudioStream _audioStream;

		public ESense(IDevice device)
		{
			this._device = device;
		}

		public string Name
		{
			get
			{
				return _device.Name;
			}
			set
			{
				throw new NotImplementedException();
			}
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
			IReadOnlyList<IService> services = await _device.GetServicesAsync();
			foreach (IService s in services)
			{
				_ = s.Id;
			}
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
