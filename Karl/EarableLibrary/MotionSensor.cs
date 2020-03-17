using Plugin.BLE.Abstractions.Extensions;
using System;
using System.Threading.Tasks;

namespace EarableLibrary
{
	/// <summary>
	/// Group of three shorts (x, y, z).
	/// </summary>
	public struct TripleShort
	{
		/// <summary>
		/// The three shorts making up the triple.
		/// </summary>
		public short x, y, z;

		/// <summary>
		/// Construct a new TripleShort from three shorts.
		/// </summary>
		/// <param name="x">First short</param>
		/// <param name="y">Second short</param>
		/// <param name="z">Third short</param>
		public TripleShort(short x, short y, short z)
		{
			this.x = x;
			this.y = y;
			this.z = z;
		}

		public override string ToString()
		{
			return string.Format("{0},{1},{2}", x, y, z);
		}

		public override bool Equals(object obj)
		{
			if (obj == null) return false;
			if (GetType() == obj.GetType())
			{
				TripleShort other = (TripleShort)obj;
				return x == other.x && y == other.y && z == other.z;
			}
			return false;
		}
	}

	/// <summary>
	/// One sample of the motion sensor readings.
	/// </summary>
	public class MotionSensorSample : EventArgs
	{
		/// <summary>
		/// Construct a new MotionSensorSample.
		/// </summary>
		/// <param name="gyro">Sensor reading of the gyroscope</param>
		/// <param name="acc">Sensor reading of the acceleromter</param>
		/// <param name="id">Consecutive sample id</param>
		public MotionSensorSample(TripleShort gyro, TripleShort acc, byte id)
		{
			Gyro = gyro;
			Acc = acc;
			SampleId = id;
		}

		/// <summary>
		/// Struct which holds sensor readings for all three axes of the gyroscope.
		/// </summary>
		public TripleShort Gyro { get; }

		/// <summary>
		/// Structs which holds sensor readings for all thee axes of the accelerometer.
		/// </summary>
		public TripleShort Acc { get; }

		/// <summary>
		/// Consecutive sample id which increases which each new sample.
		/// When the maximum capacity (255) is exceeded, an overflow happens.
		/// </summary>
		public byte SampleId { get; }

		public override string ToString()
		{
			return string.Format("MotionSensorSample<Id={0},Acc={1},Gyro={2}>", SampleId, Acc, Gyro);
		}

		public override bool Equals(object obj)
		{
			if (obj == null) return false;
			if (GetType() == obj.GetType() && obj is MotionSensorSample other)
			{
				if (SampleId == other.SampleId &&
					Acc.Equals(other.Acc) &&
					Gyro.Equals(other.Gyro))
				{
					return true;
				}
			}
			return false;
		}
	}

	/// <summary>
	/// Represents an IMU (Inertial Measurement Unit).
	/// </summary>
	public class MotionSensor : ISubscribableSensor<MotionSensorSample>
	{
		// Command used to enable and disable IMU sampling
		public static readonly byte CMD_IMU_ENABLE = 0x53;
		public static readonly byte ENABLE = 0x01;
		public static readonly byte DISABLE = 0x00;
		public static readonly Guid CHAR_IMU_ENABLE = GuidExtension.UuidFromPartial(0xFF07);
		public static readonly Guid CHAR_IMU_DATA = GuidExtension.UuidFromPartial(0xFF08);
		public static readonly Guid CHAR_IMU_OFFSET = GuidExtension.UuidFromPartial(0xFF0D);
		public static readonly Guid CHAR_IMU_CONFIG = GuidExtension.UuidFromPartial(0xFF0E);

		private readonly IDeviceConnection _connection;

		/// <summary>
		/// Invoked when a new sample is available.
		/// </summary>
		public event EventHandler<MotionSensorSample> ValueChanged;

		/// <summary>
		/// Targeted rate (in Hz) at which new samples are retrieved.
		/// </summary>
		public int SamplingRate { get; set; }

		private bool _imuEnabled;

		private async Task EnableImu()
		{
			var msg = new ESenseMessage(CMD_IMU_ENABLE, ENABLE, (byte)SamplingRate);
			await _connection.WriteAsync(CHAR_IMU_ENABLE, msg);
			_imuEnabled = true;
		}

		private async Task DisableImu()
		{
			var msg = new ESenseMessage(CMD_IMU_ENABLE, DISABLE, 0);
			await _connection.WriteAsync(CHAR_IMU_ENABLE, msg);
			_imuEnabled = false;
		}

		/// <summary>
		/// Construct a new MotionSensor.
		/// </summary>
		/// <param name="connection">BLE connection handle</param>
		public MotionSensor(IDeviceConnection connection)
		{
			_connection = connection;
			_imuEnabled = false;
			SamplingRate = 50;
		}

		/// <summary>
		/// Start sampling at the configured rate (<see cref="SamplingRate"/>).
		/// </summary>
		public async Task StartSamplingAsync()
		{
			await _connection.SubscribeAsync(CHAR_IMU_DATA, ValueUpdated);
			await EnableImu();
		}

		/// <summary>
		/// Stop sampling.
		/// </summary>
		public async Task StopSamplingAsync()
		{
			await DisableImu();
			await _connection.UnsubscribeAsync(CHAR_IMU_DATA, ValueUpdated);
		}

		private MotionSensorSample ParseMessage(byte[] bytes)
		{
			var message = new IndexedESenseMessage();
			message.Decode(bytes);
			var readings = message.DataAsShortArray();
			var gyro = new TripleShort(readings[0], readings[1], readings[2]);
			var acc = new TripleShort(readings[3], readings[4], readings[5]);
			return new MotionSensorSample(gyro, acc, message.PacketIndex);
		}

		private void ValueUpdated(byte[] value)
		{
			ValueChanged.Invoke(this, ParseMessage(value));
		}
	}
}
