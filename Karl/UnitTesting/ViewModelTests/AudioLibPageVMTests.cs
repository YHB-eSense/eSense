using Karl.Model;
using Karl.ViewModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xunit;
using Xunit.Sdk;

namespace UnitTesting.ViewModelTests
{
	public class AudioLibPageVMTests
	{
		[Fact]
		[TestBeforeAfter]
		public void TitleSortCommandTest()
		{
			AudioLibPageVM_NEW vm = new AudioLibPageVM_NEW();
			vm.Songs.Add(new BasicAudioTrack_NEW("title1", "artist1"));
			vm.Songs.Add(new BasicAudioTrack_NEW("title2", "artist2"));
			vm.TitleSortCommand.Execute(null);
			Assert.Equal(vm.CurrentColor.Color, vm.TitleSortColor);
			Assert.Equal(Color.Transparent, vm.ArtistSortColor);
			Assert.Equal(Color.Transparent, vm.BPMSortColor);
			Assert.Equal(Color.White, vm.TitleSortTextColor);
			Assert.Equal(Color.Black, vm.ArtistSortTextColor);
			Assert.Equal(Color.Black, vm.BPMSortTextColor);
			//assert correct order TODO
		}

		[Fact]
		[TestBeforeAfter]
		public void ArtistSortCommandTest()
		{
			AudioLibPageVM_NEW vm = new AudioLibPageVM_NEW();
			vm.Songs.Add(new BasicAudioTrack_NEW("title1", "artist1"));
			vm.Songs.Add(new BasicAudioTrack_NEW("title2", "artist2"));
			vm.ArtistSortCommand.Execute(null);
			Assert.Equal(Color.Transparent, vm.TitleSortColor);
			Assert.Equal(vm.CurrentColor.Color, vm.ArtistSortColor);
			Assert.Equal(Color.Transparent, vm.BPMSortColor);
			Assert.Equal(Color.Black, vm.TitleSortTextColor);
			Assert.Equal(Color.White, vm.ArtistSortTextColor);
			Assert.Equal(Color.Black, vm.BPMSortTextColor);
			//assert correct order TODO
		}

		[Fact]
		[TestBeforeAfter]
		public void BPMSortCommandTest()
		{
			AudioLibPageVM vm = new AudioLibPageVM_NEW();
			vm.Songs.Add(new BasicAudioTrack_NEW("title1", "artist1"));
			vm.Songs.Add(new BasicAudioTrack_NEW("title2", "artist2"));
			vm.BPMSortCommand.Execute(null);
			Assert.Equal(Color.Transparent, vm.TitleSortColor);
			Assert.Equal(Color.Transparent, vm.ArtistSortColor);
			Assert.Equal(vm.CurrentColor.Color, vm.BPMSortColor);
			Assert.Equal(Color.Black, vm.TitleSortTextColor);
			Assert.Equal(Color.Black, vm.ArtistSortTextColor);
			Assert.Equal(Color.White, vm.BPMSortTextColor);
			//assert correct order TODO
		}

		[Fact]
		[TestBeforeAfter]
		public void PlaySongCommandTest() { }

		[Fact]
		[TestBeforeAfter]
		public void AddSongCommandTest() { }

		[Fact]
		[TestBeforeAfter]
		public void SearchSongCommandTest()
		{
			AudioLibPageVM_NEW vm = new AudioLibPageVM_NEW();
			vm.Songs.Add(new BasicAudioTrack_NEW("title1", "artist1"));
			vm.Songs.Add(new BasicAudioTrack_NEW("title2", "artist2"));
			vm.SearchSongCommand.Execute("title1");
			foreach (AudioTrack track in vm.Songs)
			{
				Assert.Contains("title1", track.Title);
			}
		}

		[Fact]
		[TestBeforeAfter]
		public void DeleteSongCommandTest() { }

		[Fact]
		[TestBeforeAfter]
		public void EditDeleteListCommandTest() { }

		internal class AudioLibPageVM_NEW : AudioLibPageVM
		{
			public AudioLibPageVM_NEW()
			{
				Songs = new List<AudioTrack>();
			}
			public override CustomColor CurrentColor { get => new CustomColor(Color.Red); }
			public override List<AudioTrack> Songs { get; set; }
			protected override void InitializeSingletons() { }
#pragma warning disable CS1998 // In dieser Async-Methode fehlen die "await"-Operatoren, weshalb sie synchron ausgeführt wird. Sie sollten die Verwendung des "await"-Operators oder von "await Task.Run(...)" in Betracht ziehen, um auf nicht blockierende API-Aufrufe zu warten bzw. CPU-gebundene Aufgaben auf einem Hintergrundthread auszuführen.
			protected override async Task<bool> AlertWrapper()
#pragma warning restore CS1998 // In dieser Async-Methode fehlen die "await"-Operatoren, weshalb sie synchron ausgeführt wird. Sie sollten die Verwendung des "await"-Operators oder von "await Task.Run(...)" in Betracht ziehen, um auf nicht blockierende API-Aufrufe zu warten bzw. CPU-gebundene Aufgaben auf einem Hintergrundthread auszuführen.
			{
				return true;
			}
		}

		internal class BasicAudioTrack_NEW : BasicAudioTrack
		{
			public BasicAudioTrack_NEW(string title, string artist)
			{
				Title = title;
				Artist = artist;
			}
		}

		[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
		internal class TestBeforeAfter : BeforeAfterTestAttribute
		{

			public override void Before(MethodInfo methodUnderTest)
			{
				Debug.WriteLine(methodUnderTest.Name);
			}

			public override void After(MethodInfo methodUnderTest)
			{
				Debug.WriteLine(methodUnderTest.Name);
			}
		}
	}
}
