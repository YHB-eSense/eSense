#define TESTING
using Moq;
using System;
using Xunit;
using Karl.Model;
using System.Collections.Generic;
using static Karl.Model.SettingsHandler;
using static Karl.Model.AudioLib;
using Karl.Data;

namespace UnitTesting.ModelTests
{
	public class SettingsTest
	{
		SettingsHandler TestObj;
		TestFramework fw = new TestFramework();
		Mock<IDictionary<string, Object>> mockObj;
		object val;

		void BeforeAfterTest(Action TestCase)
		{
			Before();
			TestCase.Invoke();
			After();

			foreach (var action in fw.AfterActions)
			{
				action.Invoke();
			}
		}

		void Before()
		{
			SpotifyAudioLib.Testing(true);
			BasicAudioTrackDatabase.Testing(true);
			SettingsHandler.Testing(true);
			mockObj = new Mock<IDictionary<string, Object>>();
			PropertiesInjection(mockObj.Object);
			fw.ResetSingletons();
		}

		void After()
		{
			SpotifyAudioLib.Testing(false);
			BasicAudioTrackDatabase.Testing(false);
			SettingsHandler.Testing(false);
			fw.ResetSingletons();
			PropertiesInjection(null);
		}

		[Fact]
		public void ColorTest()
		{
			BeforeAfterTest(() =>
			{
				TestObj = SingletonSettingsHandler;
				mockObj.Setup(m => m.TryGetValue("color", out val)).Returns(false);
				mockObj.Verify(mock => mock.Add("color", "#FF4169E1"));
			});
		}

		[Fact]
		public void LanguageTest()
		{
			BeforeAfterTest(() =>
			{
				Mocks.TestDictionary testDictionary = new Mocks.TestDictionary();
				testDictionary.Add("lang", "TestLang");
				PropertiesInjection(testDictionary);
				TestObj = SingletonSettingsHandler;
				Assert.Equal("lang_english", SingletonSettingsHandler.CurrentLang.Tag);
				Assert.True(
					testDictionary.TriggerAddCalled_lang_english
					&& testDictionary.TriggerTryGetValueCalled_lang
					&& testDictionary.TriggerRemoveCalled_lang);
			});
		}

		[Fact]
		public void LanguageTest2()
		{
			BeforeAfterTest(() =>
			{
				Mocks.TestDictionary keyValuePairs = new Mocks.TestDictionary();
				PropertiesInjection(keyValuePairs);
				TestObj = SingletonSettingsHandler;
				Assert.Equal("lang_english", SingletonSettingsHandler.CurrentLang.Tag);
				Assert.True(
					keyValuePairs.TriggerAddCalled_lang_english
					&& keyValuePairs.TriggerTryGetValueCalled_lang);
			});
		}

		[Fact]
		public void UseDifferentLibs()
		{
			BeforeAfterTest(() =>
			{
				var called = false;
				TestObj = SingletonSettingsHandler;
				TestObj.AudioModuleChanged += (object source, EventArgs args) => { called = true; };
				TestObj.ChangeAudioModuleToSpotify();
				Assert.True(called);
				Assert.True(SingletonAudioLib.Playlists == null);
				Assert.True(SingletonAudioLib.SelectedPlaylist == null);
			});
		}

		[Fact]
		public void UseDifferentLibs2()
		{
			BeforeAfterTest(() =>
			{
				TestObj = SingletonSettingsHandler;
				TestObj.ChangeAudioModuleToSpotify();
				Assert.ThrowsAsync<NotImplementedException>(async () => await SingletonAudioLib.AddTrack("TEST", "TEST", "TEST", 0));
				Assert.ThrowsAsync<NotImplementedException>(
					async () => await SingletonAudioLib.DeleteTrack(
						new SpotifyAudioTrack(0.0, "TEST", "TEST", 0, "TEST", null)));
			});
		}

		[Fact]
		public void UseDifferentLibs3()
		{
			BeforeAfterTest(() =>
			{
				var called = 0;
				TestObj = SingletonSettingsHandler;
				TestObj.AudioModuleChanged += (object source, EventArgs args) =>
				{
					called++;
				};
				TestObj.ChangeAudioModuleToSpotify();
				TestObj.ChangeAudioModuleToBasic();
				Assert.Throws<NotImplementedException>(() => SingletonAudioLib.Playlists);
				Assert.Throws<NotImplementedException>(() => SingletonAudioLib.SelectedPlaylist);
				Assert.True(called == 2);
			});
		}

		[Fact]
		public void StepsPropertyTest()
		{
			BeforeAfterTest(() =>
			{
				Mocks.TestDictionary keyValuePairs = new Mocks.TestDictionary();
				keyValuePairs.Add("steps", 1);
				PropertiesInjection(keyValuePairs);
				TestObj = SingletonSettingsHandler;
				TestObj.ResetSteps();
				Assert.True(keyValuePairs.TriggerStepsTestCase1);
				Assert.True(SingletonSettingsHandler.Steps == 0);
			});
		}

		[Fact]
		public void ColorPropertyTest()
		{
			BeforeAfterTest(() =>
			{
				Mocks.TestDictionary keyValuePairs = new Mocks.TestDictionary();
				keyValuePairs.Add("color", Xamarin.Forms.Color.Blue.ToHex());
				PropertiesInjection(keyValuePairs);
				TestObj = SingletonSettingsHandler;
				TestObj.ResetSteps();
				Assert.True(keyValuePairs.TriggerColorTestCase1);
				TestObj.CurrentColor = new CustomColor(Xamarin.Forms.Color.Transparent);
				Assert.True(keyValuePairs.TriggerColorTestCase2);
			});
		}

		[Fact]
		public void SetLangTest()
		{
			BeforeAfterTest(() =>
			{
				Mocks.TestDictionary keyValuePairs = new Mocks.TestDictionary();
				PropertiesInjection(keyValuePairs);
				TestObj = SingletonSettingsHandler;
				TestObj.CurrentLang = new Lang(new System.IO.FileInfo("lang_test.lang"));
				Assert.True(keyValuePairs.TriggerLangTestCase2);
			});
		}
		
	}
}
