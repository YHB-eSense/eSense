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

		protected void UseBasicAudioLib()
		{
			lib = BasicAudioLib.SingletonBasicAudioLib;
		}

		public IList<AudioTrack> GetAudioTracks()
		{
			return null; //todo
		}

		public void AddTrack()
		{
			//todo
		}

	}
	internal interface IAudioLib
	{
		//todo
	}

}
