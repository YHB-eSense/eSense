using System;

namespace EarableLibraryTestApp
{
	public class TestResult<T>
	{
		public Test<T> Test { get; }

		public Exception Error { get; set; }

		public bool Failed => Error != null;

		public TestResult(Test<T> test)
		{
			Test = test;
		}

		public override string ToString()
		{
			var status = Failed ? string.Format("Failed ({0})", Error.Message) : "Passed";
			return string.Format("Result of Test {0}: {1}", Test, status);
		}
	}
}
