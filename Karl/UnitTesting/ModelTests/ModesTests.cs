#define TESTING
using Karl.Model;
using Karl.ViewModel;
using Xunit;

namespace UnitTesting.ModelTests
{
	public class ModesTests
	{

		public ModesTests() {
			//Before
			Mocks.TestDictionary testDictionary = new Mocks.TestDictionary();
			testDictionary.Add("lang", "TestLang");
			SettingsHandler.PropertiesInjection(testDictionary);
			SettingsHandler.Testing(true);
		}

		[Fact]
		public void ActivateDeactivateModesTest() {
			var mode1 = new AutostopMode();
			mode1.Active = true;
			Assert.True(mode1.Active);
			Assert.False(mode1.Autostopped);
			mode1.Active = false;
			Assert.False(mode1.Active);
			Assert.False(mode1.Autostopped);
			var mode2 = new MotivationMode();
			mode2.Active = true;
			Assert.True(mode2.Active);
			mode2.Active = false;
			Assert.False(mode2.Active);
		}

		[Fact]
		public void NameModesTest()
		{
			SettingsHandler.SingletonSettingsHandler.CurrentLang = SettingsHandler.SingletonSettingsHandler.Languages[0];
			var mode1 = new AutostopMode();
			Assert.Equal("AutostopMode", mode1.Name);
			var mode2 = new MotivationMode();
			Assert.Equal("MotivationMode", mode2.Name);
		}
	}
}
