using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.EventArgs;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace EarableLibrary
{
	public struct TripleShort
	{
		public short x;
		public short y;
		public short z;

		public static TripleShort FromByteArray(byte[] b, int offset=0)
		{
			TripleShort t;
			t.x = (short) (b[0 + offset] << 8 + b[1 + offset]);
			t.y = (short) (b[1 + offset] << 8 + b[3 + offset]);
			t.z = (short) (b[2 + offset] << 8 + b[5 + offset]);
			return t;
		}
	}

	public class MotionSensorChangedEventArgs : EventArgs
	{
		public MotionSensorChangedEventArgs(TripleShort gyro, TripleShort acc)
		{
			Gyro = gyro;
			Acc = acc;
		}
		public TripleShort Gyro { get; }
		public TripleShort Acc { get; }
	}

	public class MotionSensor : ISensor
	{
		private static readonly byte CMD_IMU_ENABLE = 0x53;
		private static readonly byte ENABLED = 0x01;
		private static readonly byte DISABLED = 0x02;

		public event EventHandler ValueChanged;

		public byte SamplingRate { get; set; } = 50;

		private readonly ICharacteristic _data, _enable, _config;

		public MotionSensor(ICharacteristic data, ICharacteristic enable, ICharacteristic config)
		{
			_data = data;
			_enable = enable;
			_config = config;
			data.ValueUpdated += OnValueChanged;
		}

		public void StartSampling()
		{
			new eSenseMessage(CMD_IMU_ENABLE, ENABLED, SamplingRate).WriteTo(_enable);
			_data.StartUpdatesAsync();
		}

		public void StopSampling()
		{
			new eSenseMessage(CMD_IMU_ENABLE, DISABLED, 0).WriteTo(_enable);
			_data.StopUpdatesAsync();
		}

		protected virtual void OnValueChanged(object sender, CharacteristicUpdatedEventArgs e)
		{
			var message = eSenseMessage.ParseMessageWithPacketIndex(e.Characteristic.Value);
			var gyro = TripleShort.FromByteArray(message.Data, offset: 0);
			var acc = TripleShort.FromByteArray(message.Data, offset: 6);
			var args = new MotionSensorChangedEventArgs(gyro, acc);
			ValueChanged?.Invoke(this, args);
		}
	}
}
