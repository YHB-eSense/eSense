using System;
using System.Collections.Generic;
using System.Text;

namespace StepDetectionLibrary
{
	public class Output
	{

	}
	public class OutputManager : IObservable<Output>
	{
		public static OutputManager SingletonOutputManager
		{
			get
			{
				if (SingletonOutputManager == null)
				{
					SingletonOutputManager = new OutputManager();
					return SingletonOutputManager;
				}
				else
				{
					return SingletonOutputManager;
				}
			}
			private set => SingletonOutputManager = value;
		}
		public IDisposable Subscribe(IObserver<Output> observer)
		{
			throw new NotImplementedException(); //todo
		}
	}
}
