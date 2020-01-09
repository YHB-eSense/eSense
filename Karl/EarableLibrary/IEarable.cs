using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace EarableLibrary
{
	public interface IEarable
	{
		string Name { get; set; }

		Guid Id { get; }

		IAudioStream AudioStream { get; }

		ReadOnlyDictionary<Type, ISensor> Sensors { get; }

		Task<bool> ConnectAsync();

		Task<bool> DisconnectAsync();

		bool IsConnected();
	}
}
