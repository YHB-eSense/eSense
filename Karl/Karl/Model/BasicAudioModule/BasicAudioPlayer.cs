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
			public static BasicAudioPlayer SingletonBasicAudioPlayer
			{
				get
				{
					if (SingletonBasicAudioPlayer == null)
					{
						SingletonBasicAudioPlayer = new BasicAudioPlayer();
						return SingletonBasicAudioPlayer;
					}
					else
					{
						return SingletonBasicAudioPlayer;
					}
				}
				set => SingletonBasicAudioPlayer = value;
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
