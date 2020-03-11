using System;
using System.Threading.Tasks;

namespace EarableLibraryTestApp
{
	public abstract class Test<T>
	{
		public string Name => GetType().Name;

		public async Task<string> RunAndCatch(T testObj)
		{
			try
			{
				await Run(testObj);
				return string.Format("Test '{0}' completed without errors.", Name);
			}
			catch(Exception e)
			{
				return string.Format("Test failed with an exception: {0}", e.Message);
			}
			
		}

		public abstract Task Run(T testObj);
	}
}
