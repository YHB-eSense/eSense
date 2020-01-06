using System;
using System.Collections.Generic;
using System.Text;

namespace Karl.Model
{
	/// <summary>
	/// The SettingsHandler is a class where you can change all App Settings and find the current Settings.
	/// </summary>
	public class SettingsHandler
	{
		private static SettingsHandler _singletonSettingsHandler;
		public static SettingsHandler SingletonSettingsHandler
		{
			get
			{
				if (_singletonSettingsHandler == null)
				{
					_singletonSettingsHandler = new SettingsHandler();
				}
				return _singletonSettingsHandler;
			}
		}

		private AudioLib audioLib;
		private AudioPlayer audioPlayer;
		private ConfigFile ConfigFile;
		/// <summary>
		/// The List of registered Languages.
		/// </summary>
		public IList<Language> Languages { get; }
		/// <summary>
		/// The currently selected Language.
		/// </summary>
		public Language CurrentLang { get; set; }
		/// <summary>
		/// The Name of the currently connected Device.
		/// </summary>
		public String DeviceName {
			get
			{
				//todo
				return null;
			}
			set
			{
				//todo
			}
		}
		/// <summary>
		/// The total Steps done.
		/// </summary>
		public int Steps { get; }
		/// <summary>
		/// The Constructor that builds a new SettingsHandler. Only used by AppLogic
		/// </summary>
		/// <param name="audioLib">The Settings Handler chooses the implementation of this.</param>
		/// <param name="audioPlayer">The Settings Handler chooses the implementation of this.</param>
		private SettingsHandler()
		{
			this.audioLib = AudioLib.SingletonAudioLib;
			this.audioPlayer = AudioPlayer.SingletonAudioPlayer;
			ConfigFile = new ConfigFile();
			
		}
		/// <summary>
		/// Reset the Step counter.
		/// </summary>
		public void ResetSteps()
		{
			//todo
		}
	}
}
