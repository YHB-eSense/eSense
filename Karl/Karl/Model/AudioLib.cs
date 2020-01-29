using SpotifyAPI.Web.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Karl.Model
{
	/// <summary>
	/// This is the wrapper class for the VM to use.
	/// </summary>
	public sealed class AudioLib
	{
		private IAudioLibImpl _audioLibImp;
		private static AudioLib _singletonAudioLib;

		public delegate void AudioLibEventHandler(object source, EventArgs e);
		public event AudioLibEventHandler AudioLibChanged;

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
			set => _audioLibImp.SelectedPlaylist = value;
		}

		private AudioLib()
		{
			_singletonAudioLib = this;
			_audioLibImp = new BasicAudioLib();
			//SettingsHandler.SingletonSettingsHandler.CurrentAudioModule.AudioLib;
			_audioLibImp.Init();
			//SettingsHandler.SingletonSettingsHandler.AudioModuleChanged += UpdateAudioLib;

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

		/// <summary>
		/// Add a new Track to the current Library
		/// </summary>
		public void AddTrack(string storage, string title, string artist, int bpm)
		{
			_audioLibImp.AddTrack(storage, title, artist, bpm);
			AudioLibChanged?.Invoke(this, null);
		}

		public void DeleteTrack(AudioTrack track)
		{
			_audioLibImp.DeleteTrack(track);
		}

		private void UpdateAudioLib(object sender, EventArgs e)
		{
			/*
			audioModule.AudioLib.Init();
			_audioLibImp = audioModule.AudioLib;
			//TODO
			*/
		}

		public void changeToSpotifyLib()
		{
			_audioLibImp = new SpotifyAudioLib();
			_audioLibImp.Init();
		}

		public void changeToBasicLib() {
			_audioLibImp = new BasicAudioLib();
		}
	}

	internal interface IAudioLibImpl
	{
		List<AudioTrack> AllAudioTracks { get; set; }
	    SimplePlaylist[] AllPlaylists { get; }
		SimplePlaylist SelectedPlaylist { get; set; }
		void AddTrack(string storage, string title, string artist, int bpm);
		void DeleteTrack(AudioTrack track);
		void Init();
	}
}
