using Karl.Data;
using Moq;
using System;
using System.Diagnostics;
using System.Reflection;
using Xunit;
using Xunit.Sdk;
using Karl.Model;

namespace UnitTesting.ModelTests
{
	public class BasicLibTest
	{
		AudioLib TestObj;
		System.Reflection.FieldInfo _singeltonDbInstance;
		System.Reflection.FieldInfo _singeltonAlInstance;

		void Before()
		{
			TestObj = AudioLib.SingletonAudioLib;
			_singeltonDbInstance = typeof(BasicAudioTrackDatabase).GetField("_singletonDatabase", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);
			_singeltonAlInstance = typeof(AudioLib).GetField("_singletonAudioLib", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);

			Mock<BasicAudioTrackDatabase> mockSingleton = new Mock<BasicAudioTrackDatabase>();
			_singeltonDbInstance.SetValue(null, mockSingleton.Object);
			_singeltonAlInstance.SetValue(null, null);
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
			TestObj.AddTrack("testUrl", "Fire and Forgive", "Powerwolf", 140);
			TestObj.AddTrack("testUrl2", "Incense and Iron", "Powerwolf", 140);
			TestObj.AddTrack("testUrl4", "Sacrament of Sin", "Powerwolf", 140);
			TestObj.AddTrack("testUrl3", "Resurrection by Erection", "Powerwolf", 140);
			TestObj.AddTrack("testUrl5", "Amen and Attack", "Powerwolf", 140);
			TestObj.AddTrack("testUrl6", "Armata Strigoi", "Powerwolf", 140);
			TestObj.AddTrack("testUrl7", "Nightside of Siberia", "Powerwolf", 140);
			Assert.True(TestObj.AudioTracks.Count == 7);
			TestObj.AudioTracks.Clear();
			Assert.Empty(TestObj.AudioTracks);
		}

		void BeforeAfterTest(Action TestCase)
		{
			Before();
			TestCase.Invoke();
			After();
		}
		
	}
}
