using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.EventArgs;
using Plugin.BLE.Abstractions.Extensions;
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
	public class PushButton : ISubscribableSensor<ButtonState> //, IReadableSensor<ButtonState>
	{
		private static readonly Guid CHAR_BUTTON = GuidExtension.UuidFromPartial(0xFF09);

		private readonly BLEConnection _conn;

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
		/// <param name="read">BLE connection handle</param>
		public PushButton(BLEConnection conn)
		{
			_conn = conn;
		}

		/// <summary>
		/// Start sampling.
		/// </summary>
		public void StartSampling()
		{
			_conn.Subscribe(CHAR_BUTTON, OnValueUpdated);
		}

		/// <summary>
		/// Stop sampling.
		/// </summary>
		public void StopSampling()
		{
			_conn.Unsubscribe(CHAR_BUTTON, OnValueUpdated);
		}

		private ButtonState ParseMessage(byte[] bytes)
		{
			var message = new ESenseMessage();
			message.Decode(bytes);
			var pushed = (message.Data[0] & 1) == 1;
			return new ButtonState(pushed);
		}

		private void OnValueUpdated(byte[] value)
		{
			ValueChanged.Invoke(this, ParseMessage(value));
		}
	}
}
