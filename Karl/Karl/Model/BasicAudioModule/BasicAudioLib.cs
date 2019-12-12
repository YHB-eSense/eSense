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
			private static BasicAudioLib _singletonBasicAudioLib;
			public static BasicAudioLib SingletonBasicAudioLib
			{
				get
				{
					if (_singletonBasicAudioLib == null)
					{
						_singletonBasicAudioLib = new BasicAudioLib();
						return _singletonBasicAudioLib;
					}
					else
					{
						return _singletonBasicAudioLib;
					}
				}
				private set => _singletonBasicAudioLib = value;
			}

			public IList<AudioTrack> AudioTracks => throw new NotImplementedException(); //todo

			public IList<AudioTrack> Queue { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

			public IList<AudioTrack> PlayedSongs => throw new NotImplementedException();

			private BasicAudioLib()
			{
				//todo
			}

			public void AddTrack()
			{
				throw new NotImplementedException(); //todo
			}

			public void NextSong()
			{
				throw new NotImplementedException();
			}

			public void PrevSong()
			{
				throw new NotImplementedException();
			}
		}
	}
}
