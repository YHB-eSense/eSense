using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Karl.Model
{
	sealed class SpotifyAudioLib : IAudioLibImpl
	{

		/// <summary>
		/// This is the tag of the Spotify Playlist this Lib is based on.
		/// </summary>
		private String PlaylistTag;

		public ObservableCollection<AudioTrack> AllAudioTracks { get; set; }

		private SpotifyAudioLib()
		{
			//todo
		}

		public void AddTrack(String indentifier, String name, String artist, int bpm)
		{
			throw new NotImplementedException();
		}
	}
	
}
