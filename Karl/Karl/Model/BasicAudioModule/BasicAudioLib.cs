using System;
using System.Collections.Generic;
using System.Text;

namespace Karl.Model
{

	sealed class BasicAudioLib : IAudioLibImpl
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

		public Stack<AudioTrack> PlayedSongs => throw new NotImplementedException();

		public AudioTrack CurrentTrack { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

		public IList<AudioTrack> AllAudioTracks => throw new NotImplementedException();

		public void AddTrack()
		{
			throw new NotImplementedException();
		}
	}
	
}
