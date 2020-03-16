#define TESTING
using Karl.Model;
using Karl.View;
using Karl.ViewModel;
using Moq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xunit;

namespace UnitTesting.ViewModelTests
{
	public class AudioLibPageVMTests
	{
		[Fact]
		public void TitleSortCommandTest()
		{
			new Thread(() =>
			{
				//setup
				SettingsHandler.Testing(true);
				var mockObj = new Mock<IDictionary<string, Object>>();
				SettingsHandler.PropertiesInjection(mockObj.Object);
				var vm = new AudioLibPageVM_NEW();
				int i = 0;
				vm.PropertyChanged += (sender, e) => i++;
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
				Assert.Equal(6, i);
			}).Start();
		}

		[Fact]
		public void ArtistSortCommandTest()
		{
			new Thread(() =>
			{
				//setup
				SettingsHandler.Testing(true);
				var mockObj = new Mock<IDictionary<string, Object>>();
				SettingsHandler.PropertiesInjection(mockObj.Object);
				var vm = new AudioLibPageVM_NEW();
				int i = 0;
				vm.PropertyChanged += (sender, e) => i++;
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
				Assert.Equal(6, i);
			}).Start();
		}

		[Fact]
		public void BPMSortCommandTest()
		{
			new Thread(() =>
			{
				Thread.CurrentThread.IsBackground = true;
				//setup
				SettingsHandler.Testing(true);
				var mockObj = new Mock<IDictionary<string, Object>>();
				SettingsHandler.PropertiesInjection(mockObj.Object);
				var vm = new AudioLibPageVM_NEW();
				int i = 0;
				vm.PropertyChanged += (sender, e) => i++;
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
				Assert.Equal(6, i);
			}).Start();
		}

		[Fact]
		public void PlaySongCommandTest()
		{
			//setup
			SettingsHandler.Testing(true);
			var mockObj = new Mock<IDictionary<string, Object>>();
			SettingsHandler.PropertiesInjection(mockObj.Object);
			var vm = new AudioLibPageVM_NEW();
			var track1 = new AudioTrack_NEW("title1", "artist1", 1);
			//test
			vm.PlaySongCommand.Execute(track1);
			Assert.Equal(track1, vm.Player.CurrentTrack);
		}

		[Fact]
		public void AddSongCommandTest()
		{
			//setup
			SettingsHandler.Testing(true);
			var mockObj = new Mock<IDictionary<string, Object>>();
			SettingsHandler.PropertiesInjection(mockObj.Object);
			var vm = new AudioLibPageVM_NEW();
			//test
			vm.AddSongCommand.Execute(null);
			Assert.Equal(typeof(AddSongPage), vm.NavHandler.CurrentPageType);
		}

		[Fact]
		public void SearchSongCommandTest()
		{
			//setup
			SettingsHandler.Testing(true);
			var mockObj = new Mock<IDictionary<string, Object>>();
			SettingsHandler.PropertiesInjection(mockObj.Object);
			var vm = new AudioLibPageVM_NEW();
			vm.Songs.Add(new AudioTrack_NEW("title1", "artist1", 1));
			vm.Songs.Add(new AudioTrack_NEW("title2", "artist2", 2));
			var oldSongs = new List<AudioTrack>(vm.Songs);
			//test
			vm.SearchSongCommand.Execute("title1");
			foreach (AudioTrack track in vm.Songs)
			{
				Assert.Contains("title1", track.Title);
			}
			Assert.NotEqual(oldSongs, vm.Songs);
			//test
			vm.SearchSongCommand.Execute("");
			Assert.Equal(oldSongs, vm.Songs);
			//test
			vm.SearchSongCommand.Execute(null);
			Assert.Equal(oldSongs, vm.Songs);
		}

		[Fact]
		public void DeleteSongsCommandTest()
		{
			//setup
			SettingsHandler.Testing(true);
			var mockObj = new Mock<IDictionary<string, Object>>();
			SettingsHandler.PropertiesInjection(mockObj.Object);
			var vm = new AudioLibPageVM_NEW();
			var track1 = new AudioTrack_NEW("title1", "artist1", 1);
			vm.EditDeleteListCommand.Execute(track1);
			vm.Player.CurrentTrack = track1;
			//test
			vm.DeleteSongsCommand.Execute(null);
			Assert.Empty(vm.DeleteList);
			Assert.Null(vm.Player.CurrentTrack);
		}

		[Fact]
		public void EditDeleteListCommandTest()
		{
			//setup
			SettingsHandler.Testing(true);
			var mockObj = new Mock<IDictionary<string, Object>>();
			SettingsHandler.PropertiesInjection(mockObj.Object);
			var vm = new AudioLibPageVM_NEW();
			var track1 = new AudioTrack_NEW("title1", "artist1", 1);
			//test
			vm.EditDeleteListCommand.Execute(track1);
			Assert.Equal(track1, vm.DeleteList[0]);
			//test
			vm.EditDeleteListCommand.Execute(track1);
			Assert.Empty(vm.DeleteList);
		}

		[Fact]
		public void RefreshTests()
		{
			new Thread(() =>
			{
				//setup
				SettingsHandler.Testing(true);
				var mockObj = new Mock<IDictionary<string, Object>>();
				SettingsHandler.PropertiesInjection(mockObj.Object);
				var vm = new AudioLibPageVM_NEW();
				int i = 0;
				vm.PropertyChanged += (sender, e) => i++;
				//test
				SettingsHandler.SingletonSettingsHandler.CurrentLang = SettingsHandler.SingletonSettingsHandler.Languages[0];
				Assert.Equal(4, i);
				i = 0;
				//test
				SettingsHandler.SingletonSettingsHandler.CurrentColor = SettingsHandler.SingletonSettingsHandler.Colors[0];
				Assert.Equal(7, i);
				i = 0;
				//test
				SettingsHandler.SingletonSettingsHandler.ChangeAudioModuleToBasic();
				Assert.Equal(11, i);
				i = 0;
				/*
				//test
				FieldInfo field = typeof(AudioLib).GetField("_audioLibImp", BindingFlags.NonPublic | BindingFlags.Instance);
				var output = field.GetValue(AudioLib.SingletonAudioLib);
				IAudioLibImpl audioLibImp = (IAudioLibImpl) output;
				audioLibImp.AddTrack("test", "title", "artist", 0);
				Assert.Equal(7, i);
				*/
			}).Start();
		}

		internal class AudioLibPageVM_NEW : AudioLibPageVM
		{
			public NavigationHandler NavHandler { get => _navHandler; }
			public List<AudioTrack> DeleteList { get => _deleteList; }
			public AudioPlayer Player { get => _audioPlayer; }
			public AudioLibPageVM_NEW() : base()
			{
				Songs = new List<AudioTrack>();
			}
			public override CustomColor CurrentColor { get => new CustomColor(Color.Red); }
			public override List<AudioTrack> Songs { get; set; }
			protected override void InitializeSingletons()
			{
				_audioPlayer = new AudioPlayer_NEW();
				_navHandler = new NavigationHandler_NEW();
				_audioLib = new AudioLib_NEW();
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

		internal class AudioLib_NEW : AudioLib
		{
			public override async Task DeleteTrack(AudioTrack track) { }
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
			public override Type CurrentPageType { get; set; }
			public override async void GotoPage<T>() { CurrentPageType = typeof(T); }
		}

	}
}
