using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace StepDetectionLibrary
{
	/// <summary>
	/// This class takes the raw gyro and acceleration data form the input, detects steps and then pushes them to the outputmanager
	/// </summary>
	public class StepDetectionAlg : IObserver<AccGyroData>, IObservable<Output>
	{
		public StepDetectionAlg()
		{
			_observer = new List<IObserver<Output>>();
			Subscribe(OutputManager.SingletonOutputManager);
		}
		private List<IObserver<Output>> _observer;
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
		/// method to add _observer to stepdetectionalg
		/// </summary>
		/// <param name="observer">object that wants to observe stepdetectionalg</param>
		/// <returns>disposable for unsubscribing</returns>
		public IDisposable Subscribe(IObserver<Output> observer)
		{
			if (!_observer.Contains(observer))
				_observer.Add(observer);
			return new Unsubscriber(_observer, observer);

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
		/// method to update _observer
		/// </summary>
		/// <param name="output">new data thats been calculated by the algorithm</param>
		public void Update(Output output)
		{
			foreach (var observer in _observer)
			{
				observer.OnNext(output);
			}
		}

		/// <summary>
		/// method with algorithm to detect steps from acceleration and GyroData
		/// </summary>
		/// <param name="data">acceleration and gyro data</param>
		private Output StepDetecAlg(AccGyroData data)
		{
			int length = AccGyroData.DATALENGTH;
			AccData accdata = data.AccData;
			const double AVGMAG = 10; // value needs to be tested
			const double THRESHHOLD = 10; //value needs to be tested
			int stepcount = 0;
			Boolean threshhold_passed = false;

			double[] netmag = new double[length];
			for (int i = 0; i < length; i++)
			{
				netmag[i] = Math.Sqrt((accdata.Xacc[i]) * (accdata.Xacc[i]) + (accdata.Yacc[i]) * (accdata.Yacc[i]) + (accdata.Zacc[i]) * (accdata.Zacc[i])) - AVGMAG;
			}

			for (int i = 0; i < length; i++)
			{
				if (netmag[i] > THRESHHOLD)
				{
					threshhold_passed = true;
				} else if (threshhold_passed) {
					threshhold_passed = false;
					stepcount++;
				}
			}

			Output output = new Output(stepcount/(length/50), stepcount);

			return output;
		}
	}

}
