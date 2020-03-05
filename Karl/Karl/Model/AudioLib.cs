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
	public sealed class AudioLib
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
					_singletonAudioLib._audioLibImp = new BasicAudioLib();
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
			set => _audioLibImp.SelectedPlaylist = value;
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


		private AudioLib()
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
		public async Task AddTrack(string storage, string title, string artist, int bpm)
		{
			await _audioLibImp.AddTrack(storage, title, artist, bpm);
			AudioLibChanged?.Invoke(this, null);
		}

		public void DeleteTrack(AudioTrack track)
		{
			_audioLibImp.DeleteTrack(track);
			AudioLibChanged?.Invoke(this, null);
		}

		public void changeToSpotifyLib()
		{
			_audioLibImp = new SpotifyAudioLib();
			_audioLibImp.Init();
		}

		public void ChangeToBasicLib()
		{
			_audioLibImp = new BasicAudioLib();
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
		void DeleteTrack(AudioTrack track);
		void Init();

		event AudioLibEventHandler AudioLibChanged;
	}
}
