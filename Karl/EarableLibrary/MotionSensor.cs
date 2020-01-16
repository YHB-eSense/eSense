using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.EventArgs;
using System;
using System.Threading.Tasks;

namespace EarableLibrary
{
	/// <summary>
	/// Group of three shorts (x, y, z).
	/// </summary>
	public struct TripleShort
	{
		public short x;
		public short y;
		public short z;

		public static TripleShort FromByteArray(byte[] b, int offset=0)
		{
			TripleShort t;
			t.x = (short) ((b[0 + offset] << 8) + b[1 + offset]);
			t.y = (short) ((b[2 + offset] << 8) + b[3 + offset]);
			t.z = (short) ((b[4 + offset] << 8) + b[5 + offset]);
			return t;
		}

		public TripleShort(short x, short y, short z)
		{
			this.x = x;
			this.y = y;
			this.z = z;
		}
	}

	public class MotionArgs : EventArgs
	{
		public MotionArgs(TripleShort gyro, TripleShort acc, byte packetId)
		{
			Gyro = gyro;
			Acc = acc;
			PacketId = packetId;
		}
		public TripleShort Gyro { get; }
		public TripleShort Acc { get; }
		public byte PacketId { get;  }
	}

	/// <summary>
	/// An IMU (Inertial Measurement Unit).
	/// </summary>
	public class MotionSensor : ISubscribableSensor<MotionArgs>
	{
		private static readonly byte CMD_IMU_ENABLE = 0x53;
		private static readonly byte ENABLE = 0x01;
		private static readonly byte DISABLE = 0x00;

		public event EventHandler<MotionArgs> ValueChanged;

		public int SamplingRate { get; set; }

		private readonly ICharacteristic _data, _enable, _config;

		public MotionSensor(ICharacteristic data, ICharacteristic enable, ICharacteristic config)
		{
			_data = data;
			_enable = enable;
			_config = config;
			data.ValueUpdated += OnValueChanged;
			SamplingRate = 50;
		}

		public async Task StartSamplingAsync()
		{
			await _data.StartUpdatesAsync();
			var msg = new ESenseMessage(CMD_IMU_ENABLE, ENABLE, (byte) SamplingRate);
			await _enable.WriteAsync(msg);
		}

		public async Task StopSamplingAsync()
		{
			await _data.StopUpdatesAsync();
			var msg = new ESenseMessage(CMD_IMU_ENABLE, DISABLE, 0);
			await _enable.WriteAsync(msg);
		}

		protected virtual void OnValueChanged(object sender, CharacteristicUpdatedEventArgs e)
		{
			var message = new ESenseMessage(received: e.Characteristic.Value, hasPacketIndex: true);
			var gyro = TripleShort.FromByteArray(message.Data, offset: 0);
			var acc = TripleShort.FromByteArray(message.Data, offset: 6);
			var args = new MotionArgs(gyro, acc, message.PacketIndex);
			ValueChanged?.Invoke(this, args);
		}
	}
}
