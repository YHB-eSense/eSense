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
		private static AudioPlayer _singletonAudioPlayer;
		private IAudioPlayerImpl _audioPlayerImp;
		private Stack<AudioTrack> _songsBefore;
		private Stack<AudioTrack> _songsAfter;

		/// <summary>
		/// The Track that is currently chosen.
		/// </summary>
		public AudioTrack CurrentTrack
		{
			get { return _audioPlayerImp.CurrentTrack; }
			set { _audioPlayerImp.CurrentTrack = value; }
		}

		/// <summary>
		/// The current system volume.
		/// </summary>
		public double Volume
		{
			get { return _audioPlayerImp.Volume; }
			set { _audioPlayerImp.Volume = value; }
		}

		/// <summary>
		/// The current second you are at in the song.
		/// </summary>
		public double CurrentSecInTrack
		{
			get { return _audioPlayerImp.CurrentSongPos; }
			set { _audioPlayerImp.CurrentSongPos = value; }
		} 

		/// <summary>
		/// Is the track paused?
		/// </summary>
		public bool Paused { get; set; }

		/// <summary>
		/// This is a Singleton that enables using the AudioPlayer Model.
		/// </summary>
		public static AudioPlayer SingletonAudioPlayer
		{
			get
			{
				if (_singletonAudioPlayer == null) { _singletonAudioPlayer = new AudioPlayer(); }
				return _singletonAudioPlayer;
			}
		}

		/// <summary>
		/// Private Constructor initializes AudioLib
		/// </summary>
		private AudioPlayer()
		{
			//testing BasicAudioPlayer
			_audioPlayerImp = new BasicAudioPlayer();
			_songsBefore = new Stack<AudioTrack>();
			_songsAfter = new Stack<AudioTrack>();
		}

		/// <summary>
		/// Start playing the song currently selected in the Library
		/// </summary>
		public void PlayTrack(AudioTrack track)
		{
			if (CurrentTrack != null) { _songsBefore.Push(CurrentTrack); }
			Paused = false;
			_audioPlayerImp.PlayTrack(track);
		}

		/// <summary>
		/// Pause/continue playback.
		/// </summary>
		public void TogglePause()
		{
			Paused = !Paused;
			_audioPlayerImp.TogglePause();
		}

		/// <summary>
		/// Skip current Track.
		/// </summary>
		public void NextTrack()
		{
			if (_songsAfter.Count != 0)
			{
				Paused = false;
				_songsBefore.Push(CurrentTrack);
				_audioPlayerImp.PlayTrack(_songsAfter.Pop());
			}
		}

		/// <summary>
		/// Go to previous Track.
		/// </summary>
		public void PrevTrack()
		{
			if (_songsBefore.Count != 0)
			{
				Paused = false;
				_songsAfter.Push(CurrentTrack);
				_audioPlayerImp.PlayTrack(_songsBefore.Pop());
			}
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
		AudioTrack CurrentTrack { get; set; }
		double Volume { get; set; }
		double CurrentSongPos { get; set; }
		void PlayTrack(AudioTrack track);
		void TogglePause();
	}
}
