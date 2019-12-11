using System;
using System.Collections.Generic;
using System.Text;

namespace Karl.Model
{
	public partial class AudioLib
	{
		protected void UseBasicAudioLib()
		{
			lib = BasicAudioLib.SingletonBasicAudioLib;
		}
		sealed private class BasicAudioLib : IAudioLib
		{
			public static BasicAudioLib SingletonBasicAudioLib
			{
				get
				{
					if (SingletonBasicAudioLib == null)
					{
						SingletonBasicAudioLib = new BasicAudioLib();
						return SingletonBasicAudioLib;
					}
					else
					{
						return SingletonBasicAudioLib;
					}
				}
				private set => SingletonBasicAudioLib = value;
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
