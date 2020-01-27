using System;
using System.Collections.Generic;
using System.Text;
using SpotifyAPI.Web;
using SpotifyAPI.Web.Models;

namespace Karl.Model
{
	sealed class SpotifyAudioPlayer : IAudioPlayerImpl
	{

		public SpotifyWebAPI api;
		private Device _activeDevice;

		public SpotifyAudioPlayer() {
			eSenseSpotifyWebAPI e = eSenseSpotifyWebAPI.WebApiSingleton;
		}

		public double CurrentSongPos { get => api.GetPlayback().ProgressMs; set => api.SeekPlayback((int)value,_activeDevice.Id); }

		public Stack<AudioTrack> PlayedSongs { get; set; }

		public AudioTrack CurrentTrack { get =>
				new SpotifyAudioTrack(api.GetPlayback().Item.DurationMs, api.GetPlayback().Item.Name,
					api.GetPlayback().Item.Artists.ToString(), 0, api.GetPlayback().Item.Id.ToString());
			set => api.ResumePlayback("", "", null, "", 0); }

		public double Volume { get => _activeDevice.VolumePercent; set => api.SetVolume((int)value); }

		public Queue<AudioTrack> Queue {get;set;}

		public void TogglePause()
		{
			api.PausePlayback(); 
		}

		public void PlayTrack(AudioTrack track)
		{
			api.ResumePlayback("","",null,"",0);
		}

		
	}
	
}
