using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.EventArgs;
using System;
using System.Threading.Tasks;

namespace EarableLibrary
{
	/// <summary>
	/// Represents a state which one button can be in.
	/// </summary>
	public class ButtonState : EventArgs
	{
		/// <summary>
		/// Constructs a new ButtonState.
		/// </summary>
		/// <param name="pressed"></param>
		public ButtonState(bool pressed)
		{
			Pressed = pressed;
		}

		/// <summary>
		/// Whether the button is currently pressed or not.
		/// </summary>
		public bool Pressed { get; }
	}

	/// <summary>
	/// Event-based sensor which measures the binary push pressure.
	/// </summary>
	public class PushButton : ISubscribableSensor<ButtonState>, IReadableSensor<ButtonState>
	{
		private readonly ICharacteristic _read;

		/// <summary>
		/// Invoked when the button state changes.
		/// </summary>
		public event EventHandler<ButtonState> ValueChanged;

		/// <summary>
		/// Always -1, since this sensor is event-based.
		/// </summary>
		public int SamplingRate { get => -1; set => _ = value; }

		/// <summary>
		/// Construct a new PushButton.
		/// </summary>
		/// <param name="read">Characteristic, which provides read-access to the current button state</param>
		internal PushButton(ICharacteristic read)
		{
			_read = read;
			_read.ValueUpdated += OnValueUpdated;
		}

		/// <summary>
		/// Start sampling.
		/// </summary>
		public async Task StartSamplingAsync()
		{
			await _read.StartUpdatesAsync();
		}

		/// <summary>
		/// Stop sampling.
		/// </summary>
		public async Task StopSamplingAsync()
		{
			await _read.StopUpdatesAsync();
		}

		/// <summary>
		/// Manually retrieve the current button state.
		/// </summary>
		/// <returns>Current button state</returns>
		public async Task<ButtonState> ReadAsync()
		{
			var message = await _read.ReadAsync();
			return ParseMessage(message);
		}

		private ButtonState ParseMessage(byte[] bytes)
		{
			var message = new ESenseMessage(received: bytes);
			var pushed = (message.Data[0] & 1) == 1;
			return new ButtonState(pushed);
		}

		private void OnValueUpdated(object sender, CharacteristicUpdatedEventArgs e)
		{
			ValueChanged?.Invoke(this, ParseMessage(e.Characteristic.Value));
		}
	}
}
