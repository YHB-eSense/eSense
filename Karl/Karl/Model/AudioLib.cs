using System;
using System.Collections.Generic;
using System.Text;

namespace Karl.Model
{
	/// <summary>
	/// This is the wrapper class for the VM to use.
	/// </summary>
	public sealed class AudioLib
	{
		private IAudioLibImpl AudioLibImp;

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
		}

		/// <summary>
		/// The Tracks already played before.
		/// </summary>
		public List<AudioTrack> PlayedTracks { get; } //todo

		/// <summary>
		/// The Track that is currently chosen.
		/// </summary>
		public AudioTrack CurrentTrack { get; set; } //todo
		/// <summary>
		/// The List of all AudioTracks in the Current Library
		/// </summary>
		/// <returns></returns>
		public IList<AudioTrack> GetAudioTracks { get; } //todo
		/// <summary>
		/// Add a new Track to the current Library
		/// </summary>
		public void AddTrack()
		{
			//todo
		}
		/// <summary>
		/// This skips the current Song and makes the next Song in the queue the current Song.
		/// </summary>
		public void NextSong()
		{
			//todo
		}
		/// <summary>
		/// This makes the previous Song the current Song.
		/// </summary>
		public void PrevSong()
		{
			//todo
		}

	}

	internal interface IAudioLibImpl
	{
		IList<AudioTrack> PlayedSongs { get; }
		AudioTrack CurrentTrack { get; set; }
		IList<AudioTrack> AllAudioTracks { get; }
		void AddTrack();
		void NextTrack();
		void PrevSong();
	}
}
