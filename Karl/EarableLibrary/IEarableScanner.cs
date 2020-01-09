using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EarableLibrary
{
	public interface IEarableScanner
	{
		event EventHandler<EarableEventArgs> EarableDiscovered;

		Task StartScanningAsync();

		Task StopScanningAsync();
	}

	public class EarableEventArgs : System.EventArgs
	{

		public IEarable Earable;

		public EarableEventArgs(IEarable earable)
		{
			Earable = earable;
		}
	}
}
