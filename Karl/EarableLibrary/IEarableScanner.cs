using System;
using System.Collections.Generic;
using System.Text;

namespace EarableLibrary
{
	public interface IEarableScanner
	{
		event EventHandler<EarableEventArgs> EarableDiscovered;

		void StartScanning();

		void StopScanning();
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
