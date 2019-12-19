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
		/// The List of all AudioTracks in the Current Library
		/// </summary>
		/// <returns></returns>
		public IList<AudioTrack> AudioTracks { get; } //todo
													  /// <summary>
													  /// Add a new Track to the current Library
													  /// </summary>
		public void AddTrack(String Indentifier, String Name, String Artist, int BPM)
		{
			//todo
		}

	}

	internal interface IAudioLibImpl
	{
		IList<AudioTrack> AllAudioTracks { get; }
		void AddTrack();
	}
}
