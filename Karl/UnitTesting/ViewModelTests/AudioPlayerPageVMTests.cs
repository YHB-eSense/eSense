using Karl.Model;
using Karl.ViewModel;
using Moq;
using Xunit;

namespace UnitTesting.ViewModelTests
{
	public class AudioPlayerPageVMTests
	{
		[Fact]
		public void PausePlayCommandTest()
		{


		}

		[Fact]
		public void PlayPrevCommandTest()
		{

		}

		[Fact]
		public void PlayNextCommandTest() { }

		[Fact]
		public void PositionDragStartedCommandTest()
		{

		}

		[Fact]
		public void PositionDragCompletedCommandTest()
		{
			AudioPlayerPageVM_New vm = new AudioPlayerPageVM_New();
			vm.PositionDragCompletedCommand.Execute(null);
			Assert.Equal(50.0, vm.CurrentPosition);
		}

		public class AudioPlayerPageVM_New : AudioPlayerPageVM
		{
			public AudioPlayerPageVM_New():base()
			{
				
			}
			public override AudioTrack AudioTrack { get => new TestAudioTrack(); }
			protected override void InitializeSingletons()
			{
				_dragValue = 0.5;
				var mock = new Mock<AudioPlayer>();
				mock.Setup(x => x.CurrentSecInTrack).Returns(2);
				_audioPlayer = mock.Object;
			}
			protected override void AudioPlayerPlayPause() { }
			protected override void AudioPlayerDrag() {
				_audioPlayer.CurrentSecInTrack = _dragValue*AudioTrack.Duration;
			}
		}

	}

}
