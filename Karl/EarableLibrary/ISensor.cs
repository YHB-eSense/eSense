using System;

namespace EarableLibrary
{

	public interface ISensor
	{
		event EventHandler ValueChanged;

		void StartSampling();

		void StopSampling();
	}
}
