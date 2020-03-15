using Karl.Model;
using Karl.ViewModel;
using Xunit;

namespace UnitTesting
{
	public class ModesPageVMTest
	{
		[Fact]
		public void ActivateAutoStopTest() {
			var vm = new ModesPageVM();
			SettingsHandler.SingletonSettingsHandler.CurrentLang = SettingsHandler.SingletonSettingsHandler.Languages[0];
			vm.Modes[0].Active = true;
			Assert.True(ModeHandler.SingletonModeHandler.Modes[0].Active);
		}

		[Fact]
		public void ActivateMotivationModeTest()
		{
			var vm = new ModesPageVM();
			SettingsHandler.SingletonSettingsHandler.CurrentLang = SettingsHandler.SingletonSettingsHandler.Languages[0];
			vm.Modes[1].Active = true;
			Assert.True(ModeHandler.SingletonModeHandler.Modes[1].Active);
		}
	}
}
