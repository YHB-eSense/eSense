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
			var vm = new AudioLibPageVM_NEW();
			var track1 = new AudioTrack_NEW("title1", "artist1", 1);
			var track2 = new AudioTrack_NEW("title2", "artist2", 2);
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
			var vm = new AudioLibPageVM_NEW();
			var track1 = new AudioTrack_NEW("title1", "artist1", 1);
			var track2 = new AudioTrack_NEW("title2", "artist2", 2);
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
			var vm = new AudioLibPageVM_NEW();
			var track1 = new AudioTrack_NEW("title1", "artist1", 1);
			var track2 = new AudioTrack_NEW("title2", "artist2", 2);
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
		public void PlaySongCommandTest()
		{
			//setup
			var vm = new AudioLibPageVM_NEW();
			var track1 = new AudioTrack_NEW("title1", "artist1", 1);
			//test
			vm.PlaySongCommand.Execute(track1);
			Assert.Equal(track1, vm.Player.CurrentTrack);
		}

		[Fact]
		public void AddSongCommandTest() { }

		[Fact]
		public void SearchSongCommandTest()
		{
			//setup
			var vm = new AudioLibPageVM_NEW();
			vm.Songs.Add(new AudioTrack_NEW("title1", "artist1", 1));
			vm.Songs.Add(new AudioTrack_NEW("title2", "artist2", 2));
			//test
			vm.SearchSongCommand.Execute("title1");
			foreach (AudioTrack track in vm.Songs)
			{
				Assert.Contains("title1", track.Title);
			}
		}

		[Fact]
		public void DeleteSongCommandTest()
		{
			/*
			//setup
			var vm = new AudioLibPageVM_NEW();
			var track1 = new AudioTrack_NEW("title1", "artist1", 1);
			vm.EditDeleteListCommand.Execute(track1);
			//test
			vm.DeleteSongsCommand.Execute(null);
			Assert.Empty(vm.DeleteList);
			*/ //TODO
		}

		[Fact]
		public void EditDeleteListCommandTest()
		{
			//setup
			var vm = new AudioLibPageVM_NEW();
			var track1 = new AudioTrack_NEW("title1", "artist1", 1);
			//test
			vm.EditDeleteListCommand.Execute(track1);
			Assert.Equal(track1, vm.DeleteList[0]);
		}

		internal class AudioLibPageVM_NEW : AudioLibPageVM
		{
			public List<AudioTrack> DeleteList { get => _deleteList; }
			public AudioPlayer Player { get => _audioPlayer; }
			public AudioLibPageVM_NEW()
			{
				Songs = new List<AudioTrack>();
			}
			public override CustomColor CurrentColor { get => new CustomColor(Color.Red); }
			public override List<AudioTrack> Songs { get; set; }
			protected override void InitializeSingletons()
			{
				_audioPlayer = new AudioPlayer_NEW();
				_navHandler = new NavigationHandler_NEW();
			}
			protected override async Task<bool> AlertWrapper() { return true; }
		}

		internal class AudioPlayer_NEW : AudioPlayer
		{
			public override AudioTrack CurrentTrack { get; set; }
			public override void PlayTrack(AudioTrack track)
			{
				CurrentTrack = track;
			}
		}

		internal class AudioTrack_NEW : AudioTrack
		{
			public AudioTrack_NEW(string title, string artist, int bpm)
			{
				Title = title;
				Artist = artist;
				BPM = bpm;
			}
			public override double Duration { get; set; }
			public override byte[] Cover { get; set; }
			public override string Title { get; set; }
			public override string Artist { get; set; }
			public override int BPM { get; set; }
			public override string StorageLocation { get; set; }
			public override string TextId { get; set; }
		}

		internal class NavigationHandler_NEW : NavigationHandler
		{
			public override async void GotoPage<T>() { }
		}
	}
}
