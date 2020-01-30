using Plugin.BLE.Abstractions.Extensions;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Diagnostics;

namespace EarableLibrary
{
	/// <summary>
	/// Implementation of IEarable for eSense headphones (https://www.esense.io/).
	/// </summary>
	public class ESense : IEarable
	{
		// Service ids from the BLE specification (https://www.esense.io/share/eSense-BLE-Specification.pdf)
		private static readonly Guid SER_GENERIC = GuidExtension.UuidFromPartial(0x1800);
		private static readonly Guid SER_ESENSE = GuidExtension.UuidFromPartial(0xFF06);

		private readonly Dictionary<Type, ISensor> _sensors = new Dictionary<Type, ISensor>();

		private readonly BLEConnection _conn;

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
		public ESense(BLEConnection conn)
		{
			_conn = conn;
		}

		/// <summary>
		/// Get device name.
		/// </summary>
		public string Name
		{
			get => _name.Get();
			set => _name.Set(value);
		}

		/// <summary>
		/// Device id.
		/// </summary>
		public Guid Id => _conn.Id;

		/// <summary>
		/// Connect to the device and initialize sensors.
		/// </summary>
		/// <returns>true if successful, false otherwise</returns>
		public async Task<bool> ConnectAsync()
		{
			if (await _conn.Open())
			{
				if (!await _conn.Validate(ServiceUuids)) return false;
				_name = new EarableName(_conn);
				_sensors.Clear();
				AddSensor(new MotionSensor(_conn));
				AddSensor(new PushButton(_conn));
				AddSensor(new VoltageSensor(_conn));
				Debug.WriteLine("Now connected to {0}", args: Name);
				return true;
			}
			return false;
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
		public ConnectionState State => _conn.State;

		/// <summary>
		/// Retrieve one of the builtin sensors.
		/// </summary>
		/// <typeparam name="T">Type of the sensor</typeparam>
		/// <returns>Object representing the requested sensor type, null if there is no such sensor builtin</returns>
		public T GetSensor<T>() where T : ISensor
		{
			return (T)_sensors[typeof(T)];
		}

		private void AddSensor(ISensor sens)
		{
			_sensors[sens.GetType()] = sens;
		}
	}
}
