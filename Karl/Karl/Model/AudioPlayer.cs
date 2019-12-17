using System;
using System.Collections.Generic;
using System.Text;

namespace Karl.Model
{
	/// <summary>
	/// This is the Wrapper class for AudioPlayer. The implementation can change freely.
	/// </summary>
	public class AudioPlayer
	{
		private AudioLib audioLib;
		private IAudioPlayerImpl audioPlayerImp;

		private AudioPlayer _singletonAudioPlayer;
		/// <summary>
		/// This is a Singleton that enables using the AudioPlayer Model.
		/// </summary>
		public AudioPlayer SingletonAudioPlayer
		{
			get
			{
				if (_singletonAudioPlayer == null)
				{
					_singletonAudioPlayer = new AudioPlayer();
					audioLib = AudioLib.SingletonAudioLib;
				}
				return _singletonAudioPlayer;
			}
		}
		
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
		/// This String helps Indentify a song.
		/// </summary>
		internal String Indetifier { get; } //todo
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

		
	}
	interface IAudioPlayerImpl
	{
		void PauseTrack();
		void PlayTrack();
		void NextTrack();
		void PrevTrack();
		/// <summary>
		/// The current position in Song.
		/// </summary>
		double CurrentSongPos();
	}
}
