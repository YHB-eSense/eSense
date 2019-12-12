using System;
using System.Collections.Generic;
using System.Text;

namespace Karl.Model
{
	public partial class AudioLib
	{
		public void UseSpotifyAudioLib(String PlaylistID)
		{
			//todo
		}
		private sealed class SpotifyAudioLib : IAudioLib
		{
			public SpotifyAudioLib(String PlaylistID)
			{
				//todo
			}

			public IList<AudioTrack> AudioTracks => throw new NotImplementedException(); //todo

			public IList<AudioTrack> Queue { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

			public IList<AudioTrack> PlayedSongs => throw new NotImplementedException();

			public void AddTrack()
			{
				throw new NotImplementedException(); //todo
			}

			public void NextSong()
			{
				throw new NotImplementedException();
			}

			public void PrevSong()
			{
				throw new NotImplementedException();
			}
		}
	}
}
