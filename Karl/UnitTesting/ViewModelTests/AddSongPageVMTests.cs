using Karl.Model;
using Karl.ViewModel;
using System.Threading.Tasks;
using Xunit;

namespace UnitTesting.ViewModelTests
{
	public class AddSongPageVMTests
	{
		[Theory]
		[InlineData("title", "artist", "0")]
		public void PickFileCommandTest(string val1, string val2, string val3)
		{
			//setup
			var vm = new AddSongPageVM_NEW(val1, val2, val3);
			//test
			vm.PickFileCommand.Execute(null);
			Assert.Equal(val1, vm.NewSongTitle);
			Assert.Equal(val2, vm.NewSongArtist);
			Assert.Equal(val3, vm.NewSongBPM);
		}

		[Theory]
		[InlineData("0")]
		public void GetBPMCommandTest(string val1)
		{
			//setup
			var vm = new AddSongPageVM_NEW(val1);
			//test
			vm.Alerted = false;
			vm.GetBPMCommand.Execute(null);
			Assert.True(vm.Alerted);
			//test
			vm.PickFileCommand.Execute(null);
			vm.CorrectExtension = true;
			vm.GetBPMCommand.Execute(null);
			Assert.Equal(val1, vm.NewSongBPM);
			//test
			vm.CorrectExtension = false;
			vm.Alerted = false;
			vm.GetBPMCommand.Execute(null);
		}

		[Fact]
		public void AddSongCommandTest()
		{
			//setup
			var vm = new AddSongPageVM_NEW();
			//test
			vm.AddSongCommand.Execute(null);
			Assert.True(vm.Alerted);
			//test
			vm.PickFileCommand.Execute(null);
			vm.AddSongCommand.Execute(null);
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
			//test
			vm.PropertyChanged += (sender, e) =>
			{
				i++;
			};

		}

		internal class AddSongPageVM_NEW : AddSongPageVM
		{
			private string _title;
			private string _artist;
			private string _bpm;
			private string _calcbpm;
			public AddSongPageVM_NEW(string title, string artist, string bpm)
			{
				_title = title;
				_artist = artist;
				_bpm = bpm;
			}
			public AddSongPageVM_NEW(string calcbpm)
			{
				_calcbpm = calcbpm;
			}
			public AddSongPageVM_NEW() { }
			public bool Alerted { get; set; }
			public bool CorrectExtension { get; set; }
			protected override string GetTitleWrapper() { return _title; }
			protected override string GetArtistWrapper() { return _artist; }
			protected override string GetBPMWrapper() { return _bpm; }
			protected override string CalculateBPMWrapper() { return _calcbpm; }
			protected override void AddTrackWrapper(int bpm) { }
			protected override void GoBackWrapper() { }
			protected override async Task<bool> FileNotNullWrapper() { return true; }
			protected override bool CorrectExtensionWrapper() { return CorrectExtension; }
			protected override async Task AlertWrapper(string title, string text, string ok) { Alerted = true; }
		}

		}
	}
