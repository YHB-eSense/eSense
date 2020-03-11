using EarableLibrary;
using System;
using System.Diagnostics;

namespace EarableLibraryTestApp
{
	class Status
	{
		public static void StatusUpdate(string status, params object[] args)
		{
			string formatted = string.Format(status, args);
			Debug.WriteLine(formatted);
			MainPageVM.Instance.StatusText = formatted;
		}

		internal static void StatusUpdate(TestResult<IEarable> result)
		{
			StatusUpdate(result.ToString());
			if (result.Failed)
			{
				Debug.WriteLine(result.Error);
			}
		}
	}
}
