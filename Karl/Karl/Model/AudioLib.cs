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
		/// <summary>
		/// This is the queue of next songs.
		/// </summary>
		public List<AudioTrack> Queue
		{
			get
			{
				return null; //todo
			}
			set
			{
				//todo Add song to queue
			}
		}
		/// <summary>
		/// The Tracks already played before.
		/// </summary>
		public List<AudioTrack> PlayedTracks
		{
			get
			{
				return null;//todo
			}
		}

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
		/// <summary>
		/// This skips the current Song and makes the next Song in the queue the current Song.
		/// </summary>
		public void NextSong()
		{
			//todo
		}
		/// <summary>
		/// This makes the previous Song the current Song.
		/// </summary>
		public void PrevSong()
		{
			//todo
		}

		private interface IAudioLib
		{
			IList<AudioTrack> AudioTracks { get; }
			IList<AudioTrack> Queue { get; set; }
			IList<AudioTrack> PlayedSongs { get; }
			void AddTrack();
			void NextSong();
			void PrevSong();

			//todo
		}

	}
}
