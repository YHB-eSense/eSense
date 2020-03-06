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
			var vm = new MainPageVM_NEW();
			vm.HelpVisible = false;
			vm.HelpCommand.Execute(null);
			Assert.True(vm.HelpVisible);
		}

		internal class MainPageVM_NEW : MainPageVM
		{
			protected override void InitializeSingletons() { }
		}

	}
}
