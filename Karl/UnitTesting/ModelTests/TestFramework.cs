using Karl.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace UnitTesting.ModelTests
{
	class TestFramework
	{
		FieldInfo _singeltonInstanceSettingsHandler;
		FieldInfo _singeltonInstanceAudioLib;
		FieldInfo _singeltonInstanceAudioPlayer;
		FieldInfo _singletonInstanceLangManager;
		public TestFramework()
		{
			_singeltonInstanceSettingsHandler = typeof(SettingsHandler).GetField("_singletonSettingsHandler", BindingFlags.Static | BindingFlags.NonPublic);
			_singeltonInstanceAudioLib = typeof(AudioLib).GetField("_singletonAudioLib", BindingFlags.Static | BindingFlags.NonPublic);
			_singeltonInstanceAudioPlayer = typeof(AudioPlayer).GetField("_singletonAudioPlayer", BindingFlags.Static | BindingFlags.NonPublic);
			_singletonInstanceLangManager = typeof(LangManager).GetField("_singletonLangManager", BindingFlags.Static | BindingFlags.NonPublic);
		}

		public void ResetSingletons()
		{
			_singeltonInstanceSettingsHandler.SetValue(null, null);
			_singeltonInstanceAudioLib.SetValue(null, null);
			_singeltonInstanceAudioPlayer.SetValue(null, null);
			_singletonInstanceLangManager.SetValue(null, null);
		}
	}
}
