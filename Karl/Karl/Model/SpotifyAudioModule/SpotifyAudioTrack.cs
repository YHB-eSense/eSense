using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Karl.Model
{
	sealed class SpotifyAudioTrack : IAudioTrackImpl
	{
		public double Duration => throw new NotImplementedException();//todo
		public string Title => throw new NotImplementedException();

		public string Artist => throw new NotImplementedException();
		public Image Cover => throw new NotImplementedException();//todo
		public int BPM { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
	}
}
