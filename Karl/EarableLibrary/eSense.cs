using Plugin.BLE;
using Plugin.BLE.Abstractions;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace EarableLibrary
{
	public class ESense : IEarable
	{
		private IDevice device;

		public ESense(IDevice device)
		{
			this.device = device;
		}

		public string Name { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

		public Guid Id => throw new NotImplementedException();

		public IAudioStream AudioStream => throw new NotImplementedException();

		public ReadOnlyCollection<ISensor> Sensors => throw new NotImplementedException();

		public async System.Threading.Tasks.Task<bool> ConnectAsync()
		{
			try
			{
				await CrossBluetoothLE.Current.Adapter.ConnectToDeviceAsync(device);
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
			IReadOnlyList<IService> services = await device.GetServicesAsync();
			foreach (IService s in services)
			{
				_ = s.Id;
			}
			return true;
		}

		public async System.Threading.Tasks.Task<bool> DisconnectAsync()
		{
			if (!IsConnected()) return false;
			await CrossBluetoothLE.Current.Adapter.DisconnectDeviceAsync(device);
			return true;
		}

		public bool IsConnected()
		{	
			return device.State == DeviceState.Connected;
		}
	}
}
