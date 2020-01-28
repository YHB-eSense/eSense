using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Text;
using System.Timers;
using SpotifyAPI.Web;
using SpotifyAPI.Web.Models;

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
		public double Volume { get => 0; set => _ = value; }

		public void TogglePause()
		{
			if (Paused != api.GetPlayback().IsPlaying)
			{
				Paused = !Paused;
			}
			var webClient = new WebClient();
			string link = api.GetPlayback().Item.Album.Images[0].Url;
			byte[] imageBytes = webClient.DownloadData(link);
			Debug.WriteLine("asd " + api.GetPlayback().IsPlaying);
			//if(_track.Duration == 0)
			//{
				_track = new SpotifyAudioTrack(api.GetPlayback().Item.DurationMs/1000
				, api.GetPlayback().Item.Name, api.GetPlayback().Item.Artists[0].Name, 0,
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

		public void PlayTrack(AudioTrack track)
		{
			api.ResumePlayback("", "", null, "", 0);
			_timer.Start();
			//CurrentTrack = new SpotifyAudioTrack(api.GetPlayback().Item.DurationMs
			//, api.GetPlayback().Item.Name, api.GetPlayback().Item.Artists[0].Name, 0, "");
		}

		private void Tick(object sender, EventArgs e)
		{
			CurrentSongPos = api.GetPlayback().ProgressMs/1000;
		}

	}
}
