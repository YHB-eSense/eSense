using System;
using System.Collections.Generic;
using System.Text;

namespace Karl.Model
{
	/// <summary>
	/// This is the wrapper class for the VM to use.
	/// </summary>
	public partial class AudioLib
	{
		//todo
		//The actual implementation.
		private IAudioLib lib;
		/// <summary>
		/// The Track that is currently chosen.
		/// </summary>
		public AudioTrack CurrentTrack { get; set; }
		/// <summary>
		/// The List of all AudioTracks in the Current Library
		/// </summary>
		/// <returns></returns>
		public IList<AudioTrack> GetAudioTracks()
		{
			return lib.AudioTracks; //todo
		}
		/// <summary>
		/// Add a new Track to the current Library
		/// </summary>
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
