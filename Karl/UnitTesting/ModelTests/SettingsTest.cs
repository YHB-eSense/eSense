#define TESTING
using Moq;
using System;
using System.Diagnostics;
using System.Reflection;
using Xunit;
using Xunit.Sdk;
using Karl.Model;
using SpotifyAPI.Web.Models;
using System.Collections.Generic;
using static Karl.Model.SettingsHandler;
using static Karl.Model.AudioLib;

namespace UnitTesting.ModelTests
{
	public class SettingsTest
	{
		SettingsHandler TestObj;
		TestFramework fw = new TestFramework();
		Mock<IDictionary<string, Object>> mockObj;
		object val;

		void Before()
		{
			mockObj = new Mock<IDictionary<string, Object>>();
			PropertiesInjection(mockObj.Object);
			fw.ResetSingletons();
		}

		void After()
		{
			fw.ResetSingletons();
			PropertiesInjection(null);
		}

		[Fact]
		public void ColorTest()
		{
			BeforeAfterTest(ColorTestCase);
		}

		void ColorTestCase()
		{
			TestObj = SingletonSettingsHandler;
			mockObj.Setup(m => m.TryGetValue("color", out val)).Returns(false);
			mockObj.Verify(mock => mock.Add("color", "#FF4169E1"));
		}

		[Fact]
		public void LanguageTest()
		{
			BeforeAfterTest(LangNotFoundTestCase);
		}

		void LangNotFoundTestCase()
		{
			Mocks.TestDictionary testDictionary = new Mocks.TestDictionary();
			testDictionary.Add("lang", "TestLang");
			PropertiesInjection(testDictionary);
			TestObj = SingletonSettingsHandler;
			Assert.Equal("lang_english", SingletonSettingsHandler.CurrentLang.Tag);
			Assert.True(testDictionary.AddCalled && testDictionary.LangTest && testDictionary.RemoveCalled);
		}

		[Fact]
		public void LanguageTest2()
		{
			BeforeAfterTest(NoLangPropertyTestCase);
		}

		void NoLangPropertyTestCase()
		{
			Mocks.TestDictionary keyValuePairs = new Mocks.TestDictionary();
			PropertiesInjection(keyValuePairs);
			TestObj = SingletonSettingsHandler;
			Assert.Equal("lang_english", SingletonSettingsHandler.CurrentLang.Tag);
			Assert.True(keyValuePairs.AddCalled && keyValuePairs.LangTest);
		}

		[Fact]
		public void UseDifferentLibs()
		{
			BeforeAfterTest(UseDifferentLibsTestCase);
		}

		void UseDifferentLibsTestCase()
		{
			var called = false;
			SpotifyAudioLib.Testing(true);
			TestObj = SingletonSettingsHandler;
			TestObj.AudioModuleChanged += (object source, EventArgs args) => { called = true; };
			TestObj.changeAudioModuleToSpotify();
			Assert.True(called);
			Assert.True(SingletonAudioLib.Playlists == null);
			Assert.True(SingletonAudioLib.SelectedPlaylist == null);
		}

		[Fact]
		public void UseDifferentLibs2()
		{
			BeforeAfterTest(UseDifferentLibsTestCase2);
		}

		void UseDifferentLibsTestCase2()
		{
			SpotifyAudioLib.Testing(true);
			TestObj = SingletonSettingsHandler;
			TestObj.changeAudioModuleToSpotify();
			Assert.ThrowsAsync<NotImplementedException>(async () => await SingletonAudioLib.AddTrack("TEST", "TEST", "TEST",0));
			Assert.Throws<NotImplementedException>(() => SingletonAudioLib.DeleteTrack(new SpotifyAudioTrack(0.0,"TEST","TEST",0,"TEST",null)));
		}

		void BeforeAfterTest(Action TestCase)
		{
			Before();
			TestCase.Invoke();
			After();
		}
		
	}
}
