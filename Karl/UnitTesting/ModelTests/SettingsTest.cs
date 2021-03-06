#define TESTING
using Moq;
using System;
using Xunit;
using Karl.Model;
using System.Collections.Generic;
using static Karl.Model.SettingsHandler;
using static Karl.Model.AudioLib;
using Karl.Data;
using System.Reflection;
using SpotifyAPI.Web.Models;
using System.Threading;

namespace UnitTesting.ModelTests
{
	public class SettingsTest
	{
		SettingsHandler TestObj;
		AudioLib TestObj2;
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
			BasicAudioTrack.Testing(true);
			fw.MockDatabase();
			mockObj = new Mock<IDictionary<string, Object>>();
			PropertiesInjection(mockObj.Object);
			fw.ResetSingletons();
		}

		void After()
		{
			SpotifyAudioLib.Testing(false);
			BasicAudioTrackDatabase.Testing(false);
			SettingsHandler.Testing(false);
			BasicAudioTrack.Testing(false);
			fw.ResetSingletons();
			PropertiesInjection(null);
		}

		[Fact]
		public void ColorTest()
		{
			BeforeAfterTest(() =>
			{
				new Thread(() =>
				{
					TestObj = SingletonSettingsHandler;
					mockObj.Setup(m => m.TryGetValue("color", out val)).Returns(false);
					mockObj.Verify(mock => mock.Add("color", "#FF4169E1"));
				}).Start();
			});
		}

		[Fact]
		public void LanguageTest()
		{
			BeforeAfterTest(() =>
			{
				new Thread(() =>
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
				}).Start();
			});
		}

		[Fact]
		public void LanguageTestNoLangInProperties()
		{
			BeforeAfterTest(() =>
			{
				new Thread(() =>
				{
					Mocks.TestDictionary keyValuePairs = new Mocks.TestDictionary();
					PropertiesInjection(keyValuePairs);
					TestObj = SingletonSettingsHandler;
					Assert.Equal("lang_english", SingletonSettingsHandler.CurrentLang.Tag);
					Assert.True(
					keyValuePairs.TriggerAddCalled_lang_english
					&& keyValuePairs.TriggerTryGetValueCalled_lang);
				}).Start();
			});
		}

		[Fact]
		public void UseDifferentLibs()
		{
			BeforeAfterTest(() =>
			{
				new Thread(() =>
				{
					var called = false;
					TestObj = SingletonSettingsHandler;
					TestObj.AudioModuleChanged += (object source, EventArgs args) => { called = true; };
					TestObj.ChangeAudioModuleToSpotify();
					Assert.True(called);
					Assert.True(SingletonAudioLib.Playlists == null);
					Assert.True(SingletonAudioLib.SelectedPlaylist == null);
				}).Start();
			});
		}

		[Fact]
		public void UseDifferentLibsAddThrowsException()
		{
			BeforeAfterTest(() =>
			{
				new Thread(() =>
				{
					TestObj = SingletonSettingsHandler;
					TestObj.ChangeAudioModuleToSpotify();
					Assert.ThrowsAsync<NotImplementedException>(async () => await SingletonAudioLib.AddTrack("TEST", "TEST", "TEST", 0));
					Assert.ThrowsAsync<NotImplementedException>(
						async () => await SingletonAudioLib.DeleteTrack(
							new SpotifyAudioTrack(0.0, "TEST", "TEST", 0, "TEST", null)));
				}).Start();
			});
		}

		[Fact]
		public void SwitchLibsTest()
		{
			BeforeAfterTest(() =>
			{
				new Thread(() =>
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
				}).Start();
			});
		}

		[Fact]
		public void StepsPropertyTest()
		{
			BeforeAfterTest(() =>
			{
				new Thread(() =>
				{
					Mocks.TestDictionary keyValuePairs = new Mocks.TestDictionary();
					keyValuePairs.Add("steps", 1);
					PropertiesInjection(keyValuePairs);
					TestObj = SingletonSettingsHandler;
					TestObj.ResetSteps();
					Assert.True(keyValuePairs.TriggerStepsTestCase1);
					Assert.True(SingletonSettingsHandler.Steps == 0);
				}).Start();
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

		[Fact]
		public void BasicAudioLibAddSongTest()
		{
			BeforeAfterTest(() =>
			{
				new Thread(async () =>
				{
					TestObj2 = SingletonAudioLib;
					await TestObj2.AddTrack("", "Fire and Forgive", "Powerwolf", 140);
					await TestObj2.AddTrack("", "Incense and Iron", "Powerwolf", 140);
					await TestObj2.AddTrack("", "Sacrament of Sin", "Powerwolf", 140);
					await TestObj2.AddTrack("", "Resurrection by Erection", "Powerwolf", 140);
					await TestObj2.AddTrack("", "Amen and Attack", "Powerwolf", 140);
					await TestObj2.AddTrack("", "Armata Strigoi", "Powerwolf", 140);
					await TestObj2.AddTrack("", "Nightside of Siberia", "Powerwolf", 140);
					Assert.True(TestObj2.AudioTracks.Count == 7);
					await TestObj2.DeleteTrack(TestObj2.AudioTracks[0]);
					Assert.True(TestObj2.AudioTracks.Count == 6);
					TestObj2.AudioTracks.Clear();
					Assert.Empty(TestObj2.AudioTracks);
				}).Start();
			});
		}

		[Fact]
		public void BasicLibAddSongNegativeBPMTest()
		{
			BeforeAfterTest(async () =>
			{
				TestObj2 = SingletonAudioLib;
				await Assert.ThrowsAsync<ArgumentException>(async () => await TestObj2.AddTrack("", "Fire and Forgive", "Powerwolf", -1));
			});
		}

		[Fact]
		public void DeleteTrackNonexistant()
		{
			BeforeAfterTest(async () =>
			{
				TestObj2 = SingletonAudioLib;
				await Assert.ThrowsAsync<ArgumentException>(
					 async () => await TestObj2.DeleteTrack(new BasicAudioTrack("", "The Devil in I", "Slipknot", 1)));
			});
		}

		[Fact]
		public void PlaylistBasicLibExceptionTest()
		{
			BeforeAfterTest(() =>
			{
				TestObj2 = SingletonAudioLib;
				Assert.Throws<NotImplementedException>(
					() => TestObj2.SelectedPlaylist = new SimplePlaylist());
			});
		}

		[Fact]
		public void SpotifyNonexistantPlaylistTest()
		{
			BeforeAfterTest(() =>
			{
				new Thread(() =>
				{
					TestObj2 = SingletonAudioLib;
					TestObj2.ChangeToSpotifyLib();
					FieldInfo _audioLibImplField = typeof(AudioLib).GetField("_audioLibImp", BindingFlags.Instance | BindingFlags.NonPublic);
					SpotifyAudioLib instance = (SpotifyAudioLib)_audioLibImplField.GetValue(TestObj2);
					instance.AllPlaylists = new SimplePlaylist[1];
					instance.AllPlaylists[0] = new SimplePlaylist();
					Assert.Throws<ArgumentException>(() =>
					{
						TestObj2.SelectedPlaylist = new SimplePlaylist();
					});
				}).Start();
			});
		}

		[Fact]
		public void SpotifyNotImplementedExceptions()
		{
			BeforeAfterTest(async () =>
			{
				TestObj2 = SingletonAudioLib;
				TestObj2.ChangeToSpotifyLib();
				await Assert.ThrowsAsync<NotImplementedException>(async () =>
				{
					await TestObj2.AddTrack("", "Neuer Alter Savas", "DCVDNS", 1);
				});
				await Assert.ThrowsAsync<NotImplementedException>(async () =>
				{
					await TestObj2.DeleteTrack(new BasicAudioTrack("", "Neuer Alter Savas", "DCVDNS", 1));
				});
			});
		}

	}
}
