using System;
using System.Collections.Generic;
using System.Text;

namespace Karl.Model
{
	public class SettingsHandler
	{
		private AudioLib audioLib;
		private AudioPlayer audioPlayer;
		private ConfigFile configFile;
		public List<> Languages { get; }
		//currentLanguage
		public String DeviceName { get
			{
				//todo
				return null;
			}
			set
			{
				//todo
			} }

		internal SettingsHandler(AudioLib audioLib, AudioPlayer audioPlayer)
		{
			this.audioLib = audioLib;
			this.audioPlayer = audioPlayer;
			configFile = ConfigFile.SingletonConfigFile;
		}

		public void resetSteps()
		{
			//todo
		}
	}
}
