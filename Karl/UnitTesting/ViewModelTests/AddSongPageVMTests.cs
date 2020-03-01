using Karl;
using Karl.Model;
using Karl.ViewModel;
using Moq;
using Plugin.FilePicker.Abstractions;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace UnitTesting
{
    public class AddSongPageVMTests
	{
        public AddSongPageVMTests()
        {
			Tests.Xamarin.Forms.Mocks.MockForms.Init();
		}

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
			//test
			vm.AddSongCommand.Execute(null);
			Assert.Null(vm.NewSongTitle);
			Assert.Null(vm.NewSongArtist);
			Assert.Null(vm.NewSongBPM);
		}
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
			string s = null;
			TagLib.File file = TagLib.File.Create(m);
			file.Tag.Title = "title";
			file.Tag.Performers = new string[] { "artist" };
			file.Tag.BeatsPerMinute = 0;
			return file;
		}
		protected override string GetBPMWrapper()
		{
			return "0";
		}
		protected override void AddTrackWrapper(int bpm) { }
		protected override void GoBackWrapper() { }

	}

}
