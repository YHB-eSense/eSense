using StepDetectionLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
			Console.WriteLine(value.StepCount);
		}
	}
}
