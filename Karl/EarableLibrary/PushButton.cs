using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.EventArgs;
using System;

namespace EarableLibrary
{

	public class PushButtonChangedEventArgs : EventArgs
	{
		public PushButtonChangedEventArgs(bool state)
		{
			Pushed = state;
		}
		public bool Pushed { get; }
	}

	public class PushButton : ISensor
	{
		public event EventHandler ValueChanged;

		private readonly ICharacteristic _characteristic;

		public PushButton(ICharacteristic characteristic)
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
			var pushed = (message.Data[0] & 1) == 1;
			var args = new PushButtonChangedEventArgs(pushed);
			ValueChanged?.Invoke(this, args);
		}
	}
}
