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
		private Stack<AudioTrack> _songsAfter;
		public Stack<AudioTrack> SongsBefore { get; private set; }
		public Queue<AudioTrack> SongsQueue { get; private set; }

		public delegate void EventListener();

		public event EventListener NextSongEvent;
		private void InvokeNextSongEvent()
		{
			NextSongEvent?.Invoke();
		}

		//Eventhandling
		public event EventHandler AudioChanged;

		/// <summary>
		/// The Track that is currently chosen.
		/// </summary>
		public AudioTrack CurrentTrack
		{
			get { return _audioPlayerImp.CurrentTrack; }
			set
			{
				if (_audioPlayerImp.CurrentTrack != value)
				{
					_audioPlayerImp.CurrentTrack = value;
					AudioChanged?.Invoke(this, null);
				}
			}
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
			SongsBefore = new Stack<AudioTrack>();
			_songsAfter = new Stack<AudioTrack>();
			Paused = true;
		}

		/// <summary>
		/// Start playing the song currently selected in the Library
		/// </summary>
		public void PlayTrack(AudioTrack track)
		{
			if (CurrentTrack != null) { SongsBefore.Push(CurrentTrack); }
			Paused = false;
			_audioPlayerImp.PlayTrack(track);
			AudioChanged?.Invoke(this, null);
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
				SongsBefore.Push(CurrentTrack);
				if (SongsQueue.Count != 0) _audioPlayerImp.PlayTrack(SongsQueue.Dequeue());
				else _audioPlayerImp.PlayTrack(_songsAfter.Pop());
			}
		}

		/// <summary>
		/// Go to previous Track.
		/// </summary>
		public void PrevTrack()
		{
			if (SongsBefore.Count != 0)
			{
				Paused = false;
				_songsAfter.Push(CurrentTrack);
				_audioPlayerImp.PlayTrack(SongsBefore.Pop());
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

		/// <summary>
		/// Clears Songs played and queue
		/// </summary>
		public void Clear()
		{
			SongsBefore.Clear();
			SongsQueue.Clear();
			_songsAfter.Clear();
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
