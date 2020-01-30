using Plugin.BLE.Abstractions.Extensions;
using System;
using System.Threading.Tasks;

namespace EarableLibrary
{
	/// <summary>
	/// Groups multiple properties into one "battery-state".
	/// </summary>
	public struct BatteryState
	{
		/// <summary>
		/// Current battery voltage.
		/// </summary>
		public float Voltage;
		/// <summary>
		/// Whether the battery is charging (true) or discharging (false).
		/// </summary>
		public bool Charging;
	}

	/// <summary>
	/// Sensor which can measure the batterie's current state.
	/// </summary>
	public class VoltageSensor : IReadableSensor<BatteryState>
	{
		private static readonly Guid CHAR_VOLTAGE = GuidExtension.UuidFromPartial(0xFF0A);

		private BLEConnection _conn;

		/// <summary>
		/// Construct a new VoltageSensor.
		/// </summary>
		/// <param name="read">Characteristic, which provides read-access to the current battery state</param>
		public VoltageSensor(BLEConnection conn)
		{
			_conn = conn;
		}

		private void OnReadCompleted(byte[] data)
		{
			var message = new ESenseMessage();
			message.Decode(data);
			new BatteryState()
			{
				Voltage = (message.Data[0] * 256 + message.Data[1]) / 1000f,
				Charging = (message.Data[2] & 1) == 1
			};
		}
	}
}
