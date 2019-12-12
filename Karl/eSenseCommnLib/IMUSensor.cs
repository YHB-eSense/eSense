using System;
using System.Collections.Generic;
using System.Text;

namespace eSenseCommnLib
{
	public class IMUChangedEventArgs : EventArgs
	{
		public IMUChangedEventArgs(short[] gyro, short[] acc)
		{
			Gyro = gyro;
			Acc = acc;
		}
		public short[] Gyro { get; }
		public short[] Acc { get; }
	}

	class IMUSensor : ISensor
	{
		public event EventHandler ValueChanged;

		protected virtual void OnValueChanged(IMUChangedEventArgs e)
		{
			ValueChanged?.Invoke(this, e);
		}
	}
}
