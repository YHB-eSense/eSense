using System;
using System.Collections.Generic;
using System.Text;

namespace Karl.Model
{
	/// <summary>
	/// This class handles the modes. It can give you a List of all registered modes.
	/// </summary>
	public class ModeHandler 
	{
		private static ModeHandler _singletonModeHandler;
		public static ModeHandler SingletonModeHandler
		{
			get
			{
				if (_singletonModeHandler == null)
				{
					_singletonModeHandler = new ModeHandler();
				}
				return _singletonModeHandler;
			}
		}
		private AudioPlayer _audioPlayer;
		/// <summary>
		/// All registered modes.
		/// </summary>
		public List<Mode> Modes { get; private set; }

		/// <summary>
		/// Constructor of the mode handler.
		/// </summary>
		/// <param name="audioPlayer">The audioplayer that is the modes do stuff on.</param>
		private ModeHandler()
		{
			_audioPlayer = AudioPlayer.SingletonAudioPlayer;
			Modes = new List<Mode>();
			Modes.Add(new AutostopMode());
			Modes.Add(new MotivationMode());
		}

		public void ResetModes()
		{
			if(Modes != null)
			{
				List<Mode> newModes = new List<Mode>(Modes);
				Modes.Clear();
				Modes = newModes;
			}
		}

	}
}
