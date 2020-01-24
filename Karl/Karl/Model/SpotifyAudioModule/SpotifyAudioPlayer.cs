using System;
using System.Collections.Generic;
using System.Text;
using SpotifyAPI.Web;
using SpotifyAPI.Web.Models;

namespace Karl.Model
{
	sealed class SpotifyAudioPlayer : IAudioPlayerImpl
	{

		private  SpotifyWebAPI _api;

		public SpotifyAudioPlayer(SpotifyWebAPI api) {
			_api = api;
		}

		public double CurrentSongPos { get => _api.GetPlayback().ProgressMs; set => _api.GetPlayback().ProgressMs = (int)value; }

		public Stack<AudioTrack> PlayedSongs => throw new NotImplementedException();

		public AudioTrack CurrentTrack { get =>
				new SpotifyAudioTrack(_api.GetPlayback().Item.DurationMs, _api.GetPlayback().Item.Name,
					_api.GetPlayback().Item.Artists.ToString(),0, _api.GetPlayback().Item.Id.ToString());
			set =>; }

		public double Volume { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

		public Queue<AudioTrack> Queue => throw new NotImplementedException();

		public void TogglePause()
		{
			throw new NotImplementedException(); //todo
		}

		public void PlayTrack(AudioTrack track)
		{
			throw new NotImplementedException(); //todo
		}

		/*

		public double CurrentSongPos { get => _api.GetPlayback().ProgressMs; } //todo

		public Stack<AudioTrack> PlayedSongs => throw new NotImplementedException();

		public AudioTrack CurrentTrack { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

		public double Volume { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

		public Queue<AudioTrack> Queue => throw new NotImplementedException();

		public void TogglePause()
		{
			_api.PausePlayback();
		}

		public void PlayTrack(AudioTrack track)
		{
			//_api.ResumePlayback(""); //todo
		}

		void IAudioPlayerImpl.PlayTrack(AudioTrack track)
		{
			throw new NotImplementedException();
		}

		void IAudioPlayerImpl.TogglePause()
		{
			throw new NotImplementedException();
		}*/
	}
	
}
