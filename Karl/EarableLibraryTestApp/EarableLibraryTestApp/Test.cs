using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EarableLibraryTestApp
{
	public abstract class Test<T>
	{
		public IStatus Status { get; }

		protected Test(IStatus status)
		{
			Status = status;
		}

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

		public override string ToString()
		{
			return Regex.Replace(GetType().Name, "`.*$", "");
		}
	}
}
