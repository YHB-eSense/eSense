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
			//setup
			var vm = new AudioPlayerPageVM_NEW();
			//test
			Assert.True(vm.Paused);
			vm.PausePlayCommand.Execute(null);
			Assert.False(vm.Paused);
		}

		[Fact]
		public void PlayPrevCommandTest() { }

		[Fact]
		public void PlayNextCommandTest() { }

		[Fact]
		public void PositionDragStartedCommandTest()
		{
			//setup
			var vm = new AudioPlayerPageVM_NEW();
			vm.PausePlayCommand.Execute(null);
			//test
			Assert.False(vm.Paused);
			Assert.True(vm.WasPaused);
			vm.PositionDragStartedCommand.Execute(null);
			Assert.True(vm.Paused);
			Assert.False(vm.WasPaused);
			vm.PositionDragStartedCommand.Execute(null);
			Assert.True(vm.Paused);
			Assert.True(vm.WasPaused);
		}

		[Fact]
		public void PositionDragCompletedCommandTest()
		{
			//setup
			var vm = new AudioPlayerPageVM_NEW();
			//test
			vm.PositionDragCompletedCommand.Execute(null);
			Assert.Equal(25, vm.CurrentPosition);
		}

		internal class AudioPlayerPageVM_NEW : AudioPlayerPageVM
		{
			public bool Paused { get => _audioPlayer.Paused; }
			public bool WasPaused { get => _wasPaused; }
			public override bool UsingBasicAudio { get => true; }
			protected override void InitializeSingletons()
			{
				_audioPlayer = new AudioPlayer_NEW();
				_dragValue = 0.5;
			}
		}

		internal class AudioPlayer_NEW : AudioPlayer
		{
			public override bool Paused { get; set; }
			public override AudioTrack CurrentTrack { get => new AudioTrack_NEW(); }
			public AudioPlayer_NEW()
			{
				Paused = true;
			}
			public override void TogglePause()
			{
				Paused = !Paused;
			}
			public override void PrevTrack() { }
			public override void NextTrack() { }
		}

		internal class AudioTrack_NEW : AudioTrack
		{
			public AudioTrack_NEW()
			{
				Duration = 50;
			}
			public override double Duration { get; set; }
			public override byte[] Cover { get; set; }
			public override string Title { get; set; }
			public override string Artist { get; set; }
			public override int BPM { get; set; }
			public override string StorageLocation { get; set; }
			public override string TextId { get; set; }
		}
	}

}
