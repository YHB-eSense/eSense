using SpotifyAPI.Web;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
			Debug.WriteLine("asd Add Track");
		}

		public void AddTrack(string storage)
		{
			Debug.WriteLine("asd Add Track");
		}

		public void AddTrack(string storage, string title)
		{
			Debug.WriteLine("asd Add Track");
		}

		public void AddTrack(string storage, string title, string artist, int bpm)
		{
			Debug.WriteLine("asd Add Track");
		}

		public void DeleteTrack(AudioTrack track)
		{
			Debug.WriteLine("asd Add Track");
		}
	}
	
}
