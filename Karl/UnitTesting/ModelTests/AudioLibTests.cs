#define TESTING
using System;
using System.Reflection;
using SpotifyAPI.Web.Models;
using Karl.Model;
using Karl.Data;
using static Karl.Model.AudioLib;
using Xunit;

namespace UnitTesting.ModelTests
{
	public class AudioLibTests
	{
		AudioLib TestObj;
		TestFramework fw = new TestFramework();
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
			fw.ResetSingletons();
			fw.MockDatabase();
		}

		void After()
		{
			SpotifyAudioLib.Testing(false);
			BasicAudioTrackDatabase.Testing(false);
			SettingsHandler.Testing(false);
			BasicAudioTrack.Testing(false);
			fw.ResetSingletons();
		}

		[Fact]
		public void BasicAudioLibAddSongTest()
		{
			BeforeAfterTest(async () =>
			{
				TestObj = SingletonAudioLib;
				await TestObj.AddTrack("", "Fire and Forgive", "Powerwolf", 140);
				await TestObj.AddTrack("", "Incense and Iron", "Powerwolf", 140);
				await TestObj.AddTrack("", "Sacrament of Sin", "Powerwolf", 140);
				await TestObj.AddTrack("", "Resurrection by Erection", "Powerwolf", 140);
				await TestObj.AddTrack("", "Amen and Attack", "Powerwolf", 140);
				await TestObj.AddTrack("", "Armata Strigoi", "Powerwolf", 140);
				await TestObj.AddTrack("", "Nightside of Siberia", "Powerwolf", 140);
				Assert.True(TestObj.AudioTracks.Count == 7);
				TestObj.DeleteTrack(TestObj.AudioTracks[0]);
				Assert.True(TestObj.AudioTracks.Count == 6);
				TestObj.AudioTracks.Clear();
				Assert.Empty(TestObj.AudioTracks);
			});
		}

		[Fact]
		public void BasicLibAddSongNegativeBPMTest()
		{
			BeforeAfterTest(() =>
			{
				TestObj = SingletonAudioLib;
				Assert.ThrowsAsync<ArgumentException>(async () => await TestObj.AddTrack("", "Fire and Forgive", "Powerwolf", -1));
			});
		}

		[Fact]
		public void DeleteTrackNonexistant()
		{
			BeforeAfterTest(() =>
			{
				TestObj = SingletonAudioLib;
				Assert.ThrowsAsync<ArgumentException>(
					 async () => TestObj.DeleteTrack(new BasicAudioTrack("", "The Devil in I", "Slipknot", 1)));
			});
		}

		[Fact]
		public void PlaylistBasicLibExceptionTest()
		{
			BeforeAfterTest(() =>
			{
				TestObj = SingletonAudioLib;
				Assert.Throws<NotImplementedException>(
					() => TestObj.SelectedPlaylist = new SimplePlaylist());
			});
		}

		[Fact]
		public void SpotifyNonexistantPlaylistTest()
		{
			BeforeAfterTest(() =>
			{
				TestObj = SingletonAudioLib;
				TestObj.ChangeToSpotifyLib();
				FieldInfo _audioLibImplField = typeof(AudioLib).GetField("_audioLibImpl", BindingFlags.Instance | BindingFlags.NonPublic);
				SpotifyAudioLib instance = (SpotifyAudioLib)_audioLibImplField.GetValue(TestObj);
				instance.AllPlaylists = new SimplePlaylist[1];
				instance.AllPlaylists[0] = new SimplePlaylist();
				Assert.Throws<ArgumentException>(() =>
				{
					TestObj.SelectedPlaylist = new SimplePlaylist();
				});
			});
		}

		[Fact]
		public void SpotifyNotImplementedExceptions()
		{
			BeforeAfterTest(() =>
			{
				TestObj = SingletonAudioLib;
				TestObj.ChangeToSpotifyLib();
				Assert.ThrowsAsync<NotImplementedException>(async () =>
				{
					await TestObj.AddTrack("", "Neuer Alter Savas", "DCVDNS", 1);
				});
				Assert.Throws<NotImplementedException>(() =>
				{
					TestObj.DeleteTrack(new BasicAudioTrack("", "Neuer Alter Savas", "DCVDNS", 1));
				});
			});
		}
	}
}
