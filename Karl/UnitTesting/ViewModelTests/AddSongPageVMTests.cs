using Karl;
using Karl.Model;
using Karl.ViewModel;
using Moq;
using Plugin.FilePicker.Abstractions;
using System;
using System.Linq;
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
			Assert.NotNull(vm.NewSongArtist);
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
			/*
			var mock = new Mock<IBPMCalculator>();
			mock.Setup(x => x.Calculate()).Returns(0);
			vm.GetBPMCommand.Execute(mock.Object);
			*/
		}

		[Fact]
		public void AddSongCommandTest()
		{
			//setup
			var vm = new AddSongPageVM_NEW();
			vm.PickFileCommand.Execute(null);
			vm.NewSongArtist = "artist";
			vm.NewSongBPM = "artist";
			vm.NewSongTitle = "artist";
			//test
			vm.AddSongCommand.Execute(null);
			Assert.Null(vm.NewSongTitle);
			Assert.Null(vm.NewSongArtist);
			Assert.Null(vm.NewSongBPM);
		}

		internal class AddSongPageVM_NEW : AddSongPageVM
		{
			protected override async Task<FileData> PickFileWrapper()
			{
				FileData data = new FileData();
				data.FilePath = "test.wav";
				return new FileData();
			}
			protected override TagLib.File CreateFileWrapper()
			{
				var mock = new Mock<TagLib.File>();
				mock.Setup(x => x.Tag.Title).Returns("title");
				mock.Setup(x => x.Tag.Performers).Returns(new string[] { "artist" });
				mock.Setup(x => x.Tag.BeatsPerMinute).Returns(0);
				return mock.Object;
			}
			protected override string GetBPMWrapper()
			{
				return "0";
			}
			protected override void AddTrackWrapper(int bpm) { }
			protected override void GoBackWrapper() { }
			protected override void InitializeSingletons() { }

		}
	}
}
