using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Karl.Model
{
	interface IEarable
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
