using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Karl.Model
{
	public partial class AudioTrack
	{
		sealed private class BasicAudioTrack : IAudioTrack
		{
			public double Duration => throw new NotImplementedException(); //todo

			public Image Cover => throw new NotImplementedException(); //todo

			public double CurrentPosition => throw new NotImplementedException(); //todo
		}
	}
}
