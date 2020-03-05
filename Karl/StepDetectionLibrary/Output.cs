using System;
using System.Collections.Generic;
using System.IO;

namespace StepDetectionLibrary
{
	/// <summary>
	/// struct for dataoutput
	/// </summary>
	public class Output
	{
		public ActivityLog Log => OutputManager.SingletonOutputManager.Log;

		public double StepCount() => Log.CountSteps();
		public double Frequency() => Log.AverageStepFrequency(TimeSpan.FromSeconds(10));
	}
	/// <summary>
	/// gets data from stepdetectionalg and handles the output of data
	/// </summary>
	public class OutputManager : IObservable<Output>, IObserver<Output>
	{
		private static OutputManager _singletonOutputManager;
		private List<IObserver<Output>> _observers;


		/// <summary>
		/// singleton pattern
		/// </summary>
		public static OutputManager SingletonOutputManager
		{
			get
			{
				if (_singletonOutputManager == null)
				{
					_singletonOutputManager = new OutputManager();
					return _singletonOutputManager;
				}
				else
				{
					return _singletonOutputManager;
				}
			}
			private set => _singletonOutputManager = value;
		}

		/// <summary>
		/// Singleton -> Don't call.
		/// </summary>
		private OutputManager()
		{
			_observers = new List<IObserver<Output>>();
			var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "ActivityLog.db3");
			Log = new ActivityLog(path);
		}

		public ActivityLog Log { get; }

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
		/// <param name="value">accelertion + gyro data</param>
		public void OnNext(Output value)
		{
			Update(value);
		}

		/// <summary>
		/// method to add _observer to outputmanager
		/// </summary>
		/// <param name="observer">object that wants to observe outputmanager</param>
		/// <returns>disposable to unsubscribe</returns>
		public IDisposable Subscribe(IObserver<Output> observer)
		{

			if (!_observers.Contains(observer))
				_observers.Add(observer);
			return new Unsubscriber(_observers, observer);

		}


		/// <summary>
		/// method to update _observer with new data
		/// </summary>
		/// <param name="output">new stepfreq and count data</param>
		public void Update(Output output)
		{
			foreach (var observer in _observers)
			{
				observer.OnNext(output);
			}
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
	}
}
