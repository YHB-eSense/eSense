using Karl.Model;
using Karl.ViewModel;
using Moq;
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
		public void TitleSortCommandTest()
		{
			//setup
			AudioLibPageVM_NEW vm = new AudioLibPageVM_NEW();
			BasicAudioTrack_NEW track1 = new BasicAudioTrack_NEW("title1", "artist1", 1);
			BasicAudioTrack_NEW track2 = new BasicAudioTrack_NEW("title2", "artist2", 2);
			vm.Songs.Add(track1);
			vm.Songs.Add(track2);
			//test
			vm.TitleSortCommand.Execute(null);
			Assert.Equal(vm.CurrentColor.Color, vm.TitleSortColor);
			Assert.Equal(Color.Transparent, vm.ArtistSortColor);
			Assert.Equal(Color.Transparent, vm.BPMSortColor);
			Assert.Equal(Color.White, vm.TitleSortTextColor);
			Assert.Equal(Color.Black, vm.ArtistSortTextColor);
			Assert.Equal(Color.Black, vm.BPMSortTextColor);
			Assert.Equal(vm.Songs[0], track1);
			Assert.Equal(vm.Songs[1], track2);
		}

		[Fact]
		public void ArtistSortCommandTest()
		{
			//setup
			AudioLibPageVM_NEW vm = new AudioLibPageVM_NEW();
			BasicAudioTrack_NEW track1 = new BasicAudioTrack_NEW("title1", "artist1", 1);
			BasicAudioTrack_NEW track2 = new BasicAudioTrack_NEW("title2", "artist2", 2);
			vm.Songs.Add(track1);
			vm.Songs.Add(track2);
			//test
			vm.ArtistSortCommand.Execute(null);
			Assert.Equal(Color.Transparent, vm.TitleSortColor);
			Assert.Equal(vm.CurrentColor.Color, vm.ArtistSortColor);
			Assert.Equal(Color.Transparent, vm.BPMSortColor);
			Assert.Equal(Color.Black, vm.TitleSortTextColor);
			Assert.Equal(Color.White, vm.ArtistSortTextColor);
			Assert.Equal(Color.Black, vm.BPMSortTextColor);
			Assert.Equal(vm.Songs[0], track1);
			Assert.Equal(vm.Songs[1], track2);
		}

		[Fact]
		public void BPMSortCommandTest()
		{
			//setup
			AudioLibPageVM vm = new AudioLibPageVM_NEW();
			BasicAudioTrack_NEW track1 = new BasicAudioTrack_NEW("title1", "artist1", 1);
			BasicAudioTrack_NEW track2 = new BasicAudioTrack_NEW("title2", "artist2", 2);
			vm.Songs.Add(track1);
			vm.Songs.Add(track2);
			//test
			vm.BPMSortCommand.Execute(null);
			Assert.Equal(Color.Transparent, vm.TitleSortColor);
			Assert.Equal(Color.Transparent, vm.ArtistSortColor);
			Assert.Equal(vm.CurrentColor.Color, vm.BPMSortColor);
			Assert.Equal(Color.Black, vm.TitleSortTextColor);
			Assert.Equal(Color.Black, vm.ArtistSortTextColor);
			Assert.Equal(Color.White, vm.BPMSortTextColor);
			Assert.Equal(vm.Songs[0], track1);
			Assert.Equal(vm.Songs[1], track2);
		}

		[Fact]
		public void PlaySongCommandTest() { }

		[Fact]
		public void AddSongCommandTest() { }

		[Fact]
		public void SearchSongCommandTest()
		{
			//setup
			AudioLibPageVM_NEW vm = new AudioLibPageVM_NEW();
			vm.Songs.Add(new BasicAudioTrack_NEW("title1", "artist1", 1));
			vm.Songs.Add(new BasicAudioTrack_NEW("title2", "artist2", 2));
			//test
			vm.SearchSongCommand.Execute("title1");
			foreach (AudioTrack track in vm.Songs)
			{
				Assert.Contains("title1", track.Title);
			}
		}

		[Fact]
		public void DeleteSongCommandTest() { }

		[Fact]
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
			protected override async Task<bool> AlertWrapper() { return true; }
		}

		internal class BasicAudioTrack_NEW : BasicAudioTrack
		{
			public BasicAudioTrack_NEW(string title, string artist, int bpm)
			{
				Title = title;
				Artist = artist;
				BPM = bpm;
			}
		}
	}
}
