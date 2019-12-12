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

		public Output(double freq, int stepcount)
		{
			this.freq = freq;
			this.stepcount = stepcount;
		}

		public double Frequency
		{ get{ return this.freq; } }

		public int StepCount
		{ get { return this.stepcount; } }

	}
	/// <summary>
	/// gets data from stepdetectionalg and handles the output of data
	/// </summary>
	public class OutputManager : IObservable<Output>
	{
		private static OutputManager _singletonOutputManager;
		private List<IObserver<Output>> observers;


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
