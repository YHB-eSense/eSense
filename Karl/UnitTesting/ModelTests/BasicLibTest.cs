using Karl.Data;
using Moq;
using System;
using System.Diagnostics;
using System.Reflection;
using Xunit;
using Xunit.Sdk;
using Karl.Model;
using SQLite;
using System.Threading;

namespace UnitTesting.ModelTests
{
	public class BasicLibTest
	{
		AudioLib TestObj;
		System.Reflection.PropertyInfo _dbInstance;
		System.Reflection.FieldInfo _singeltonDbInstance;
		System.Reflection.FieldInfo _singeltonAlInstance;

		void Before()
		{
			_dbInstance = typeof(BasicAudioTrackDatabase).GetProperty("_database", BindingFlags.NonPublic | BindingFlags.Instance);
			_singeltonAlInstance = typeof(AudioLib).GetField("_singletonAudioLib", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);
			_singeltonAlInstance.SetValue(null, null);
			_singeltonDbInstance = typeof(BasicAudioTrackDatabase).GetField("_singletonDatabase", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);
			_singeltonDbInstance.SetValue(null, null);
			TestObj = AudioLib.SingletonAudioLib;
			BasicAudioTrackDatabase database = BasicAudioTrackDatabase.SingletonDatabase;
			Mock<SQLiteAsyncConnection> mockDatabase = new Mock<SQLiteAsyncConnection>("", true);
			_dbInstance.SetValue(database, mockDatabase.Object);
			BasicAudioTrack.Testing(true);
		}

		void After()
		{
			_singeltonDbInstance.SetValue(null, null);
			_singeltonAlInstance.SetValue(null, null);
		}

		[Fact]
		public void AddSongTest()
		{
			BeforeAfterTest(Add_Clear_Song);
		}

		void Add_Clear_Song()
		{
			new Thread(async () =>
			{
				await TestObj.AddTrack("testUrl", "Fire and Forgive", "Powerwolf", 140);
				await TestObj.AddTrack("testUrl2", "Incense and Iron", "Powerwolf", 140);
				await TestObj.AddTrack("testUrl4", "Sacrament of Sin", "Powerwolf", 140);
				await TestObj.AddTrack("testUrl3", "Resurrection by Erection", "Powerwolf", 140);
				await TestObj.AddTrack("testUrl5", "Amen and Attack", "Powerwolf", 140);
				await TestObj.AddTrack("testUrl6", "Armata Strigoi", "Powerwolf", 140);
				await TestObj.AddTrack("testUrl7", "Nightside of Siberia", "Powerwolf", 140);
				Assert.True(TestObj.AudioTracks.Count == 7);
				TestObj.AudioTracks.Clear();
				Assert.Empty(TestObj.AudioTracks);
			}).Start();

		}

		void BeforeAfterTest(Action TestCase)
		{
			Before();
			TestCase.Invoke();
			After();
		}
		
	}
}
