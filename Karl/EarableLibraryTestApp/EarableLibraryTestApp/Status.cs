using System.Diagnostics;

namespace EarableLibraryTestApp
{
	public interface IStatus
	{
		void StatusUpdate(string status, params object[] args);
	}

	public class DebugStatus : IStatus
	{
		public void StatusUpdate(string status, params object[] args)
		{
			Debug.WriteLine(status, args);
		}
	}
}
