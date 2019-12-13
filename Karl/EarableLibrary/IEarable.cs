using System;
using System.Collections.Generic;

namespace EarableLibrary
{
	public interface IEarable
	{
		string Name { get; set; }

		Guid Id { get; }

		IAudioStream AudioStream { get; }

		List<ISensor> Sensors { get; }

		System.Threading.Tasks.Task<bool> ConnectAsync();

		System.Threading.Tasks.Task<bool> DisconnectAsync();

		bool IsConnected();
	}
}
