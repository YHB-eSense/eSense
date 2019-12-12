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
		private AudioLib audioLib;
		private AudioPlayer audioPlayer;
		private ConfigFile configFile;
		/// <summary>
		/// The List of registered Languages.
		/// </summary>
		public List<Language> Languages { get; }
		/// <summary>
		/// The currently selected Language.
		/// </summary>
		public Language CurrentLang { get; }
		/// <summary>
		/// The Name of the currently connected Device.
		/// </summary>
		public String DeviceName { get
			{
				//todo
				return null;
			}
			set
			{
				//todo
			} }
		/// <summary>
		/// The Constructor that builds a new SettingsHandler. Only used by AppLogic
		/// </summary>
		/// <param name="audioLib">The Settings Handler chooses the implementation of this.</param>
		/// <param name="audioPlayer">The Settings Handler chooses the implementation of this.</param>
		internal SettingsHandler(AudioLib audioLib, AudioPlayer audioPlayer)
		{
			this.audioLib = audioLib;
			this.audioPlayer = audioPlayer;
			configFile = ConfigFile.SingletonConfigFile;
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
