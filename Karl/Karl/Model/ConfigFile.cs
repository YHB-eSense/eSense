using System;
using System.Collections.Generic;
using System.Text;

namespace Karl.Model
{
	class ConfigFile
	{
		internal Nullable<Language> Language
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
		internal String AudioLibImp { get; set; }//todo
		private static ConfigFile singletonConfigFile;
		internal static ConfigFile SingletonConfigFile
		{
			get
			{
				if (singletonConfigFile == null)
				{
					singletonConfigFile = new ConfigFile();
					return singletonConfigFile;
				}
				else
				{
					return singletonConfigFile;
				}
			}
			set => singletonConfigFile = value;
		}

		private ConfigFile() { }

		
	}
}
