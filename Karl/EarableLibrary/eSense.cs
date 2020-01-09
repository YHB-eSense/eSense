using Plugin.BLE;
using Plugin.BLE.Abstractions;
using Plugin.BLE.Abstractions.Contracts;
using System.Collections.ObjectModel;
using Plugin.BLE.Abstractions.Extensions;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

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

		public ReadOnlyDictionary<Type, ISensor> Sensors { get; private set; }

		private async Task<Dictionary<Guid, ICharacteristic>> GetCharacteristicsAsync(Guid serviceId)
		{
			var dict = new Dictionary<Guid, ICharacteristic>();
			var service = await _device.GetServiceAsync(serviceId);
			if (service == null) throw new NotSupportedException(String.Format("Service {0} not found", serviceId));
			var characteristics = await service.GetCharacteristicsAsync();
			foreach (var c in characteristics) dict.Add(c.Id, c);
			return dict;
		}

		private async Task<Dictionary<Type, ISensor>> CreateSensors()
		{
			var c = await GetCharacteristicsAsync(SER_ESENSE);
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
				//var name = (await GetCharacteristicsAsync(SER_GENERIC))[CHAR_NAME_R];
				//_name = Encoding.UTF8.GetString(await name.ReadAsync());
				Sensors = new ReadOnlyDictionary<Type, ISensor>(await CreateSensors());
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

		public bool IsValid()
		{
			return _validated;
		}
	}
}
