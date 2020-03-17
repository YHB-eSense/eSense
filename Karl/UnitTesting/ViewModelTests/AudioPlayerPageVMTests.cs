#define TESTING
using Karl.Model;
using Karl.ViewModel;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading;
using Xunit;

namespace UnitTesting.ViewModelTests
{
	public class AudioPlayerPageVMTests
	{
		[Fact]
		public void PausePlayCommandTest()
		{
			new Thread(() =>
			{
				//setup
				SettingsHandler.Testing(true);
				var mockObj = new Mock<IDictionary<string, Object>>();
				SettingsHandler.PropertiesInjection(mockObj.Object);
				var vm = new AudioPlayerPageVM_NEW();
				int i = 0;
				vm.PropertyChanged += (sender, e) => i++;
				//test
				Assert.True(vm.Player.Paused);
				vm.PausePlayCommand.Execute(null);
				Assert.False(vm.Player.Paused);
				Assert.Equal(3, i);
			}).Start();
		}

		[Fact]
		public void PlayPrevCommandTest()
		{
			//setup
			SettingsHandler.Testing(true);
			var mockObj = new Mock<IDictionary<string, Object>>();
			SettingsHandler.PropertiesInjection(mockObj.Object);
			var vm = new AudioPlayerPageVM_NEW();
			//test
			vm.PlayPrevCommand.Execute(null);
			Assert.Null(vm.Player.CurrentTrack);
		}

		[Fact]
		public void PlayNextCommandTest()
		{
			//setup
			SettingsHandler.Testing(true);
			var mockObj = new Mock<IDictionary<string, Object>>();
			SettingsHandler.PropertiesInjection(mockObj.Object);
			var vm = new AudioPlayerPageVM_NEW();
			//test
			vm.PlayNextCommand.Execute(null);
			Assert.Null(vm.Player.CurrentTrack);
		}

		[Fact]
		public void PositionDragStartedCommandTest()
		{
			//setup
			SettingsHandler.Testing(true);
			var mockObj = new Mock<IDictionary<string, Object>>();
			SettingsHandler.PropertiesInjection(mockObj.Object);
			var vm = new AudioPlayerPageVM_NEW();
			vm.PausePlayCommand.Execute(null);
			//test
			Assert.False(vm.Player.Paused);
			Assert.True(vm.WasPaused);
			vm.PositionDragStartedCommand.Execute(null);
			Assert.True(vm.Player.Paused);
			Assert.False(vm.WasPaused);
			vm.PositionDragStartedCommand.Execute(null);
			Assert.True(vm.Player.Paused);
			Assert.True(vm.WasPaused);
		}

		[Fact]
		public void PositionDragCompletedCommandTest()
		{
			//setup
			SettingsHandler.Testing(true);
			var mockObj = new Mock<IDictionary<string, Object>>();
			SettingsHandler.PropertiesInjection(mockObj.Object);
			var vm = new AudioPlayerPageVM_NEW();
			//test
			vm.PositionDragCompletedCommand.Execute(null);
			Assert.Equal(0.5, vm.CurrentPosition);
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
				var vm = new AudioPlayerPageVM_NEW();
				int i = 0;
				vm.PropertyChanged += (sender, e) => i++;
				//test
				SettingsHandler.SingletonSettingsHandler.CurrentColor = SettingsHandler.SingletonSettingsHandler.Colors[0];
				Assert.Equal(1, i);
				i = 0;
				//test
				SettingsHandler.SingletonSettingsHandler.ChangeAudioModuleToBasic();
				Assert.Equal(7, i);
			}).Start();
		}

		[Fact]
		public void PropertyTest()
		{
			//setup
			SettingsHandler.Testing(true);
			var mockObj = new Mock<IDictionary<string, Object>>();
			SettingsHandler.PropertiesInjection(mockObj.Object);
			var vm = new AudioPlayerPageVM_NEW();
			//test
			vm.Volume = 0.5;
			Assert.Equal(0.2, vm.CurrentPosition);
			Assert.Equal("0:10", vm.TimePlayed);
			Assert.Equal("0:40", vm.TimeLeft);
			Assert.Equal(0.5, vm.Volume);
		}

		internal class AudioPlayerPageVM_NEW : AudioPlayerPageVM
		{
			public AudioPlayerPageVM_NEW()
			{
				_dragValue = 0.5;
				UsingBasicAudio = false;
			}
			public AudioPlayer Player { get => _audioPlayer; }
			public bool WasPaused { get => _wasPaused; }
			public override bool UsingBasicAudio { get; }
			protected override void InitializeSingletons()
			{
				_audioPlayer = new AudioPlayer_NEW();
			}
		}

		internal class AudioPlayer_NEW : AudioPlayer
		{
			public AudioPlayer_NEW()
			{
				Paused = true;
				CurrentTrack = new AudioTrack_NEW();
				CurrentSecInTrack = 10;
			}
			public override bool Paused { get; set; }
			public override AudioTrack CurrentTrack { get; set; }
			public override double CurrentSecInTrack { get; set; }
			public override double Volume { get; set; }
			public override void TogglePause() { Paused = !Paused; }
			public override void PrevTrack() { CurrentTrack = null; }
			public override void NextTrack() { CurrentTrack = null; }
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
