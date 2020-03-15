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

		public override string ToString()
		{
			return string.Format("<BatteryState(Voltage={0},Charging={1})>", Voltage, Charging);
		}
	}

	/// <summary>
	/// Sensor which can measure the batterie's current state.
	/// </summary>
	public class VoltageSensor : IReadableSensor<BatteryState>
	{
		private static readonly Guid CHAR_VOLTAGE = GuidExtension.UuidFromPartial(0xFF0A);

		private readonly IDeviceConnection _conn;

		/// <summary>
		/// Construct a new VoltageSensor.
		/// </summary>
		/// <param name="read">Characteristic, which provides read-access to the current battery state</param>
		public VoltageSensor(IDeviceConnection conn)
		{
			_conn = conn;
		}
		/// <summary>
		/// Retrieve the current battery state.
		/// </summary>
		/// <returns>Current battery state</returns>
		public async Task<BatteryState> ReadAsync()
		{
			return ParseMessage(await _conn.ReadAsync(CHAR_VOLTAGE));
		}

		private BatteryState ParseMessage(byte[] data)
		{
			var message = new ESenseMessage();
			message.Decode(data);
			return new BatteryState()
			{
				Voltage = (message.Data[0] * 256 + message.Data[1]) / 1000f,
				Charging = (message.Data[2] & 1) == 1
			};
		}
	}
}
