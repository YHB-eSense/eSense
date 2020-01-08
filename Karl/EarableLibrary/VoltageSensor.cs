using Plugin.BLE.Abstractions.Contracts;
using System;

namespace EarableLibrary
{
	public class VoltageChangedEventArgs : EventArgs
	{
		public VoltageChangedEventArgs(float voltage, bool charging)
		{
			Voltage = voltage;
			Charging = charging;
		}
		public float Voltage { get; }
		public bool Charging { get; }
	}

	public class VoltageSensor : ISensor
	{
		public float Voltage { get; private set; }

		public bool Charging { get; private set; }

		private readonly ICharacteristic _characteristic;

		public VoltageSensor(ICharacteristic characteristic)
		{
			_characteristic = characteristic;
			Voltage = -1;
			Charging = false;
		}

		public async void UpdateValueAsync()
		{
			var bytes = await _characteristic.ReadAsync();
			var message = (eSenseMessage)bytes;
			Voltage = (message.Data[0] * 256 + message.Data[1]) / 1000f;
			Charging = (message.Data[0] & 1) == 1;

		}
	}
}
