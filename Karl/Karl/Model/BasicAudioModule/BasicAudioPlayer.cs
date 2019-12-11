using System;
using System.Collections.Generic;
using System.Text;

namespace Karl.Model
{
	public partial class AudioPlayer
	{
		private sealed class BasicAudioPlayer : IAudioPlayer
		{
			private static BasicAudioPlayer singletonBasicAudioPlayer;

			public static BasicAudioPlayer SingletonBasicAudioPlayer
			{
				get
				{
					if (singletonBasicAudioPlayer == null)
					{
						singletonBasicAudioPlayer = new BasicAudioPlayer();
						return singletonBasicAudioPlayer;
					}
					else
					{
						return singletonBasicAudioPlayer;
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
		}
	}
}
