using System.Collections.Generic;
using Xamarin.Forms;

namespace Karl.Model
{
	interface Earable
	{
		string Name { get; set; }

		long Id { get; set; }

		IAudioStream AudioStream { get; }

		List<ISensor> Sensors { get; }

		bool Connect();

		bool IsConnected();

		bool Disconnect();
	}
}
