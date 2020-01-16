using Plugin.BLE.Abstractions.Contracts;
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
		private readonly ICharacteristic _read;


		/// <summary>
		/// Construct a new VoltageSensor.
		/// </summary>
		/// <param name="read">Characteristic, which provides read-access to the current battery state</param>
		public VoltageSensor(ICharacteristic read)
		{
			_read = read;
		}

		/// <summary>
		/// Query the earable for its current battery state.
		/// </summary>
		/// <returns>Current battery state</returns>
		public async Task<BatteryState> ReadAsync()
		{
			var bytes = await _read.ReadAsync();
			var message = new ESenseMessage(received: bytes);
			return new BatteryState()
			{
				Voltage = (message.Data[0] * 256 + message.Data[1]) / 1000f,
				Charging = (message.Data[0] & 1) == 1
			};
		}
	}
}
