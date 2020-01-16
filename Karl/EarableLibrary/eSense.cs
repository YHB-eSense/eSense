using Plugin.BLE;
using Plugin.BLE.Abstractions;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.Extensions;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Diagnostics;

namespace EarableLibrary
{
	public class ESense : IEarable
	{
		private static readonly Guid SER_GENERIC = GuidExtension.UuidFromPartial(0x1800);
		private static readonly Guid CHAR_NAME_R = GuidExtension.UuidFromPartial(0x2A00);
		private static readonly Guid SER_ESENSE = GuidExtension.UuidFromPartial(0xFF06);
		private static readonly Guid CHAR_IMU_ENABLE = GuidExtension.UuidFromPartial(0xFF07);
		private static readonly Guid CHAR_IMU_DATA = GuidExtension.UuidFromPartial(0xFF08);
		private static readonly Guid CHAR_BUTTON = GuidExtension.UuidFromPartial(0xFF09);
		private static readonly Guid CHAR_VOLTAGE = GuidExtension.UuidFromPartial(0xFF0A);
		private static readonly Guid CHAR_INTERVALS = GuidExtension.UuidFromPartial(0xFF0B);
		private static readonly Guid CHAR_NAME_W = GuidExtension.UuidFromPartial(0xFF0C);
		private static readonly Guid CHAR_IMU_OFFSET = GuidExtension.UuidFromPartial(0xFF0D);
		private static readonly Guid CHAR_IMU_CONFIG = GuidExtension.UuidFromPartial(0xFF0E);

		public static Guid[] RequiredServiceUuids = { SER_GENERIC, SER_ESENSE };

		private readonly IDevice _device;

		private EarableName _name;

		public ESense(IDevice device)
		{
			_device = device;
		}

		public string Name
		{
			get => _name.Value;
			set => _name.Value = value;
		}

		public Guid Id => _device.Id;

		private Dictionary<Type, ISensor> _sensors;

		private async Task<Dictionary<Guid, ICharacteristic>> GetCharacteristicsAsync(params Guid[] serviceIds)
		{
			var dict = new Dictionary<Guid, ICharacteristic>();
			foreach (Guid serviceId in serviceIds)
			{
				var service = await _device.GetServiceAsync(serviceId);
				if (service == null) throw new NotSupportedException(string.Format("Service {0} not found", serviceId));
				var characteristics = await service.GetCharacteristicsAsync();
				foreach (var c in characteristics) dict.Add(c.Id, c);
			}
			return dict;
		}

		private Dictionary<Type, ISensor> CreateSensors(Dictionary<Guid, ICharacteristic> c)
		{
			var dict = new Dictionary<Type, ISensor>
			{
				{ typeof(MotionSensor), new MotionSensor(data: c[CHAR_IMU_DATA], enable: c[CHAR_IMU_ENABLE], config: c[CHAR_IMU_CONFIG]) },
				{ typeof(PushButton), new PushButton(c[CHAR_BUTTON]) },
				{ typeof(VoltageSensor), new VoltageSensor(c[CHAR_VOLTAGE]) }
			};
			return dict;
		}

		protected async Task Initialize()
		{
			try
			{
				var characteristics = await GetCharacteristicsAsync(SER_GENERIC, SER_ESENSE);
				_name = new EarableName(characteristics[CHAR_NAME_R], characteristics[CHAR_NAME_W]);
				_sensors = CreateSensors(characteristics);
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
				await Initialize();
				Debug.WriteLine("Now connected to {0}", Name);
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

		public T GetSensor<T>() where T : ISensor
		{
			return (T)_sensors[typeof(T)];
		}
	}
}
