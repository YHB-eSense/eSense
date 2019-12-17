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

		public void PauseTrack()
		{
			throw new NotImplementedException();//todo
		}

		public void PlayTrack()
		{
			throw new NotImplementedException();//todo
		}

		public void NextTrack()
		{
			throw new NotImplementedException();//todo
		}

		public void PrevTrack()
		{
			throw new NotImplementedException();//todo
		}

		public double CurrentSongPos()
		{
			throw new NotImplementedException();
		}
	}
	
}
