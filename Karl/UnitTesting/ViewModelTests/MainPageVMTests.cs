using Karl.ViewModel;
using Xunit;

namespace UnitTesting.ViewModelTests
{
	public class MainPageVMTests
	{
		[Fact]
		public void AudioPlayerPageCommandTest() { }

		[Fact]
		public void AudioLibPageCommandTest() { }

		[Fact]
		public void TryConnectCommandTest() { }

		[Fact]
		public void ModesPageCommandTest() { }

		[Fact]
		public void SettingsPageCommandTest() { }

		[Fact]
		public void HelpCommandTest()
		{
			var vm = new MainPageVM();
			vm.HelpVisible = false;
			vm.HelpCommand.Execute(null);
			Assert.True(vm.HelpVisible);
		}

	}
}
