using Karl.Model;
using Karl.ViewModel;
using System;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace UnitTesting.ViewModelTests
{
	public class AddSongPageVMTests
	{
		[Fact]
		public void PickFileCommandTest()
		{
			//setup
			//@"C:\Users\maxib\Desktop\BAB.wav"
			var vm = new AddSongPageVM_NEW(Path.Combine(Environment.CurrentDirectory, @"Data\BAB.wav"));
			SettingsHandler.SingletonSettingsHandler.CurrentLang = SettingsHandler.SingletonSettingsHandler.Languages[0];
			//test
			vm.PickFileCommand.Execute(null);
			Assert.Equal("Back In Black", vm.NewSongTitle);
			Assert.Equal("Unknown", vm.NewSongArtist);
			Assert.Equal("Unknown", vm.NewSongBPM);
		}

		[Theory]
		[InlineData("0")]
		public void GetBPMCommandTest(string val1)
		{
			//setup
			var vm = new AddSongPageVM_NEW(val1, Path.Combine(Environment.CurrentDirectory, @"Data\BAB.wav"));
			SettingsHandler.SingletonSettingsHandler.CurrentLang = SettingsHandler.SingletonSettingsHandler.Languages[0];
			//test
			vm.Alerted = false;
			vm.GetBPMCommand.Execute(null);
			Assert.True(vm.Alerted);
			//test
			vm.PickFileCommand.Execute(null);
			vm.GetBPMCommand.Execute(null);
			Assert.Equal(val1, vm.NewSongBPM);
			//test
			vm = new AddSongPageVM_NEW(Path.Combine(Environment.CurrentDirectory, @"Data\BAB.mp3"));
			vm.Alerted = false;
			vm.PickFileCommand.Execute(null);
			vm.GetBPMCommand.Execute(null);
			Assert.True(vm.Alerted);
		}

		[Theory]
		[InlineData("0")]
		public void AddSongCommandTest(string val1)
		{
			//setup
			var vm = new AddSongPageVM_NEW(val1, Path.Combine(Environment.CurrentDirectory, @"Data\BAB.wav"));
			SettingsHandler.SingletonSettingsHandler.CurrentLang = SettingsHandler.SingletonSettingsHandler.Languages[0];
			//test
			vm.AddSongCommand.Execute(null);
			Assert.True(vm.Alerted);
			//test
			vm.Alerted = false;
			vm.PickFileCommand.Execute(null);
			vm.GetBPMCommand.Execute(null);
			vm.AddSongCommand.Execute(null);
			System.Threading.Thread.Sleep(1000);
			Assert.Null(vm.NewSongTitle);
			Assert.Null(vm.NewSongArtist);
			Assert.Null(vm.NewSongBPM);
		}

		[Fact]
		public void RefreshTest()
		{
			//setup
			var vm = new AddSongPageVM_NEW();
			int i = 0;
			vm.PropertyChanged += (sender, e) => i++;
			//test
			SettingsHandler.SingletonSettingsHandler.CurrentLang = SettingsHandler.SingletonSettingsHandler.Languages[0];
			Assert.Equal(6, i);
			i = 0;
			//test
			SettingsHandler.SingletonSettingsHandler.CurrentColor = SettingsHandler.SingletonSettingsHandler.Colors[0];
			Assert.Equal(1, i);
		}

		[Fact]
		public void PropertyTest()
		{
			//setup
			var vm = new AddSongPageVM_NEW();
			SettingsHandler.SingletonSettingsHandler.CurrentLang = SettingsHandler.SingletonSettingsHandler.Languages[0];
			SettingsHandler.SingletonSettingsHandler.CurrentColor = SettingsHandler.SingletonSettingsHandler.Colors[0];
			//test
			Assert.Equal(SettingsHandler.SingletonSettingsHandler.Colors[0], vm.CurrentColor);
			Assert.Equal("Add New Song", vm.AddSongLabel);
			Assert.Equal("Artist", vm.ArtistLabel);
			Assert.Equal("BPM", vm.BPMLabel);
			Assert.Equal("get_bpm", vm.GetBPMLabel);
			Assert.Equal("Pick Audio File", vm.PickFileLabel);
			Assert.Equal("Title", vm.TitleLabel);
		}

		internal class AddSongPageVM_NEW : AddSongPageVM
		{
			private string _calcbpm;
			private string _path;
			public AddSongPageVM_NEW() { }
			public AddSongPageVM_NEW(string path) { _path = path; }
			public AddSongPageVM_NEW(string calcbpm, string path)
			{
				_calcbpm = calcbpm;
				_path = path;
			}
			public bool Alerted { get; set; }
			protected override string CalculateBPMWrapper() { return _calcbpm; }
			protected override async Task<bool> FileNotNullWrapper()
			{
				_newSongFileLocation = _path;
				return true;
			}
			protected override async Task AlertWrapper(string title, string text, string ok) { Alerted = true; }
		}

		internal class NavigationHandler_NEW : NavigationHandler
		{
			protected override async Task GoBackWrapper() { }
		}

		internal class AudioLib_NEW : AudioLib
		{
			public override async Task AddTrack(string storage, string title, string artist, int bpm) { }
		}

	}
}
