using System;
using System.Collections.Generic;

namespace eSenseCommnLib
{
	public interface IEarable
	{
		string Name { get; set; }

		Guid Id { get; }

		IAudioStream AudioStream { get; }

		List<ISensor> Sensors { get; }

		bool Connect();

		bool IsConnected();

		bool Disconnect();
	}
}
