using System;
using System.Collections.Generic;
using System.Text;

namespace EarableLibrary
{
	public struct TripleShort
	{
		public short x;
		public short y;
		public short z;
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

	class MotionSensor : ISensor
	{
		public event EventHandler ValueChanged;

		protected virtual void OnValueChanged(MotionSensorChangedEventArgs e)
		{
			ValueChanged?.Invoke(this, e);
		}
	}
}
