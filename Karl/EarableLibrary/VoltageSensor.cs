using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.EventArgs;
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
		public event EventHandler ValueChanged;

		private readonly ICharacteristic _characteristic;

		public VoltageSensor(ICharacteristic characteristic)
		{
			_characteristic = characteristic;
			_characteristic.ValueUpdated += OnValueChanged;
		}

		public void StartSampling()
		{
			_characteristic.StartUpdatesAsync();
		}

		public void StopSampling()
		{
			_characteristic.StopUpdatesAsync();
		}

		protected virtual void OnValueChanged(object sender, CharacteristicUpdatedEventArgs e)
		{
			var message = (eSenseMessage)e.Characteristic.Value;
			var voltage = (message.Data[0] * 256 + message.Data[1]) / 1000f;
			var charging = (message.Data[0] & 1) == 1;
			var args = new VoltageChangedEventArgs(voltage, charging);
			ValueChanged?.Invoke(this, args);

		}
	}
}
