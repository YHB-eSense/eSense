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
			//testing BasicAudioLib
			_audioLibImp = new BasicAudioLib();
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
		public void AddTrack(String indentifier, String name, String artist, int bpm)
		{
			_audioLibImp.AddTrack(indentifier, name, artist, bpm);
		}

	}

	internal interface IAudioLibImpl
	{
		ObservableCollection<AudioTrack> AllAudioTracks { get; set; }
		void AddTrack(String indentifier, String name, String artist, int bpm);
	}
}
