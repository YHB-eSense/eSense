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

		public override string ToString()
		{
			return Pressed ? "Pressed" : "Released";
		}

		public override bool Equals(object obj)
		{
			if (obj == null) return false;
			if (GetType() == obj.GetType() && obj is ButtonState other)
			{
				return Pressed == other.Pressed;
			}
			return false;
		}
	}

	/// <summary>
	/// Event-based sensor which measures the binary push pressure.
	/// </summary>
	public class PushButton : ISubscribableSensor<ButtonState>, IReadableSensor<ButtonState>
	{
		internal static readonly Guid CHAR_BUTTON = GuidExtension.UuidFromPartial(0xFF09);

		private readonly IDeviceConnection _conn;

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
		public PushButton(IDeviceConnection conn)
		{
			_conn = conn;
		}

		/// <summary>
		/// Start sampling.
		/// </summary>
		public async Task StartSamplingAsync()
		{
			await _conn.SubscribeAsync(CHAR_BUTTON, OnValueUpdated);
		}

		/// <summary>
		/// Stop sampling.
		/// </summary>
		public async Task StopSamplingAsync()
		{
			await _conn.UnsubscribeAsync(CHAR_BUTTON, OnValueUpdated);
		}

		/// <summary>
		/// Manually retrieve the current button state.
		/// </summary>
		/// <returns>Current button state</returns>
		public async Task<ButtonState> ReadAsync()
		{
			return ParseMessage(await _conn.ReadAsync(CHAR_BUTTON));
		}

		private ButtonState ParseMessage(byte[] bytes)
		{
			try
			{
				var message = new ESenseMessage();
				message.Decode(bytes);
				var pushed = (message.Data[0] & 1) == 1;
				return new ButtonState(pushed);
			}
			catch (Exception)
			{
				return null;
			}
		}

		private void OnValueUpdated(byte[] value)
		{
			ValueChanged.Invoke(this, ParseMessage(value));
		}
	}
}
