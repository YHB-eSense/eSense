using EarableLibrary;
using System;
using System.Collections.Generic;

namespace StepDetectionLibrary
{
	public struct AccelerationSample
	{
		public TripleShort Acceleration;
		public DateTime Time;
	}

	/// <summary>
	/// Takes motion-samples as input and sends them to an instance of <see cref="StepDetectionAlg"/>.
	/// </summary>
	public class Input : IObservable<AccelerationSample>
	{
		private readonly StepDetectionAlg _algorithm = new StepDetectionAlg();
		private List<IObserver<AccelerationSample>> _observer;

		/// <summary>
		/// contructor for input
		/// </summary>
		public Input()
		{
			_observer = new List<IObserver<AccelerationSample>>();
			Subscribe(_algorithm);
		}

		public int SamplingRate { get => 25; } // TODO: make this configurable

		/// <summary>
		/// method for subscribing to input
		/// </summary>
		/// <param name="observer">object that wants to observe input</param>
		/// <returns>disposable for unsubscribing</returns>
		public IDisposable Subscribe(IObserver<AccelerationSample> observer)
		{
			{
				if (!_observer.Contains(observer))
					_observer.Add(observer);
				return new Unsubscriber(_observer, observer);
			}
		}

		/// <summary>
		/// Unsubscriber
		/// </summary>
		private class Unsubscriber : IDisposable
		{
			private List<IObserver<AccelerationSample>> _observers;
			private IObserver<AccelerationSample> _observer;

			/// <summary>
			/// constructor for unsubscriber
			/// </summary>
			/// <param name="observers">list of observers</param>
			/// <param name="observer">observer</param>
			public Unsubscriber(List<IObserver<AccelerationSample>> observers, IObserver<AccelerationSample> observer)
			{
				this._observers = observers;
				this._observer = observer;
			}

			/// <summary>
			/// dispose
			/// </summary>
			public void Dispose()
			{
				if (_observer != null && _observers.Contains(_observer))
					_observers.Remove(_observer);
			}
		}

		/// <summary>
		/// method to update _observer
		/// </summary>
		/// <param name="data">new accleration + gyro data</param>
		public void Update(AccelerationSample data)
		{
			foreach (var observer in _observer)
			{
				observer.OnNext(data);
			}
		}



		/// <summary>
		/// method to get data from sensors
		/// </summary>
		/// <param name="sender">sender object</param>
		/// <param name="args">parameter</param>
		public void ValueChanged(object sender, MotionSensorSample args)
		{
			var acc = new AccelerationSample
			{
				Acceleration = args.Acc,
				Time = DateTime.UtcNow
			};
			Update(acc);
		}
	}
}
