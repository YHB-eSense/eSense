using System;
using System.Collections.Generic;
using System.Text;

namespace StepDetectionLibrary
{
	/// <summary>
	/// struct for dataoutput
	/// </summary>
	public struct Output
	{
		private double freq;
		private int stepcount;

		/// <summary>
		/// constructor for output
		/// </summary>
		/// <param name="freq">frequency</param>
		/// <param name="stepcount">step count</param>
		public Output(double freq, int stepcount)
		{
			this.freq = freq;
			this.stepcount = stepcount;
		}

		/// <summary>
		/// freqency property
		/// </summary>
		public double Frequency
		{ get{ return this.freq; } }

		/// <summary>
		/// step count property
		/// </summary>
		public int StepCount
		{ get { return this.stepcount; } }

	}
	/// <summary>
	/// gets data from stepdetectionalg and handles the output of data
	/// </summary>
	public class OutputManager : IObservable<Output>, IObserver<Output>
	{
		private static OutputManager _singletonOutputManager;
		private List<IObserver<Output>> observers;

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
			throw new NotImplementedException();
		}

		/// <summary>
		/// method to add observers to outputmanager
		/// </summary>
		/// <param name="observer">object that wants to observe outputmanager</param>
		/// <returns>disposable to unsubscribe</returns>
		public IDisposable Subscribe(IObserver<Output> observer)
		{
			throw new NotImplementedException(); //todo
		}

		/// <summary>
		/// method to update observers with new data
		/// </summary>
		/// <param name="output">new stepfreq and count data</param>
		public void Update(Output output)
		{
			foreach (var observer in observers)
			{
				observer.OnNext(output);
			}
		}
	}
}
