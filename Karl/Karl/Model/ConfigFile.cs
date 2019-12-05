using System;
using System.Collections.Generic;
using System.Text;

namespace Karl.Model
{
	internal class ConfigFile
	{
		private static ConfigFile singletonConfigFile = null;
		public static ConfigFile SingletonConfigFile { get
			{
				if (singletonConfigFile == null)
				{
					singletonConfigFile = new ConfigFile();
					return singletonConfigFile;
				} else
				{
					return singletonConfigFile;
				}
			}
		}

		private ConfigFile() { }

	}
}
