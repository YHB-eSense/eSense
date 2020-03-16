#define TESTING
using Karl.Model;
using Karl.ViewModel;
using Xunit;

namespace UnitTesting
{
	public class ModesPageVMTest
	{

		public ModesPageVMTest() {
			//Before
			Mocks.TestDictionary testDictionary = new Mocks.TestDictionary();
			testDictionary.Add("lang", "TestLang");
			SettingsHandler.PropertiesInjection(testDictionary);
			SettingsHandler.Testing(true);
		}

		[Fact]
		public void ActivateAutoStopTest() {
			var vm = new ModesPageVM();
			vm.Modes[0].Active = true;
			Assert.True(ModeHandler.SingletonModeHandler.Modes[0].Active);
		}

		[Fact]
		public void ActivateMotivationModeTest()
		{
			var vm = new ModesPageVM();
			vm.Modes[1].Active = true;
			Assert.True(ModeHandler.SingletonModeHandler.Modes[1].Active);
		}
	}
}
