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

		public SpotifyWebAPI WebAPI { get; set; }

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
		public double Volume { get => 0; set => _ = 0; }

		public void TogglePause()
		{
			if (WebAPI.GetPlayback() == null) {
				return;
			}
			if (Paused != WebAPI.GetPlayback().IsPlaying)
			{
				Paused = !Paused;
			}
			var webClient = new WebClient();
			string link;
			if (WebAPI.GetPlayback().Item != null) link = WebAPI.GetPlayback().Item.Album.Images[0].Url;
			else return;
			byte[] imageBytes = webClient.DownloadData(link);
			Debug.WriteLine("asd " + WebAPI.GetPlayback().IsPlaying);
			//if(_track.Duration == 0)
			//{
				_track = new SpotifyAudioTrack(WebAPI.GetPlayback().Item.DurationMs/1000
				, WebAPI.GetPlayback().Item.Name, WebAPI.GetPlayback().Item.Artists[0].Name,
				(int)WebAPI.GetAudioFeatures(WebAPI.GetPlayback().Item.Id).Tempo,
				WebAPI.GetPlayback().Item.Id,imageBytes);
			//}
			if (WebAPI.GetPlayback().IsPlaying)
			{
				_timer.Stop();
				WebAPI.PausePlayback();
			}
			else
			{
				_timer.Start();
				WebAPI.ResumePlayback("", "", null, "", 0);
			}
		}

		public void PlayTrack(AudioTrack track)
		{
			if (WebAPI.GetPlayback() == null)
			{
				return;
			}
			_timer.Start();
			List<String> list = new List<string>();
			CurrentTrack = track;
			list.Add("spotify:track:"+track.TextId);
			if(WebAPI.ResumePlayback("", "", list, "", 0).HasError())
			Debug.WriteLine(WebAPI.ResumePlayback("", "", list, "", 0).Error.Message);
			Debug.WriteLine("bds Play "+track.Title);
		}

		private void Tick(object sender, EventArgs e)
		{
			if (WebAPI.GetPlayback() == null) return;
			CurrentSongPos = WebAPI.GetPlayback().ProgressMs/1000;
		}

	}
}
