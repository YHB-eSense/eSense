using SpotifyAPI.Web.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static Karl.Model.AudioLib;

namespace Karl.Model
{
	/// <summary>
	/// This is the wrapper class for the VM to use.
	/// </summary>
	public class AudioLib
	{
		private IAudioLibImpl _audioLibImp;
		private static AudioLib _singletonAudioLib;

		public static AudioLib SingletonAudioLib
		{
			get
			{
				if (_singletonAudioLib == null)
				{
					_singletonAudioLib = new AudioLib();
					return _singletonAudioLib;
				}
				else
				{
					return _singletonAudioLib;
				}
			}
			private set => _singletonAudioLib = value;
		}

		public SimplePlaylist[] Playlists
		{
			get => _audioLibImp.AllPlaylists;
		}

		public SimplePlaylist SelectedPlaylist
		{
			get => _audioLibImp.SelectedPlaylist;
			set
			{
				foreach (var playlist in Playlists)
					if (value == playlist)
					{
						_audioLibImp.SelectedPlaylist = value;
						return;
					}
				throw new ArgumentException("This Playlist does not exist.");
			}
		}

		/// <summary>
		/// The List of all AudioTracks in the Current Library
		/// </summary>
		/// <returns></returns>
		public List<AudioTrack> AudioTracks
		{
			get { return _audioLibImp.AllAudioTracks; }
			set { _audioLibImp.AllAudioTracks = value; }
		}


		public delegate void AudioLibEventHandler(object source, EventArgs e);
		public event AudioLibEventHandler AudioLibChanged;
		public delegate void AudioLibSwitchedHandler();
		public event AudioLibSwitchedHandler AudioLibSwitched;


		protected AudioLib()
		{
			_singletonAudioLib = this;
			_audioLibImp = new BasicAudioLib();
			//SettingsHandler.SingletonSettingsHandler.CurrentAudioModule.AudioLib;
			_audioLibImp.Init();
			//SettingsHandler.SingletonSettingsHandler.AudioModuleChanged += UpdateAudioLib;
			_audioLibImp.AudioLibChanged += UpdateLib;

		}

		/// <summary>
		/// Add a new Track to the current Library
		/// </summary>
		public virtual async Task AddTrack(string storage, string title, string artist, int bpm)
		{
			if (bpm < 0) throw new ArgumentException("BPM can't be negative.");
			await _audioLibImp.AddTrack(storage, title, artist, bpm);
			AudioLibChanged?.Invoke(this, null);
		}

		public virtual async Task DeleteTrack(AudioTrack track)
		{
			await _audioLibImp.DeleteTrack(track);
			AudioLibChanged?.Invoke(this, null);
		}

		public void ChangeToSpotifyLib()
		{
			_audioLibImp = new SpotifyAudioLib();
			_audioLibImp.Init();
			AudioLibSwitched?.Invoke();
		}

		public void ChangeToBasicLib()
		{
			_audioLibImp = new BasicAudioLib();
			AudioLibSwitched?.Invoke();
		}

		private void UpdateLib(object sender, EventArgs args)
		{
			AudioLibChanged?.Invoke(this, null);
		}


	}

	internal interface IAudioLibImpl
	{
		List<AudioTrack> AllAudioTracks { get; set; }
		SimplePlaylist[] AllPlaylists { get; }
		SimplePlaylist SelectedPlaylist { get; set; }
		Task AddTrack(string storage, string title, string artist, int bpm);
		Task DeleteTrack(AudioTrack track);
		void Init();
		event AudioLibEventHandler AudioLibChanged;
	}

}
