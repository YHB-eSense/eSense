using System;
using System.Collections.Generic;
using System.Text;

namespace Karl.Model
{
	public class AudioLib
	{
		//todo
		private IAudioLib lib;
		public AudioTrack currentTrack { get; set; }

		protected void useBasicAudioLib()
		{
			lib = BasicAudioLib.SingletonBasicAudioLib;
		}

		public IList<AudioTrack> getAudioTracks()
		{
			return null; //todo
		}

		public void addTrack()
		{
			//todo
		}

	}
	internal interface IAudioLib
	{
		//todo
	}

}
