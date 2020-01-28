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
		private AudioTrack _track;

		public SpotifyWebAPI api { get; set; }

		public SpotifyAudioPlayer()
		{
			_timer = new Timer();
			_timer.Interval = 100;
			_timer.Elapsed += new ElapsedEventHandler(Tick);
			_timer.AutoReset = true;
			_track = new SpotifyAudioTrack(0, "", "", 0, "", null);
			Paused = true;
		}
		public double CurrentSongPos { get; set; }
		public Stack<AudioTrack> PlayedSongs { get; set; }

		public AudioTrack CurrentTrack { get => _track; set => _track = value; }

		public Queue<AudioTrack> Queue { get; set; }
		public bool Paused { get; set;  }

		public async void TogglePause()
		{
			if (api.GetPlayback() == null) {
				await Application.Current.MainPage.DisplayAlert("", "Please start Spotify", "OK");
				return;
			}
			if (Paused != api.GetPlayback().IsPlaying)
			{
				Paused = !Paused;
			}
			var webClient = new WebClient();
			string link;
			if (api.GetPlayback().Item != null) link = api.GetPlayback().Item.Album.Images[0].Url;
			else return;
			byte[] imageBytes = webClient.DownloadData(link);
			Debug.WriteLine("asd " + api.GetPlayback().IsPlaying);
			//if(_track.Duration == 0)
			//{
				_track = new SpotifyAudioTrack(api.GetPlayback().Item.DurationMs/1000
				, api.GetPlayback().Item.Name, api.GetPlayback().Item.Artists[0].Name,
				(int)api.GetAudioFeatures(api.GetPlayback().Item.Id).Tempo,
				api.GetPlayback().Item.Id,imageBytes);
			//}
			if (api.GetPlayback().IsPlaying)
			{
				_timer.Stop();
				api.PausePlayback();
			}
			else
			{
				_timer.Start();
				api.ResumePlayback("", "", null, "", 0);
			}
		}

		public async void PlayTrack(AudioTrack track)
		{
			if (api.GetPlayback() == null)
			{
				await Application.Current.MainPage.DisplayAlert("", "Please start Spotify", "OK");
				return;
			}
			_timer.Start();
			List<String> list = new List<string>();
			CurrentTrack = track;
			list.Add("spotify:track:"+track.TextId);
			if(api.ResumePlayback("", "", list, "", 0).HasError())
			Debug.WriteLine(api.ResumePlayback("", "", list, "", 0).Error.Message);
			Debug.WriteLine("bds Play "+track.Title);
		}

		private void Tick(object sender, EventArgs e)
		{
			if (api.GetPlayback() == null) return;
			CurrentSongPos = api.GetPlayback().ProgressMs/1000;
		}

	}
}
