using Plugin.SimpleAudioPlayer;
using System;
using System.IO;

namespace Karl.Model
{

	public class BasicAudioPlayer : IAudioPlayerImpl
	{
		private Stream _stream;
		protected ISimpleAudioPlayer _simpleAudioPlayer;
		private bool _paused;

		public AudioTrack CurrentTrack { get; set; }

		public double CurrentSongPos
		{
			get { return _simpleAudioPlayer.CurrentPosition; }
			set { _simpleAudioPlayer.Seek(value); }
		}

		public bool Paused { get => !_simpleAudioPlayer.IsPlaying; set => _paused = value; }
		public double Volume
		{
			get => _simpleAudioPlayer.Volume;
			set => _simpleAudioPlayer.Volume = value;
		}

		public BasicAudioPlayer()
		{
			GetSimpleAudioPlayer();
		}

		public void PlayTrack(AudioTrack track)
		{
			CurrentTrack = track;
			_stream = GetStream();
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

		protected virtual void GetSimpleAudioPlayer()
		{
			_simpleAudioPlayer = Plugin.SimpleAudioPlayer.CrossSimpleAudioPlayer.Current;
			_simpleAudioPlayer.PlaybackEnded += OnPlaybackEndedEvent;
		}

		protected virtual Stream GetStream()
		{
			return File.OpenRead(CurrentTrack.StorageLocation);
		}

	}
}
