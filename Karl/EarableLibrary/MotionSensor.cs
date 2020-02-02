using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.EventArgs;
using System;
using System.Diagnostics;
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
		/// Parse a TripleShort from an array of bytes.
		/// The first three pairs of bytes are converted to one short each (in the order x, y, z).
		/// </summary>
		/// <param name="array">The array of bytes</param>
		/// <param name="offset">Offset of the first byte</param>
		/// <param name="bigEndian">Whether to use big-endian (MSB first) or little-endian</param>
		/// <returns>The parsed TripleShort</returns>
		public static TripleShort FromByteArray(byte[] array, int offset=0, bool bigEndian=true)
		{
			TripleShort t;
			if (bigEndian)
			{
				t.x = (short)((array[0 + offset] << 8) + array[1 + offset]);
				t.y = (short)((array[2 + offset] << 8) + array[3 + offset]);
				t.z = (short)((array[4 + offset] << 8) + array[5 + offset]);
			}
			else
			{
				t.x = (short)((array[1 + offset] << 8) + array[0 + offset]);
				t.y = (short)((array[3 + offset] << 8) + array[2 + offset]);
				t.z = (short)((array[5 + offset] << 8) + array[4 + offset]);
			}
			return t;
		}

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
		public byte SampleId { get;  }
	}

	/// <summary>
	/// Represents an IMU (Inertial Measurement Unit).
	/// </summary>
	public class MotionSensor : ISubscribableSensor<MotionSensorSample>, IReadableSensor<MotionSensorSample>
	{
		// Command used to enable and disable IMU sampling
		private static readonly byte CMD_IMU_ENABLE = 0x53;
		private static readonly byte ENABLE = 0x01;
		private static readonly byte DISABLE = 0x00;

		private readonly ICharacteristic _data, _enable, _config, _offset;

		/// <summary>
		/// Invoked when a new sample is available.
		/// </summary>
		public event EventHandler<MotionSensorSample> ValueChanged;

		/// <summary>
		/// Targeted rate (in Hz) at which new samples are retrieved.
		/// </summary>
		public int SamplingRate { get; set; }

		/// <summary>
		/// Construct a new MotionSensor.
		/// </summary>
		/// <param name="data">Characteristic giving access to the sensor readings</param>
		/// <param name="enable">Characteristic giving access to the enable-flag</param>
		/// <param name="config">Characteristic giving access to the sensor configuration</param>
		internal MotionSensor(ICharacteristic data, ICharacteristic enable, ICharacteristic config, ICharacteristic offset)
		{
			_data = data;
			_enable = enable;
			_config = config;
			_offset = offset;
			data.ValueUpdated += OnValueUpdated;
			SamplingRate = 50;
		}

		/// <summary>
		/// Start sampling at the configured rate (<see cref="SamplingRate"/>).
		/// </summary>
		public async Task StartSamplingAsync()
		{
			await _data.StartUpdatesAsync();
			var msg = new ESenseMessage(CMD_IMU_ENABLE, ENABLE, (byte) SamplingRate);
			await _enable.WriteAsync(msg);
		}

		/// <summary>
		/// Stop sampling.
		/// </summary>
		public async Task StopSamplingAsync()
		{
			await _data.StopUpdatesAsync();
			var msg = new ESenseMessage(CMD_IMU_ENABLE, DISABLE, 0);
			await _enable.WriteAsync(msg);
		}

		/// <summary>
		/// Manually retrieve the current sensor reading.
		/// </summary>
		/// <returns>Sensor reading</returns>
		public async Task<MotionSensorSample> ReadAsync()
		{
			var message = await _data.ReadAsync();
			return ParseMessage(message);
		}

		private MotionSensorSample ParseMessage(byte[] bytes)
		{
			var message = new ESenseMessage(received: bytes, hasPacketIndex: true);
			var gyro = TripleShort.FromByteArray(message.Data, offset: 0);
			var acc = TripleShort.FromByteArray(message.Data, offset: 6);
			return new MotionSensorSample(gyro, acc, message.PacketIndex.Value);
		}

		private void OnValueUpdated(object sender, CharacteristicUpdatedEventArgs e)
		{
			ValueChanged?.Invoke(this, ParseMessage(e.Characteristic.Value));
		}
	}
}
