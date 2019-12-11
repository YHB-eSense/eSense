using System;
using System.Collections.Generic;
using System.Text;

namespace Karl.Model
{
	public partial class AudioLib
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
			return lib.AudioTracks; //todo
		}

		public void AddTrack()
		{
			//todo
		}

		private interface IAudioLib
		{
			IList<AudioTrack> AudioTracks { get; }
			void AddTrack();
			//todo
		}

	}

	/*internal interface IAudioLib
	{
		IList<AudioTrack> AudioTracks { get; }
			void AddTrack();
			//todo
		
	}*/

}
