using System;
using System.Collections.Generic;
using System.Text;

namespace Karl.Model
{
	public partial class AudioPlayer
	{
		internal void UseBasicAudioPlayer()
		{
			audioPlayerImp = BasicAudioPlayer.SingletonBasicAudioPlayer;
		}
		private sealed class BasicAudioPlayer : IAudioPlayer
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
				set => _singletonBasicAudioPlayer = value;
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
		}
	}
}
