using System;
using System.Collections.Generic;
using System.Text;

namespace Karl.Model
{
	/// <summary>
	/// This is the Wrapper class for AudioPlayer. The implementation can change freely.
	/// </summary>
	public partial class AudioPlayer
	{
		private AudioLib audioLib;
		private IAudioPlayer audioPlayerImp;
		/// <summary>
		/// The current system volume.
		/// </summary>
		public int Volume { get; set; } //todo react to changes from system
		/// <summary>
		/// The current second you are at in the song.
		/// </summary>
		public double CurrentSecInTrack { get; set; } //todo updating it
		/// <summary>
		/// Is the track paused?
		/// </summary>
		public bool Paused { get; set; }
		/// <summary>
		/// Constructor for the Audioplayer. Only to be used in AppLogic.
		/// </summary>
		/// <param name="audioLib"></param>
		internal AudioPlayer(AudioLib audioLib)
		{
			this.audioLib = audioLib;
		}
		/// <summary>
		/// Pause playback.
		/// </summary>
		public void PauseTrack()
		{
			audioPlayerImp.PauseTrack();
		}
		/// <summary>
		/// Start playing the song currently selected int the Library
		/// </summary>
		public void PlayTrack()
		{
			audioPlayerImp.PlayTrack();
		}
		/// <summary>
		/// Skip current Track.
		/// </summary>
		public void NextTrack()
		{
			audioPlayerImp.NextTrack();
		}
		/// <summary>
		/// Go to previous Track.
		/// </summary>
		public void PrevTrack()
		{
			audioPlayerImp.PrevTrack();
		}

		private interface IAudioPlayer
		{
			void PauseTrack();
			void PlayTrack();
			void NextTrack();
			void PrevTrack();
		}
	}

	/*internal interface IAudioPlayer
	{

	}*/
}
