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
		private List<IObserver<Output>> observers;
		/// <summary>
		/// method if provider finished sending data
		/// </summary>
		public void OnCompleted()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// method when provider experienced an error condition
		/// </summary>
		/// <param name="error">exception</param>
		public void OnError(Exception error)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// method when recieving new data
		/// </summary>
		/// <param name="value">accleration + gyro data</param>
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

		/// <summary>
		/// method with algorithm to detect steps from acceleration and gyrodata
		/// </summary>
		/// <param name="data">acceleration and gyro data</param>
		private void StepDetecAlg(AccGyroData data)
		{
			throw new NotImplementedException();
		}
	}

}
