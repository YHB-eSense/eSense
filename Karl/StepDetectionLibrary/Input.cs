using System;

namespace StepDetectionLibrary
{


	//todo how to input data
	public struct AccGyroData
	{
		public short[] xacc;
		public short[] yacc;
		public short[] zacc;
		public short[] xgyro;
		public short[] ygyro;
		public short[] zgyro;
	}
	public class Input : IObservable<AccGyroData>

	{
		public IDisposable Subscribe(IObserver<AccGyroData> observer)
		{
			throw new NotImplementedException();
		}

		public void Update(AccGyroData data)
		{
			throw new NotImplementedException();
		}
	}
}
