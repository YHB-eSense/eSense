using System;
using System.Collections.Generic;
using System.Text;

namespace Karl.Model
{
	public partial class AudioLib
	{
		sealed private class BasicAudioLib : IAudioLib
		{
			private static BasicAudioLib singletonBasicAudioLib;

			public static BasicAudioLib SingletonBasicAudioLib
			{
				get
				{
					if (singletonBasicAudioLib == null)
					{
						singletonBasicAudioLib = new BasicAudioLib();
						return singletonBasicAudioLib;
					}
					else
					{
						return singletonBasicAudioLib;
					}
				}
			}

			public IList<AudioTrack> AudioTracks => throw new NotImplementedException(); //todo

			private BasicAudioLib()
			{
				//todo
			}

			public void AddTrack()
			{
				throw new NotImplementedException(); //todo
			}
		}
	}
}
