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

		internal SettingsHandler(AudioLib audioLib, AudioPlayer audioPlayer)
		{
			this.audioLib = audioLib;
			this.audioPlayer = audioPlayer;
			configFile = ConfigFile.SingletonConfigFile;
		}


	}
}
