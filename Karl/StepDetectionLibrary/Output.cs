using System;
using System.Collections.Generic;
using System.Text;

namespace StepDetectionLibrary
{
	public struct Output
	{
		private double freq;
		//private int stepcount?

		public Output(double freq)
		{
			this.freq = freq;
		}

		public double Frequency
		{ get{ return this.freq; } }

	}
	public class OutputManager : IObservable<Output>
	{
		private static OutputManager _singletonOutputManager;
	

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
		public IDisposable Subscribe(IObserver<Output> observer)
		{
			throw new NotImplementedException(); //todo
		}
	}
}
