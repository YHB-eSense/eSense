namespace StepDetectionLibrary
{


	/// <summary>
	/// class with accerleration and gyro data for all 3 axes
	/// </summary>
	public class AccGyroData
	{
		/// <summary>
		/// acceleration data of 3 axis
		/// </summary>
		public readonly AccData AccData;

		/// <summary>
		/// gyroscope data of 3 axis
		/// </summary>
		public readonly GyroData GyroData;

		/// <summary>
		/// datalength
		/// </summary>
		public readonly int DataLength;

		/// <summary>
		/// samplingrate
		/// </summary>
		public int SamplingRate;

		/// <summary>
		/// length in seconds
		/// </summary>
		public double LengthInSeconds => (double)DataLength / SamplingRate;

		/// <summary>
		/// constructor for accgyrodata
		/// </summary>
		/// <param name="dataLength"> datalength of data</param>
		/// <param name="samplingRate">samplingrate</param>
		public AccGyroData(int dataLength, int samplingRate)
		{
			DataLength = dataLength;
			SamplingRate = samplingRate;
			AccData = new AccData(dataLength);
			GyroData = new GyroData(dataLength);
		}
	}

	/// <summary>
	/// struct with acceleration data for 3 axes
	/// </summary>
	public struct AccData
	{
		public double[] Xacc;
		public double[] Yacc;
		public double[] Zacc;

		public AccData(int a)
		{
			Xacc = new double[a];
			Yacc = new double[a];
			Zacc = new double[a];
		}
		public AccData(double[] xacc, double[] yacc, double[] zacc)
		{
			Xacc = xacc;
			Yacc = yacc;
			Zacc = zacc;
		}
	}

	/// <summary>
	/// struct with gyroscope data for 3 axes
	/// </summary>
	public struct GyroData
	{
		public double[] Xgyro;
		public double[] Ygyro;
		public double[] Zgyro;

		public GyroData(int a)
		{
			Xgyro = new double[a];
			Ygyro = new double[a];
			Zgyro = new double[a];
		}
		public GyroData(double[] xgyro, double[] ygyro, double[] zgyro)
		{
			Xgyro = xgyro;
			Ygyro = ygyro;
			Zgyro = zgyro;
		}
	}
}
