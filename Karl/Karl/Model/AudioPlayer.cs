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
		private AudioLib AudioLib;
		private IAudioPlayerImpl audioPlayerImp;

		private static  AudioPlayer _singletonAudioPlayer;
		/// <summary>
		/// This is a Singleton that enables using the AudioPlayer Model.
		/// </summary>
		public static AudioPlayer SingletonAudioPlayer
		{
			get
			{
				if (_singletonAudioPlayer == null)
				{
					_singletonAudioPlayer = new AudioPlayer();
				}
				return _singletonAudioPlayer;
			}
		}

		private AudioPlayer()
		{
			this.AudioLib = AudioLib.SingletonAudioLib;
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
		/// The Tracks already played before.
		/// </summary>
		public Stack<AudioTrack> PlayedTracks { get; } //todo

		/// <summary>
		/// The Track that is currently chosen.
		/// </summary>
		public AudioTrack CurrentTrack { get; set; } //todo
		/// <summary>
		/// Pause/continue playback.
		/// </summary>
		public void TogglePause()
		{
			audioPlayerImp.TogglePause();
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
			//todo
		}
		/// <summary>
		/// Go to previous Track.
		/// </summary>
		public void PrevTrack()
		{
			//todo
		}
		/// <summary>
		/// Adds a1 to queue.
		/// </summary>
		/// <param name="audioTrack">a1</param>
		public void AddToQueue(AudioTrack audioTrack)
		{
			//todo
		}

		
	}
	interface IAudioPlayerImpl
	{
		Stack<AudioTrack> PlayedSongs { get; }
		Queue<AudioTrack> Queue { get; }
		AudioTrack CurrentTrack { get; set; }
		void TogglePause();
		void PlayTrack();
		/// <summary>
		/// The current position in Song.
		/// </summary>
		double CurrentSongPos { get; set; }
	}
}
