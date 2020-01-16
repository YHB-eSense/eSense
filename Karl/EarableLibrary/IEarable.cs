using System;
using System.Threading.Tasks;

namespace EarableLibrary
{
	public interface IEarable
	{
		string Name { get; set; }

		Guid Id { get; }

		T GetSensor<T>() where T : ISensor;

		Task<bool> ConnectAsync();

		Task<bool> DisconnectAsync();

		bool IsConnected();
	}
}
