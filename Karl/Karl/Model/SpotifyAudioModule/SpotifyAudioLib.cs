using SpotifyAPI.Web;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Karl.Model
{
	sealed class SpotifyAudioLib : IAudioLibImpl
	{
		private SpotifyWebAPI webAPI;
		private bool _initDone;
		/// <summary>
		/// This is the tag of the Spotify Playlist this Lib is based on.
		/// </summary>
		private String PlaylistTag;

		public ObservableCollection<AudioTrack> AllAudioTracks { get; set; }

		public async void Init()
		{
			lock (this)
			{
				if (_initDone) return;
				webAPI = eSenseSpotifyWebAPI.WebApiSingleton.api;
				_initDone = true;
			}
		}

		public void AddTrack(String storage, String title, double duration)
		{
			throw new NotImplementedException();
		}

		public void AddTrack(string storage)
		{
			throw new NotImplementedException();
		}

		public void AddTrack(string storage, string title)
		{
			throw new NotImplementedException();
		}

		public void AddTrack(string storage, string title, string artist, int bpm)
		{
			throw new NotImplementedException();
		}

		public void DeleteTrack(AudioTrack track)
		{
			throw new NotImplementedException();
		}
	}
	
}
