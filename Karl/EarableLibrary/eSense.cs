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
				if (_name == null) return _device.Name;
				return _name;
			}
			set
			{
				_name = value;
				//updateCharacteristic();
			}
		}

		public Guid Id => _device.Id;

		public IAudioStream AudioStream => _audioStream;

		public ReadOnlyCollection<ISensor<Object>> Sensors { get; private set; }

		private async Task<ICharacteristic[]> GetCharacteristicsAsync(int service, params int[] characteristics)
		{
			var s = await _device.GetServiceAsync(GuidExtension.UuidFromPartial(service));
			if (s == null) throw new NotSupportedException();
			ICharacteristic[] result = new ICharacteristic[characteristics.Length];
			for (int i = 0; i < characteristics.Length; i++)
			{
				result[i] = await s.GetCharacteristicAsync(GuidExtension.UuidFromPartial(characteristics[i]));
				if (result[i] == null) throw new NotSupportedException();
			} 
			return result;
		}

		private async Task<IList<ISensor<Object>>> CreateSensors()
		{
			ICharacteristic[] imuChars = await GetCharacteristicsAsync(CHAR_IMU_DATA, CHAR_IMU_ENABLE, CHAR_IMU_CONFIG);
			IList<ISensor<Object>> l = new List<ISensor<Object>>
			{
				(ISensor<Object>)new MotionSensor(imuChars[0], imuChars[1], imuChars[2]),
				// PushButton
				// VoltageSensor
			};
			return l;
		}

		public async Task Initialize()
		{
			Debug.WriteLine("Trying to initialize: " + _device.Name);
			try
			{
				_name = (await GetCharacteristicsAsync(SER_GENERIC, CHAR_NAME_R))[0].StringValue;
				Sensors = new ReadOnlyCollection<ISensor<Object>>(await CreateSensors());
				_validated = true;
			}
			catch (NotSupportedException e)
			{
				Debug.WriteLine("Unsupported Device!");
				Debug.WriteLine(e);
				return;
			}
			return;
		}

		public async Task<bool> ConnectAsync()
		{
			if (!CanConnect()) return false;
			try
			{
				await CrossBluetoothLE.Current.Adapter.ConnectToDeviceAsync(_device);
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
			await CrossBluetoothLE.Current.Adapter.DisconnectDeviceAsync(_device);
			return true;
		}

		public bool IsConnected()
		{
			return _validated && _device.State == DeviceState.Connected;
		}

		public bool CanConnect()
		{
			return _validated;
		}
	}
}
