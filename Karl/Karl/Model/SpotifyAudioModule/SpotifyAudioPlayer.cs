using SpotifyAPI.Web;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Timers;
using static Karl.Model.eSenseSpotifyWebAPI;

namespace Karl.Model
{
	sealed class SpotifyAudioPlayer : IAudioPlayerImpl
	{
		//Getting the Songposition from Spotify takes to long, so the player
		//tiks to with the timer to imitate the changing SongPosition
		private Timer _timer;

		private double _currentSongPosition;
		private AudioTrack _track;
		private SpotifyWebAPI _webAPI;

		public SpotifyAudioPlayer()
		{
			_timer = new Timer();
			_timer.Interval = 100;
			_timer.Elapsed += new ElapsedEventHandler(Tick);
			_timer.AutoReset = true;
			_track = new SpotifyAudioTrack(0, "", "", 0, "", null);
			_webAPI = eSenseSpotifyWebAPI.WebApiSingleton.api;
			Paused = true;
		}
		public double CurrentSongPos
		{
			get => _currentSongPosition;
			set
			{
				_currentSongPosition = value;
				_webAPI.SeekPlayback((int)_currentSongPosition * 1000);
			}
		}
		public Stack<AudioTrack> PlayedSongs { get; set; }

		public AudioTrack CurrentTrack { get => _track; set => _track = value; }

		public Queue<AudioTrack> Queue { get; set; }
		public bool Paused { get; set; }
		public double Volume { get => 0; set => _ = 0; }

		public async void TogglePause()
		{
			var playback = await _webAPI.GetPlaybackAsync();
			//Checks if users has started the playback(otherwise TogglePause isn't working)
			if (playback == null)
			{
				return;
			}
			if (Paused != playback.IsPlaying)
			{
				Paused = !Paused;
			}

			//Load Cover of playing Song
			var webClient = new WebClient();
			string link;
			if (playback.Item != null) link = playback.Item.Album.Images[0].Url;
			else return;
			byte[] imageBytes = webClient.DownloadData(link);

			//Reload Cover
			_track = new SpotifyAudioTrack(playback.Item.DurationMs / 1000
			, playback.Item.Name, playback.Item.Artists[0].Name,
			(int)_webAPI.GetAudioFeatures(playback.Item.Id).Tempo,
			playback.Item.Id, imageBytes);

			//Pause Track 
			if (playback.IsPlaying)
			{
				_timer.Stop();
				_webAPI.PausePlayback();
			}

			//Play Track
			else
			{
				_timer.Start();
				_webAPI.ResumePlayback("", "", null, "", (int)_currentSongPosition * 1000);
			}
		}

		public async void PlayTrack(AudioTrack track)
		{
			//Checks if users has started the playback(otherwise TogglePause isn't working)
			if (await _webAPI.GetPlaybackAsync() == null && WebApiSingleton.Equals(""))
			{
				return;
			}
			_timer.Start();
			List<string> currentTrackList = new List<string>();
			CurrentTrack = track;
			currentTrackList.Add("spotify:track:" + track.TextId);
			_webAPI.ResumePlayback(WebApiSingleton.DeviceId, "", currentTrackList, "", 0);
			Debug.WriteLine("UPdating2");
		}

		private async void Tick(object sender, EventArgs e)
		{
			_currentSongPosition = (await _webAPI.GetPlaybackAsync()).ProgressMs / 1000;
		}

	}
}
