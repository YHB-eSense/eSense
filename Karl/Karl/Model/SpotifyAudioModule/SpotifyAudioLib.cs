using SpotifyAPI.Web;
using SpotifyAPI.Web.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net;
using System.Text;

namespace Karl.Model
{
	sealed class SpotifyAudioLib : IAudioLibImpl
	{
		private SpotifyWebAPI WebAPI;
		private SimplePlaylist _playlist;
		public PrivateProfile Profile { get; set; }
		private bool _initDone;
		/// <summary>
		/// This is the tag of the Spotify Playlist this Lib is based on.
		/// </summary>
		private String PlaylistTag;

		public List <AudioTrack> AllAudioTracks { get; set; }

		public SimplePlaylist[] AllPlaylists { get; set; }
		public SimplePlaylist SelectedPlaylist
		{
			get => _playlist;
			set {
				_playlist = value;
				if(value != null) { ChangePlaylist(value); }
			}
		}


		public async void Init()
		{
			lock (this)
			{
				if (_initDone) return;
				WebAPI = eSenseSpotifyWebAPI.WebApiSingleton.api;
				Profile = eSenseSpotifyWebAPI.WebApiSingleton.UsersProfile;
				if(WebAPI.GetUserPlaylists(Profile.Id).Items.Count != 0) { AllPlaylists = WebAPI.GetUserPlaylists(Profile.Id).Items.ToArray(); }
				else { AllPlaylists = null; }
				if (AllPlaylists != null) { SelectedPlaylist = AllPlaylists[0]; }
				else { SelectedPlaylist = null; }
				_initDone = true;
			}
		}

		private void ChangePlaylist(SimplePlaylist playlist)
		{
			PlaylistTrack[] tracks = WebAPI.GetPlaylistTracks(playlist.Id, "", 100, 0, "").Items.ToArray();
			AllAudioTracks = new List<AudioTrack>();
			foreach (var track in tracks)
			{
				var webClient = new WebClient();
				string link = track.Track.Album.Images[0].Url;
				byte[] imageBytes = webClient.DownloadData(link);
				AllAudioTracks.Add(new SpotifyAudioTrack(track.Track.DurationMs / 1000, track.Track.Name,
					track.Track.Artists[0].Name, (int)WebAPI.
					GetAudioFeatures(track.Track.Id).Tempo, track.Track.Id, imageBytes));
				Debug.WriteLine(track.Track.Name);
			}
		}

		public SpotifyAudioLib()
		{

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
