using System;
using System.Collections.Generic;
using System.IO;
using Plugin.SimpleAudioPlayer;
using System.Text;

namespace Karl.Model
{
	
	sealed class BasicAudioPlayer : IAudioPlayerImpl
	{
		private Stream _stream;
		private ISimpleAudioPlayer _simpleAudioPlayer;

		public AudioTrack CurrentTrack { get; set; }

		public double CurrentSongPos
		{
			get { return _simpleAudioPlayer.CurrentPosition; }
			set { _simpleAudioPlayer.Seek(value); }
		}

		public BasicAudioPlayer()
		{
			_simpleAudioPlayer = Plugin.SimpleAudioPlayer.CrossSimpleAudioPlayer.Current;
			_simpleAudioPlayer.PlaybackEnded += OnPlaybackEndedEvent;
		}

		public void PlayTrack(AudioTrack track)
		{
			CurrentTrack = track;
			_stream = File.OpenRead(CurrentTrack.StorageLocation);
			_simpleAudioPlayer.Load(_stream);
			_simpleAudioPlayer.Play();
		}

		public void TogglePause()
		{
			if (_simpleAudioPlayer.IsPlaying)
			{
				_simpleAudioPlayer.Pause();
			}
			else
			{
				_simpleAudioPlayer.Play();
			}
		}

		private void OnPlaybackEndedEvent(Object source, System.EventArgs e)
		{
			TogglePause();
		}

	}
}
