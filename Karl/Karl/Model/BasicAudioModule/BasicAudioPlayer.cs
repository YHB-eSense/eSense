using System;
using System.Collections.Generic;
using System.Text;

namespace Karl.Model
{
	sealed class BasicAudioPlayer : IAudioPlayer
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

		}
	}
}
