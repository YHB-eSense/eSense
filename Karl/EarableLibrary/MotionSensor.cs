using System;
using System.Collections.Generic;
using System.Text;

namespace eSenseCommnLib
{
	public class MotionSensorChangedEventArgs : EventArgs
	{
		public MotionSensorChangedEventArgs(short[] gyro, short[] acc)
		{
			Gyro = gyro;
			Acc = acc;
		}
		public short[] Gyro { get; }
		public short[] Acc { get; }
	}

	class MotionSensor : ISensor
	{
		public event EventHandler ValueChanged;

		protected virtual void OnValueChanged(MotionSensorChangedEventArgs e)
		{
			ValueChanged?.Invoke(this, e);
		}
	}
}
