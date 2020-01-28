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

		public double Volume
		{
			get { return _simpleAudioPlayer.Volume; }
			set { _simpleAudioPlayer.Volume = value; }
		}

		public double CurrentSongPos
		{
			get { return _simpleAudioPlayer.CurrentPosition; }
			set { _simpleAudioPlayer.Seek(value); }
		}

		public bool Paused { get; set; }

		public BasicAudioPlayer()
		{
			_simpleAudioPlayer = Plugin.SimpleAudioPlayer.CrossSimpleAudioPlayer.Current;
			_simpleAudioPlayer.PlaybackEnded += OnPlaybackEndedEvent;
			Paused = true;
		}

		public void PlayTrack(AudioTrack track)
		{
			CurrentTrack = track;
			_stream = File.OpenRead(track.StorageLocation); //GetStreamFromFile(CurrentTrack.StorageLocation);
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




		/* //For loading an embedded mp3
		private Stream GetStreamFromFile(string filename)
		{
			var assembly = typeof(App).Assembly;
			var stream = assembly.GetManifestResourceStream("Karl." + filename);
			return stream;
		}
		*/
	}
	
}
