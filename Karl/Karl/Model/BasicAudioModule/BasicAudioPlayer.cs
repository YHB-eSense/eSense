using System;
using System.Collections.Generic;
using System.Text;

namespace Karl.Model
{
	
	sealed class BasicAudioPlayer : IAudioPlayerImpl
	{
		private static BasicAudioPlayer _singletonBasicAudioPlayer;
		public static BasicAudioPlayer SingletonBasicAudioPlayer
		{
			get
			{
				if (_singletonBasicAudioPlayer == null)
				{
					_singletonBasicAudioPlayer = new BasicAudioPlayer();
					return _singletonBasicAudioPlayer;
				}
				else
				{
					return _singletonBasicAudioPlayer;
				}
			}
		}

		private BasicAudioPlayer()
		{
			//todo
		}

		public void TogglePause()
		{
			throw new NotImplementedException();//todo
		}

		public void PlayTrack()
		{
			throw new NotImplementedException();//todo
		}

		public double CurrentSongPos { get; set; } //todo

		public Stack<AudioTrack> PlayedSongs => throw new NotImplementedException();

		public AudioTrack CurrentTrack { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
	}
	
}
