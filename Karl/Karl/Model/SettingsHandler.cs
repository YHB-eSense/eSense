using System;
using System.Collections.Generic;
using System.Text;

namespace Karl.Model
{
	public class SettingsHandler
	{
		private AudioLib audioLib;
		private ConfigFile configFile;

		internal SettingsHandler(AudioLib audioLib)
		{
			this.audioLib = audioLib;
			configFile = ConfigFile.SingletonConfigFile;
		}


	}
}
