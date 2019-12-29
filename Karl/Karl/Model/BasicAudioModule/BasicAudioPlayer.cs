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

		public BasicAudioPlayer()
		{
			_audioPlayer = Plugin.SimpleAudioPlayer.CrossSimpleAudioPlayer.Current;
		}

		private Stream GetStreamFromFile(string filename)
		{
			var assembly = typeof(App).Assembly;
			var stream = assembly.GetManifestResourceStream("Karl." + filename);
			return stream;
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

		public void PlayTrack(AudioTrack track)
		{
			CurrentTrack = track;
			_stream = GetStreamFromFile(CurrentTrack.StorageLocation);
			_audioPlayer.Load(_stream);
			_audioPlayer.Play();
		}

		public TimeSpan CurrentSongPos { get; set; } //todo

		public Stack<AudioTrack> PlayedSongs => throw new NotImplementedException();

		public AudioTrack CurrentTrack { get; set; }

		public Queue<AudioTrack> Queue => throw new NotImplementedException();
	}
	
}
