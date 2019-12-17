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

		public IList<AudioTrack> PlayedSongs => throw new NotImplementedException();

		public AudioTrack CurrentTrack { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

		public IList<AudioTrack> AllAudioTracks => throw new NotImplementedException();

		private SpotifyAudioLib()
		{
			//todo
		}

		public void AddTrack()
		{
			throw new NotImplementedException();
		}

		public void NextTrack()
		{
			throw new NotImplementedException();
		}

		public void PrevSong()
		{
			throw new NotImplementedException();
		}
	}
	
}
