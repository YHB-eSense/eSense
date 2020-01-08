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
		private static readonly byte ON = 0x01;
		private static readonly byte OFF = 0x02;

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
			WriteBytes(_enable, new eSenseMessage(CMD_IMU_ENABLE, ON, SamplingRate));
			_data.StartUpdatesAsync();
		}

		private void WriteBytes(ICharacteristic c, byte[] bytes)
		{
			Device.BeginInvokeOnMainThread(() =>
			{
				c.WriteAsync(bytes);
			});
		}

		public void StopSampling()
		{
			WriteBytes(_enable, new eSenseMessage(CMD_IMU_ENABLE, OFF, 0));
			_data.StopUpdatesAsync();
		}

		protected virtual void OnValueChanged(object sender, CharacteristicUpdatedEventArgs e)
		{
			eSenseMessage message = (eSenseMessage) e.Characteristic.Value;
			TripleShort gyro = TripleShort.FromByteArray(message.Data, 0);
			TripleShort acc = TripleShort.FromByteArray(message.Data, 6);
			MotionSensorChangedEventArgs args = new MotionSensorChangedEventArgs(gyro, acc);
			ValueChanged?.Invoke(this, args);
		}
	}
}
