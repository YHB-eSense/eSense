using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace StepDetectionLibrary
{
	class StepDetectionAlg :IObserver<AccGyroData>, IObservable<Output>
	{
		public void OnCompleted()
		{
			throw new NotImplementedException();
		}

		public void OnError(Exception error)
		{
			throw new NotImplementedException();
		}

		public void OnNext(AccGyroData value)
		{
			throw new NotImplementedException();
		}

		public IDisposable Subscribe(IObserver<Output> observer)
		{
			throw new NotImplementedException();
		}

		public void Update(Output output)
		{
			throw new NotImplementedException();
		}
		private void Algorithm()
		{

		}

		private void CalcFreq()
		{

		}
	}

}
