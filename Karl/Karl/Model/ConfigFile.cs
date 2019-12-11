using System;
using System.Collections.Generic;
using System.Text;

namespace Karl.Model
{
	class ConfigFile
	{
		internal Nullable<Language> language
		{
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
		internal String AudioLibImp { get; set; }
		private static ConfigFile singletonConfigFile = null;
		internal static ConfigFile SingletonConfigFile { get
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
