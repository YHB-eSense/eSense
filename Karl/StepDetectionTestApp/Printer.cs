using StepDetectionLibrary;
using System;

namespace StepDetectionTestApp
{
	class Printer : IObserver<Output>
	{
		public void OnCompleted()
		{
			throw new NotImplementedException();
		}

		public void OnError(Exception error)
		{
			throw new NotImplementedException();
		}

		public void OnNext(Output value)
		{
			Console.WriteLine(value.Log.CountSteps());
		}
	}
}
