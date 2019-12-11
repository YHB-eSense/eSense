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
		internal static ConfigFile SingletonConfigFile
		{
			get
			{
				if (SingletonConfigFile == null)
				{
					SingletonConfigFile = new ConfigFile();
					return SingletonConfigFile;
				}
				else
				{
					return SingletonConfigFile;
				}
			}
			set => SingletonConfigFile = value;
		}

		private ConfigFile() { }

		
	}
}
