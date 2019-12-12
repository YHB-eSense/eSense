using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace StepDetectionLibrary
{
	/// <summary>
	/// This class takes the raw gyro and acceleration data form the input, detects steps and then pushes them to the outputmanager
	/// </summary>
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

		/// <summary>
		/// method to add observers to stepdetectionalg
		/// </summary>
		/// <param name="observer">object that wants to observe stepdetectionalg</param>
		/// <returns>disposable for unsubscribing</returns>
		public IDisposable Subscribe(IObserver<Output> observer)
		{
			throw new NotImplementedException();
		}
		/// <summary>
		/// method to update observers
		/// </summary>
		/// <param name="output">new data thats been calculated by the algorithm</param>
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
