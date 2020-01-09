using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.EventArgs;
using System;
using System.Threading.Tasks;

namespace EarableLibrary
{

	public class ButtonArgs : EventArgs
	{
		public ButtonArgs(bool pressed)
		{
			Pressed = pressed;
		}
		public bool Pressed { get; }
	}

	public class PushButton : ISubscribableSensor<ButtonArgs>
	{
		public event EventHandler<ButtonArgs> ValueChanged;

		private readonly ICharacteristic _characteristic;

		public PushButton(ICharacteristic characteristic)
		{
			_characteristic = characteristic;
			_characteristic.ValueUpdated += OnValueChanged;
		}

		public async Task StartSamplingAsync()
		{
			await _characteristic.StartUpdatesAsync();
		}

		public async Task StopSamplingAsync()
		{
			await _characteristic.StopUpdatesAsync();
		}

		protected virtual void OnValueChanged(object sender, CharacteristicUpdatedEventArgs e)
		{
			var message = new eSenseMessage(received: e.Characteristic.Value);
			var pushed = (message.Data[0] & 1) == 1;
			var args = new ButtonArgs(pushed);
			ValueChanged?.Invoke(this, args);
		}
	}
}
