using System;
using System.Collections.Generic;
using System.Text;

namespace Karl.Model
{
	public partial class AudioPlayer
	{
		private sealed class SpotifyAudioPlayer : IAudioPlayer
		{
			public void NextTrack()
			{
				throw new NotImplementedException(); //todo
			}

			public void PauseTrack()
			{
				throw new NotImplementedException(); //todo
			}

			public void PlayTrack()
			{
				throw new NotImplementedException(); //todo
			}

			public void PrevTrack()
			{
				throw new NotImplementedException(); //todo
			}
		}
	}
}
