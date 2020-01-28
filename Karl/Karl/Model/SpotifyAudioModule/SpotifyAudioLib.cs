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
		public SpotifyWebAPI WebAPI { get; set; }
		public PrivateProfile Profile { get; set; }
		private bool _initDone;
		/// <summary>
		/// This is the tag of the Spotify Playlist this Lib is based on.
		/// </summary>
		private String PlaylistTag;

		public ObservableCollection<AudioTrack> AllAudioTracks { get; set; }
		public ObservableCollection<SimplePlaylist> AllPlaylists { get; set ; }

		public async void Init()
		{
			lock (this)
			{
				if (_initDone) return;
				AllAudioTracks = new ObservableCollection<AudioTrack>();
				WebAPI = eSenseSpotifyWebAPI.WebApiSingleton.api;
				AllPlaylists = WebAPI.GetUserPlaylists(Profile.Id).Items
				var playlist = AllPlaylists[0];
				PlaylistTrack[] tracks = WebAPI.GetPlaylistTracks(playlist.Id, "", 100, 0, "").Items.ToArray();
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
				_initDone = true;
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
