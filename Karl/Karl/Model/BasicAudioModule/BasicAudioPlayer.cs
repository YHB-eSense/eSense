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
		private ISimpleAudioPlayer _audioPlayer;

		public AudioTrack CurrentTrack { get; set; }

		public double Volume
		{
			get { return _audioPlayer.Volume; }
			set { _audioPlayer.Volume = value; }
		}

		public double CurrentSongPos
		{
			get { return _audioPlayer.CurrentPosition; }
			set { _audioPlayer.Seek(value); }
		}

		public Stack<AudioTrack> PlayedSongs => throw new NotImplementedException();

		public Queue<AudioTrack> Queue => throw new NotImplementedException();

		public BasicAudioPlayer()
		{
			_audioPlayer = Plugin.SimpleAudioPlayer.CrossSimpleAudioPlayer.Current;
		}

		public void PlayTrack(AudioTrack track)
		{
			CurrentTrack = track;
			_stream = File.OpenRead(CurrentTrack.StorageLocation); //GetStreamFromFile(CurrentTrack.StorageLocation);
			_audioPlayer.Load(_stream);
			_audioPlayer.Play();
		}

		public void TogglePause()
		{
			if (_audioPlayer.IsPlaying)
			{
				_audioPlayer.Pause();
			}
			else
			{
				_audioPlayer.Play();
			}
		}

		private Stream GetStreamFromFile(string filename)
		{
			var assembly = typeof(App).Assembly;
			var stream = assembly.GetManifestResourceStream("Karl." + filename);
			return stream;
		}

	}
	
}
