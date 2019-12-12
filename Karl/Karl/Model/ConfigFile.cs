using System;
using System.Collections.Generic;
using System.Text;

namespace Karl.Model
{
	/// <summary>
	/// This represents the config File. This file will also be present on your devices storage.
	/// It will load and store changes to the Settings.
	/// </summary>
	class ConfigFile
	{
		/// <summary>
		/// The Language used
		/// </summary>
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
		/// <summary>
		/// The AudioModule used.
		/// </summary>
		internal String AudioModule { get; set; }//todo

		private static ConfigFile _singletonConfigFile;
		internal static ConfigFile SingletonConfigFile
		{
			get
			{
				if (_singletonConfigFile == null)
				{
					_singletonConfigFile = new ConfigFile();
					return _singletonConfigFile;
				}
				else
				{
					return _singletonConfigFile;
				}
			}
			set => _singletonConfigFile = value;
		}

		private ConfigFile() { }
		
	}
}
