using System;
using System.Threading.Tasks;

namespace EarableLibraryTestApp
{
	public abstract class Test<T>
	{
		public string Name => GetType().Name;

		public async Task<TestResult<T>> RunAndCatch(T testObj)
		{
			TestResult<T> result = new TestResult<T>(this);
			try
			{
				await Run(testObj);
			}
			catch(Exception e)
			{
				result.Error = e;
			}
			return result;
		}

		public abstract Task Run(T testObj);
	}
}
