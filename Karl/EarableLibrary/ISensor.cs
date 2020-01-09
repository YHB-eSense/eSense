using System;
using System.Threading.Tasks;

namespace EarableLibrary
{
	public interface ISensor
	{
	}

	public interface ISubscribableSensor<T> : ISensor
	{
		event EventHandler<T> ValueChanged;

		Task StartSamplingAsync();

		Task StopSamplingAsync();

	}
}
