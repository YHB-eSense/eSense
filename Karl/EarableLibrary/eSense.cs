using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace EarableLibrary
{
	/// <summary>
	/// Implementation of IEarable for eSense headphones (https://www.esense.io/).
	/// </summary>
	public class ESense : IEarable
	{
		// Service and characteristic ids from the BLE specification (https://www.esense.io/share/eSense-BLE-Specification.pdf)
		private static readonly Guid SER_GENERIC = GuidExtension.UuidFromPartial(0x1800);
		private static readonly Guid SER_ESENSE = GuidExtension.UuidFromPartial(0xFF06);

		private readonly BLEConnection _conn;

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
			_conn = new BLEConnection(device);
		}

		/// <summary>
		/// Get device name.
		/// </summary>
		public string Name
		{
			get => _name.Get();
		}

		/// <summary>
		/// Asynchronously set a new device name.
		/// </summary>
		/// <param name="name">New name</param>
		/// <returns>Whether the name could be set successfully or not</returns>
		public async Task<bool> SetNameAsync(string name)
		{
			return await _name.SetAsync(name);
		}

		/// <summary>
		/// Device id.
		/// </summary>
		public Guid Id => _conn.Id;

		/// <summary>
		/// Called after the connection has been established.
		/// Can be used to initialize sensors and load device properties.
		/// </summary>
		protected async Task<bool> InitializeConnection()
		{
			try
			{
				_name = new EarableName(_conn);
				await _name.Initialize();
				_sensors = CreateSensors();
				return true;
			}
			catch (Exception e)
			{
				if (Name != null && Name.Length > 0) Debug.WriteLine("Unsupported Device: {0} ({1})", Name, e.Message);
				return false;
			}
		}

		/// <summary>
		/// Connect to the device and initialize sensors.
		/// </summary>
		/// <returns>true if successful, false otherwise</returns>
		public async Task<bool> ConnectAsync()
		{
			var status = await _conn.Open();
			if (status) status = await InitializeConnection();
			if (IsConnected()) await _conn.Close();
			return status;
		}

		/// <summary>
		/// Disconnect from the device.
		/// </summary>
		/// <returns>true if successful, false otherwise</returns>
		public async Task<bool> DisconnectAsync()
		{
			return await _conn.Close();
		}

		/// <summary>
		/// Retrieve current connection state.
		/// </summary>
		/// <returns>Whether the device is currently connected or not</returns>
		public bool IsConnected()
		{
			return _conn.State == ConnectionState.Connected;
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

		private Dictionary<Type, ISensor> CreateSensors()
		{
			var dict = new Dictionary<Type, ISensor>
			{
				{ typeof(MotionSensor), new MotionSensor(_conn) },
				{ typeof(PushButton), new PushButton(_conn) },
				{ typeof(VoltageSensor), new VoltageSensor(_conn) }
			};
			return dict;
		}
	}
}
