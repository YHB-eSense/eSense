using Karl.Data;
using Karl.Model;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnitTesting.Mocks;

namespace UnitTesting.ModelTests
{
	class TestFramework
	{

		private List<Action> _after = new List<Action>();
		public IList<Action> AfterActions
		{
			get
			{
				var result = _after;
				_after = new List<Action>();
				return result;
			}
		}

		FieldInfo _singeltonInstanceSettingsHandler;
		FieldInfo _singeltonInstanceAudioLib;
		FieldInfo _singeltonInstanceAudioPlayer;
		FieldInfo _singletonInstanceLangManager;
		FieldInfo _singletonInstanceBasicDatabase;
		public TestFramework()
		{
			_singeltonInstanceSettingsHandler = typeof(SettingsHandler).GetField("_singletonSettingsHandler", BindingFlags.Static | BindingFlags.NonPublic);
			_singeltonInstanceAudioLib = typeof(AudioLib).GetField("_singletonAudioLib", BindingFlags.Static | BindingFlags.NonPublic);
			_singeltonInstanceAudioPlayer = typeof(AudioPlayer).GetField("_singletonAudioPlayer", BindingFlags.Static | BindingFlags.NonPublic);
			_singletonInstanceLangManager = typeof(LangManager).GetField("_singletonLangManager", BindingFlags.Static | BindingFlags.NonPublic);
			_singletonInstanceBasicDatabase = typeof(BasicAudioTrackDatabase).GetField("_singletonDatabase", BindingFlags.NonPublic | BindingFlags.Static);
		}


		public void ResetSingletons()
		{
			_singeltonInstanceSettingsHandler.SetValue(null, null);
			_singeltonInstanceAudioLib.SetValue(null, null);
			_singeltonInstanceAudioPlayer.SetValue(null, null);
			_singletonInstanceLangManager.SetValue(null, null);
			_singletonInstanceBasicDatabase.SetValue(null, null);
		}

		public void MockDatabase()
		{
			var mock = new DatabaseMock();
			_singletonInstanceBasicDatabase.SetValue(null, mock);
			_after.Add(() => { _singletonInstanceBasicDatabase.SetValue(null, null); });
		}
	}
}
