using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Text;
using System.Timers;
using SpotifyAPI.Web;
using SpotifyAPI.Web.Models;
using Xamarin.Forms;

namespace Karl.Model
{
	sealed class SpotifyAudioPlayer : IAudioPlayerImpl
	{
		private Timer _timer;
		private double _currentSongPosition;
		private AudioTrack _track;
		private SpotifyWebAPI _webAPI { get; set; }

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
				_webAPI.SeekPlayback((int)_currentSongPosition*1000);
			}
		}
		public Stack<AudioTrack> PlayedSongs { get; set; }

		public AudioTrack CurrentTrack { get => _track; set => _track = value; }

		public Queue<AudioTrack> Queue { get; set; }
		public bool Paused { get; set; }
		public double Volume { get => 0; set => _ = 0; }

		public void TogglePause()
		{
			if (_webAPI.GetPlayback() == null)
			{
				return;
			}
			if (Paused != _webAPI.GetPlayback().IsPlaying)
			{
				Paused = !Paused;
			}
			var webClient = new WebClient();
			string link;
			if (_webAPI.GetPlayback().Item != null) link = _webAPI.GetPlayback().Item.Album.Images[0].Url;
			else return;
			byte[] imageBytes = webClient.DownloadData(link);
			if (_track.Duration == 0)
			{
				_track = new SpotifyAudioTrack(_webAPI.GetPlayback().Item.DurationMs / 1000
				, _webAPI.GetPlayback().Item.Name, _webAPI.GetPlayback().Item.Artists[0].Name,
				(int)_webAPI.GetAudioFeatures(_webAPI.GetPlayback().Item.Id).Tempo,
				_webAPI.GetPlayback().Item.Id, imageBytes);
			}
			if (_webAPI.GetPlayback().IsPlaying)
			{
				_timer.Stop();
				_webAPI.PausePlayback();
			}
			else
			{
				_timer.Start();
				_webAPI.ResumePlayback("", "", null, "",(int)_currentSongPosition*1000);
			}
		}

		public void PlayTrack(AudioTrack track)
		{
			if (_webAPI.GetPlayback() == null)
			{
				return;
			}
			_timer.Start();
			List<String> currentTrackList = new List<string>();
			CurrentTrack = track;
			currentTrackList.Add("spotify:track:" + track.TextId);
			_webAPI.ResumePlayback("", "", currentTrackList, "", 0);
		}

		private void Tick(object sender, EventArgs e)
		{
			_currentSongPosition = _webAPI.GetPlayback().ProgressMs / 1000;
		}

	}
}
