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
		public ObservableCollection<AudioTrack> AudioTracks
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

		public void changeToSpotify()
		{
			SpotifyAudioLib lib = new SpotifyAudioLib();
			lib.webAPI = eSenseSpotifyWebAPI.WebApiSingleton.api;
			lib.Profile = eSenseSpotifyWebAPI.WebApiSingleton.UsersProfile;
			_audioLibImp = lib;
			_audioLibImp.Init();
		}
	}

	internal interface IAudioLibImpl
	{
		ObservableCollection<AudioTrack> AllAudioTracks { get; set; }
		void AddTrack(string storage, string title, string artist, int bpm);
		void DeleteTrack(AudioTrack track);
		void Init();
	}
}
