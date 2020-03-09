using EarableLibrary;
using System;
using System.Collections.Generic;

namespace StepDetectionLibrary
{
	/// <summary>
	/// This class takes the raw gyro and acceleration data form the input, detects steps and then pushes them to the outputmanager
	/// </summary>
	public class StepDetectionAlg : IObserver<AccelerationSample>, IObservable<Output>
	{
		private List<IObserver<Output>> _observer;
		protected OutputManager OutManager;

		protected virtual void GetOutManager()
		{
			OutManager = OutputManager.SingletonOutputManager;
		}

		/// <summary>
		/// constructor for stepdetectionalg
		/// </summary>
		public StepDetectionAlg()
		{
			_observer = new List<IObserver<Output>>();
			GetOutManager();
			Subscribe(OutManager);
		}

		/// <summary>
		/// method if provider finished sending data
		/// </summary>
		public void OnCompleted()
		{
			throw new Exception();
		}

		/// <summary>
		/// method when provider experienced an error condition
		/// </summary>
		/// <param name="error">exception</param>
		public void OnError(Exception error)
		{
			throw new Exception();
		}

		/// <summary>
		/// method when recieving new data
		/// </summary>
		/// <param name="value">accleration + gyro data</param>
		public void OnNext(AccelerationSample value)
		{
			Update(StepDetectAlg(value.Acceleration, value.Time));
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

		private Step _currentStep;


		/// <summary>
		/// Value which corresponds to the uncalibrated acceleration-intensity when the earables are not in motion.
		/// </summary>
		public double IntensityOffset = 10;

		/// <summary>
		/// Threshold above which the calibrated acceleration-intensity will be counted as a step.
		/// </summary>
		public double IntensityThreshold = 6500;

		/// <summary>
		/// Calculate the current acceleration-intensity from the measured acceleration.
		/// Calibration is applied by substraction of <see cref="IntensityOffset"/> as the last step of calculation.
		/// </summary>
		/// <param name="acc">Measured acceleration</param>
		/// <returns>Acceleration-Intensity</returns>
		public double CalculateIntensity(TripleShort acc)
		{
			return Math.Sqrt(acc.x * acc.x + acc.y * acc.y + acc.z * acc.z) - IntensityOffset;
		}


		/// <summary>
		/// Algorithm to detect steps from acceleration data.
		/// Samples must be passed in chronological order.
		/// </summary>
		/// <param name="acceleration">One acceleration sample</param>
		/// <param name="sampleTaken">Time (in UTC!) at which the sample was recorded</param>
		private Output StepDetectAlg(TripleShort acceleration, DateTime sampleTaken)
		{
			var intensity = CalculateIntensity(acceleration);
			if (intensity > IntensityThreshold)
			{
				if (_currentStep == null)
				{
					_currentStep = new Step
					{
						Taken = sampleTaken,
						Intensity = intensity
					};
				}
			}
			else
			{
				if (_currentStep != null)
				{
					_currentStep.Duration = sampleTaken - _currentStep.Taken;
					OutManager.Log.Add(_currentStep);
					_currentStep = null;
				}
			}
			return new Output();
		}

		/// <summary>
		/// Unsubscriber
		/// </summary>
		private class Unsubscriber : IDisposable
		{
			private List<IObserver<Output>> _observers;
			private IObserver<Output> _observer;

			/// <summary>
			/// constructor for unsubscriber
			/// </summary>
			/// <param name="observers">observers</param>
			/// <param name="observer">observer</param>
			public Unsubscriber(List<IObserver<Output>> observers, IObserver<Output> observer)
			{
				this._observers = observers;
				this._observer = observer;
			}

			/// <summary>
			/// dipsose
			/// </summary>
			public void Dispose()
			{
				if (_observer != null && _observers.Contains(_observer))
					_observers.Remove(_observer);
			}
		}
	}

}
