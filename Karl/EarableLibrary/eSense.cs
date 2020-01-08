using Plugin.BLE;
using Plugin.BLE.Abstractions;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.Exceptions;
using System.Collections.ObjectModel;
using Plugin.BLE.Abstractions.Extensions;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Diagnostics;

namespace EarableLibrary
{
	public class ESense : IEarable
	{
		private static readonly int SER_ESENSE = 0xFF06;
		private static readonly int CHAR_IMU_ENABLE = 0xFF07;
		private static readonly int CHAR_IMU_DATA = 0xFF08;
		private static readonly int CHAR_BUTTON = 0xFF09;
		private static readonly int CHAR_VOLTAGE = 0xFF0A;
		private static readonly int CHAR_INTERVALS = 0xFF0B;
		private static readonly int CHAR_NAME_W = 0xFF0C;
		private static readonly int CHAR_IMU_OFFSET = 0xFF0D;
		private static readonly int CHAR_IMU_CONFIG = 0xFF0E;
		private static readonly int SER_GENERIC = 0x1800;
		private static readonly int CHAR_NAME_R = 0x2A00;

		private readonly IDevice _device;
		private readonly IAudioStream _audioStream;

		private string _name;
		private bool _validated;

		public ESense(IDevice device)
		{
			_device = device;
			_validated = false;
		}

		public string Name
		{
			get
			{
				//if (_name != null) return _name;
				return _device.Name;
			}
			set
			{
				_name = value;
				//updateCharacteristic();
			}
		}

		public Guid Id => _device.Id;

		public IAudioStream AudioStream => _audioStream;

		public ReadOnlyCollection<ISensor> Sensors { get; private set; }

		private async Task<ICharacteristic[]> GetCharacteristicsAsync(int serviceId, params int[] characteristicIds)
		{
			Guid uuid;
			uuid = GuidExtension.UuidFromPartial(serviceId);
			var s = await _device.GetServiceAsync(uuid);
			if (s == null) throw new NotSupportedException("Service " + uuid + " not found");
			ICharacteristic[] result = new ICharacteristic[characteristicIds.Length];
			for (int i = 0; i < characteristicIds.Length; i++)
			{
				uuid = GuidExtension.UuidFromPartial(characteristicIds[i]);
				result[i] = await s.GetCharacteristicAsync(uuid);
				if (result[i] == null) throw new NotSupportedException("Characteristic " + uuid + " not found");
			} 
			return result;
		}

		private async Task<IList<ISensor>> CreateSensors()
		{
			ICharacteristic[] imu = await GetCharacteristicsAsync(SER_ESENSE, CHAR_IMU_DATA, CHAR_IMU_ENABLE, CHAR_IMU_CONFIG);
			IList<ISensor> list = new List<ISensor>
			{
				new MotionSensor(data: imu[0], enable: imu[1], config: imu[2])
				// PushButton
				// VoltageSensor
			};
			return list;
		}

		public async Task Initialize()
		{
			try
			{
				_name = (await GetCharacteristicsAsync(SER_GENERIC, CHAR_NAME_R))[0].StringValue;
				Sensors = new ReadOnlyCollection<ISensor>(await CreateSensors());
				_validated = true;
			}
			catch (Exception e)
			{
				if (_device.Name != null && _device.Name.Length > 0) Debug.WriteLine("Unsupported Device: " + _device.Name + " / " + e.Message);
			}
			return;
		}

		public async Task<bool> ConnectAsync()
		{
			try
			{
				var parameters = new ConnectParameters(forceBleTransport: true);
				await CrossBluetoothLE.Current.Adapter.ConnectToDeviceAsync(_device, parameters);
			}
			catch (DeviceConnectionException)
			{
				return false;
			}
			catch (Exception)
			{
				return false;
			}
			return true;
		}

		public async Task<bool> DisconnectAsync()
		{
			if (!IsConnected()) return false;
			try
			{
				await CrossBluetoothLE.Current.Adapter.DisconnectDeviceAsync(_device);
			}
			catch (Exception)
			{
				return false;
			}
			return true;
		}

		public bool IsConnected()
		{
			return _device.State == DeviceState.Connected;
		}

		public bool IsValid()
		{
			return _validated;
		}
	}
}
