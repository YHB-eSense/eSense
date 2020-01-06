using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace StepDetectionLibrary
{
	/// <summary>
	/// This class takes the raw gyro and acceleration data form the input, detects steps and then pushes them to the outputmanager
	/// </summary>
	class StepDetectionAlg : IObserver<AccGyroData>, IObservable<Output>
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
			Update(StepDetecAlg(value));
		}

		/// <summary>
		/// method to add observers to stepdetectionalg
		/// </summary>
		/// <param name="observer">object that wants to observe stepdetectionalg</param>
		/// <returns>disposable for unsubscribing</returns>
		public IDisposable Subscribe(IObserver<Output> observer)
		{
			if (!observers.Contains(observer))
				observers.Add(observer);
			return new Unsubscriber(observers, observer);

		}

		/// <summary>
		/// Unsubscriber
		/// </summary>
		private class Unsubscriber : IDisposable
		{
			private List<IObserver<Output>> _observers;
			private IObserver<Output> _observer;

			public Unsubscriber(List<IObserver<Output>> observers, IObserver<Output> observer)
			{
				this._observers = observers;
				this._observer = observer;
			}

			public void Dispose()
			{
				if (_observer != null && _observers.Contains(_observer))
					_observers.Remove(_observer);
			}
		}

		/// <summary>
		/// method to update observers
		/// </summary>
		/// <param name="output">new data thats been calculated by the algorithm</param>
		public void Update(Output output)
		{
			foreach (var observer in observers)
			{
				observer.OnNext(output);
			}
		}

		/// <summary>
		/// method with algorithm to detect steps from acceleration and gyrodata
		/// </summary>
		/// <param name="data">acceleration and gyro data</param>
		private Output StepDetecAlg(AccGyroData data)
		{
			throw new NotImplementedException();
		}
	}

}
