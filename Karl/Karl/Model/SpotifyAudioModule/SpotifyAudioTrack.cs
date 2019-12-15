using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Karl.Model
{
	public partial class AudioTrack
	{
		private sealed class SpotifyAudioTrack : IAudioTrack
		{
			public double Duration => throw new NotImplementedException();//todo
			
			public Image Cover => throw new NotImplementedException();//todo

			public double CurrentPosition => throw new NotImplementedException();//todo

			public string Title => throw new NotImplementedException();

			public string Artist => throw new NotImplementedException();

			public int BPM => throw new NotImplementedException();
		}
	}
}
