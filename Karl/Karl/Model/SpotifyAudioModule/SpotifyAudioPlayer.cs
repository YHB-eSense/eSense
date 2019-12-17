using System;
using System.Collections.Generic;
using System.Text;

namespace Karl.Model
{
	sealed class SpotifyAudioPlayer : IAudioPlayerImpl
	{
		public double CurrentSongPos { get; set; } //todo

		public void TogglePause()
		{
			throw new NotImplementedException(); //todo
		}

		public void PlayTrack()
		{
			throw new NotImplementedException(); //todo
		}
	}
	
}
