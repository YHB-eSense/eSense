using System;
using System.Collections.Generic;
using System.Text;

namespace Karl.Model
{
	sealed class BasicAudioLib : IAudioLib
	{
		private static BasicAudioLib singletonBasicAudioLib;

		public static BasicAudioLib SingletonBasicAudioLib { get
			{
				if (singletonBasicAudioLib == null)
				{
					singletonBasicAudioLib = new BasicAudioLib();
					return singletonBasicAudioLib;
				} else
				{
					return singletonBasicAudioLib;
				}
			}
		}

		private BasicAudioLib()
		{

		}
	}
}
