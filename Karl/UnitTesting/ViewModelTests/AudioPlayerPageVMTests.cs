using Karl.Model;
using Karl.ViewModel;
using Moq;
using Xunit;

namespace UnitTesting.ViewModelTests
{
	public class AudioPlayerPageVMTests
	{
		[Fact]
		public void PausePlayCommandTest() {
			
			
		}

		[Fact]
		public void PlayPrevCommandTest() {

		}

		[Fact]
		public void PlayNextCommandTest() { }

		[Fact]
		public void PositionDragStartedCommandTest() {

		}

		[Fact]
		public void PositionDragCompletedCommandTest() {
			//AudioPlayerPageVM apVM = new AudioPlayerPageVM();
			var vm = new Mock<AudioPlayerPageVM_New>();
			vm.Setup(x => x.AudioTrack).Returns(new TestAudioTrack());
			vm.Object.PositionDragCompletedCommand.Execute(0.5);
			Assert.Equal(vm.Object.TimePlayed,"50");
		}

		public class AudioPlayerPageVM_New : AudioPlayerPageVM {
			protected override void InitializeSingletons() {}
			
		}

	}

}
