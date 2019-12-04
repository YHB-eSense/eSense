using System.Collections.Generic;
using Xamarin.Forms;

namespace Karl.Model
{
	interface Earable
	{
		string Name { get; set; }

		long Id { get; set; }

		List<ISensor> Sensors { get; }

		bool Connect();

		bool IsConnected();

		bool Disconnect();

		IAudioStream AudioStream();
	}
}
