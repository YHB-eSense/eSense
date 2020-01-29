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
		public delegate void AudioEventHandler(object source, EventArgs e);
		public event AudioEventHandler AudioChanged;

		/// <summary>
		/// The Track that is currently chosen.
		/// </summary>
		public AudioTrack CurrentTrack
		{
			get { return _audioPlayerImp.CurrentTrack; }
		}

		public double Volume
		{
			get => _audioPlayerImp.Volume;
			set => _audioPlayerImp.Volume = value;
		}
		/// <summary>
		/// The current second you are at in the song.
		/// </summary>
		public double CurrentSecInTrack
		{
			get => _audioPlayerImp.CurrentSongPos;
			set => _audioPlayerImp.CurrentSongPos = value;
		}

		/// <summary>
		/// Is the track paused?
		/// </summary>
		public bool Paused
		{
			get => _audioPlayerImp.Paused;
			set => _audioPlayerImp.Paused = value;
		}

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
			//_audioPlayerImp = SettingsHandler.SingletonSettingsHandler.CurrentAudioModule.AudioPlayer;
			//SettingsHandler.SingletonSettingsHandler.AudioModuleChanged += UpdateAudioModule;
			_audioPlayerImp = new BasicAudioPlayer();
			SongsQueue = new Queue<AudioTrack>();
			SongsBefore = new Stack<AudioTrack>();
			_songsAfter = new Stack<AudioTrack>();

		}

		public void changeToSpotifyPlayer()
		{
			resetQueuedandPlayedTracks();
			if(!_audioPlayerImp.Paused)_audioPlayerImp.TogglePause();
			_audioPlayerImp = new SpotifyAudioPlayer();
			
		}

		public void changeToBasicPlayer()
		{
			resetQueuedandPlayedTracks();
			_audioPlayerImp = new BasicAudioPlayer();
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
			if (_songsAfter.Count != 0 || SongsQueue.Count != 0)
			{
				Paused = false;
				SongsBefore.Push(CurrentTrack);
				if (SongsQueue.Count != 0) _audioPlayerImp.PlayTrack(SongsQueue.Dequeue());
				else _audioPlayerImp.PlayTrack(_songsAfter.Pop());
				AudioChanged?.Invoke(this, null);
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
				AudioChanged?.Invoke(this, null);
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

		/*
		private void UpdateAudioModule(SettingsHandler.AudioModule audioModule)
		{
			_audioPlayerImp = audioModule.AudioPlayer;
			//TODO
		}
		*/

		private void resetQueuedandPlayedTracks() {
			SongsQueue = new Queue<AudioTrack>();
			SongsBefore = new Stack<AudioTrack>();
			_songsAfter = new Stack<AudioTrack>();
		}
	}

	interface IAudioPlayerImpl
	{
		AudioTrack CurrentTrack { get; set; }
		double CurrentSongPos { get; set; }
		double Volume { get; set; }
		bool Paused { get; set; }
		void PlayTrack(AudioTrack track);
		void TogglePause();
	}

}
