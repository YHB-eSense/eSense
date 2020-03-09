using Karl.ViewModel;
using Moq;
using Plugin.FilePicker.Abstractions;
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
			var vm = new AddSongPageVM_NEW();
			//test
			vm.PickFileCommand.Execute(null);
			Assert.Equal("title", vm.NewSongTitle);
			Assert.Equal("artist", vm.NewSongArtist);
			Assert.Equal("0", vm.NewSongBPM);
		}

		[Fact]
		public void GetBPMCommandTest()
		{
			//setup
			var vm = new AddSongPageVM_NEW();
			vm.PickFileCommand.Execute(null);
			//test
			vm.GetBPMCommand.Execute(null);
			Assert.Equal("0", vm.NewSongBPM);
		}

		[Fact]
		public void AddSongCommandTest()
		{
			//setup
			var vm = new AddSongPageVM_NEW();
			vm.PickFileCommand.Execute(null);
			//test
			vm.AddSongCommand.Execute(null);
			Assert.Null(vm.NewSongTitle);
			Assert.Null(vm.NewSongArtist);
			Assert.Null(vm.NewSongBPM);
		}

		internal class AddSongPageVM_NEW : AddSongPageVM
		{
			protected override string GetTitle() { return "title"; }
			protected override string GetArtist() { return "artist"; }
			protected override string GetBPM() { return "0"; }
			protected override string CalculateBPMWrapper() { return "0"; }
			protected override void AddTrackWrapper(int bpm) { }
			protected override void GoBackWrapper() { }
			protected override void InitializeSingletons() { }
			protected override async Task<bool> FileNotNullWrapper() { return true; }
			protected override bool CorrectExtensionWrapper() { return true; }
		}
	}
}
