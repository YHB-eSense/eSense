using Plugin.BLE;
using Plugin.BLE.Abstractions;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.Extensions;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace EarableLibrary
{
	/// <summary>
	/// Implementation of IEarable for eSense headphones (https://www.esense.io/).
	/// </summary>
	public class ESense : IEarable
	{
		// Service and characteristic ids from the BLE specification (https://www.esense.io/share/eSense-BLE-Specification.pdf)
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

		private readonly IDevice _device;

		private Dictionary<Type, ISensor> _sensors;

		private EarableName _name;

		/// <summary>
		/// Ids of all services which are used for BLE communication with the earables.
		/// If one of these services is not present, communication will most likely fail.
		/// </summary>
		public static Guid[] ServiceUuids = { SER_GENERIC, SER_ESENSE };

		/// <summary>
		/// Construct a new ESense object.
		/// </summary>
		/// <param name="device">BLE handle used for communication</param>
		public ESense(IDevice device)
		{
			_device = device;
		}

		/// <summary>
		/// Device name.
		/// </summary>
		public string Name
		{
			get => _name.Get();
			set => _name.SetAsync(value).ConfigureAwait(false);
		}

		/// <summary>
		/// Device id.
		/// </summary>
		public Guid Id => _device.Id;

		/// <summary>
		/// Called after the connection has been established.
		/// Can be used to initialize sensors and load device properties.
		/// </summary>
		protected async Task InitializeConnection()
		{
			try
			{
				var characteristics = await GetCharacteristicsAsync(ServiceUuids);
				_name = new EarableName(characteristics[CHAR_NAME_R], characteristics[CHAR_NAME_W]);
				_sensors = CreateSensors(characteristics);
			}
			catch (Exception e)
			{
				if (_device.Name != null && _device.Name.Length > 0) Debug.WriteLine("Unsupported Device: " + _device.Name + " / " + e.Message);
			}
			return;
		}

		/// <summary>
		/// Connect to the device and initialize sensors.
		/// </summary>
		/// <returns>true if successful, false otherwise</returns>
		public async Task<bool> ConnectAsync()
		{
			try
			{
				var parameters = new ConnectParameters(forceBleTransport: true);
				await CrossBluetoothLE.Current.Adapter.ConnectToDeviceAsync(_device, parameters);
				await InitializeConnection();
				Debug.WriteLine("Now connected to {0}", Name);
			}
			catch (Exception)
			{
				return false;
			}
			return true;
		}

		/// <summary>
		/// Disconnect from the device.
		/// </summary>
		/// <returns>true if successful, false otherwise</returns>
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

		/// <summary>
		/// Retrieve current connection state.
		/// </summary>
		/// <returns>Whether the device is currently connected or not</returns>
		public bool IsConnected()
		{
			return _device.State == DeviceState.Connected;
		}

		/// <summary>
		/// Retrieve one of the builtin sensors.
		/// </summary>
		/// <typeparam name="T">Type of the sensor</typeparam>
		/// <returns>Object representing the requested sensor type, null if there is no such sensor builtin</returns>
		public T GetSensor<T>() where T : ISensor
		{
			return (T)_sensors[typeof(T)];
		}

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
	}
}
