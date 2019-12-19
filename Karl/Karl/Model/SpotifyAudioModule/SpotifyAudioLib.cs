using System;
using System.Collections.Generic;
using System.Text;

namespace Karl.Model
{
	sealed class SpotifyAudioLib : IAudioLibImpl
	{
		private static SpotifyAudioLib _singletonSpotifyAudioLib;
		public static SpotifyAudioLib SingletonSpotifyAudioLib
		{
			get
			{
				if (_singletonSpotifyAudioLib == null)
				{
					_singletonSpotifyAudioLib = new SpotifyAudioLib();
					return _singletonSpotifyAudioLib;
				}
				else
				{
					return _singletonSpotifyAudioLib;
				}
			}
			private set => _singletonSpotifyAudioLib = value;
		}

		/// <summary>
		/// This is the tag of the Spotify Playlist this Lib is based on.
		/// </summary>
		private String PlaylistTag;

		public IList<AudioTrack> AllAudioTracks => throw new NotImplementedException();

		private SpotifyAudioLib()
		{
			//todo
		}

		public void AddTrack()
		{
			throw new NotImplementedException();
		}
	}
	
}
