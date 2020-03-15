using SpotifyAPI.Web;
using SpotifyAPI.Web.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;
using static Karl.Model.AudioLib;

namespace Karl.Model
{
	sealed class SpotifyAudioLib : IAudioLibImpl
	{
		private static bool _testing;
		private bool _initDone;
		private SpotifyWebAPI WebAPI;
		private SimplePlaylist _playlist;

		public event AudioLibEventHandler AudioLibChanged;

		public PrivateProfile Profile { get; set; }

		//List of all AudioTracks in chosen Playlist
		public List<AudioTrack> AllAudioTracks { get; set; }
		//List of the Users Playlists
		public SimplePlaylist[] AllPlaylists { get; set; }
		//Playlist which is shown in the Lib
		public SimplePlaylist SelectedPlaylist
		{
			get => _playlist;
			set
			{
				_playlist = value;
				if (value != null) { ChangePlaylist(value); }
			}
		}

		/// <summary>
		/// Loads all available Playlists and sets selected Playlist to the first Playlist
		/// </summary>
		public async void Init()
		{
			lock (this)
			{
				if (_initDone) return;

				WebAPI = eSenseSpotifyWebAPI.WebApiSingleton.api;
				Profile = eSenseSpotifyWebAPI.WebApiSingleton.UsersProfile;

				if (!_testing && WebAPI.GetUserPlaylists(Profile.Id).Items.Count != 0)
					AllPlaylists = WebAPI.GetUserPlaylists(Profile.Id).Items.ToArray();
				else if (!_testing)
				{
					Debug.WriteLine("[Exception] There are no playlists on this spotify account");
					AudioLibSwitched += SwitchBackToBasicLibExceptionHandler;
					AllPlaylists = null;
				}
				else
					AllPlaylists = null;

				if (AllPlaylists != null)
					SelectedPlaylist = AllPlaylists[0];
				else
					SelectedPlaylist = null;

				_initDone = true;
			}
		}

		/// <summary>
		/// Changes the Selected (used) Playlist to the param playlist
		/// </summary>
		/// <param name="playlist">New Selected Playlist</param>
		private void ChangePlaylist(SimplePlaylist playlist)
		{
			PlaylistTrack[] tracks = WebAPI.GetPlaylistTracks(playlist.Id).Items.ToArray();
			AllAudioTracks = new List<AudioTrack>();
			var webClient = new WebClient();
			foreach (var track in tracks)
			{
				//string link = track.Track.Album.Images[0].Url;
				//byte[] imageBytes = webClient.DownloadData(link);
				AllAudioTracks.Add(new SpotifyAudioTrack(track.Track.DurationMs / 1000, track.Track.Name,
					track.Track.Artists[0].Name, (int)WebAPI.
					GetAudioFeatures(track.Track.Id).Tempo, track.Track.Id, /*imageBytes*/ null));
			}
		}

		public async Task AddTrack(string storage, string title, string artist, int bpm)
		{
			throw new NotImplementedException("Spotify Lib can't add Songs");
		}

		public void DeleteTrack(AudioTrack track)
		{
			throw new NotImplementedException("Spotify Lib can't add Songs");
		}

		private void SwitchBackToBasicLibExceptionHandler()
		{
			SettingsHandler.SingletonSettingsHandler.ChangeAudioModuleToBasic();
			AudioLib.AudioLibSwitched -= SwitchBackToBasicLibExceptionHandler;
		}

		[Conditional("TESTING")]
		internal static void Testing(bool testing)
		{
			_testing = testing;
		}
	}

}
