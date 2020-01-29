using System;
using System.Collections.Generic;

namespace StepDetectionLibrary
{
	/// <summary>
	/// struct for dataoutput
	/// </summary>
	public struct Output
	{
		private double _freq;
		private int _stepcount;

		/// <summary>
		/// constructor for output
		/// </summary>
		/// <param name="freq">frequency</param>
		/// <param name="stepcount">step count</param>
		public Output(double freq, int stepcount)
		{
			this._freq = freq;
			this._stepcount = stepcount;
		}

		/// <summary>
		/// freqency property in Hz
		/// </summary>
		public double Frequency
		{ get { return this._freq; } }

		/// <summary>
		/// step count property
		/// </summary>
		public int StepCount
		{ get { return this._stepcount; } }

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
		}

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
