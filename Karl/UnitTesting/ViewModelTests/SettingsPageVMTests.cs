using Karl.Model;
using Karl.ViewModel;
using System;
using System.Collections.Generic;
using Xunit;

namespace UnitTesting.ViewModelTests
{
	public class SettingsPageVMTests : IDisposable
	{

		public SettingsPageVMTests()
		{
			//TO-DO: Setup
		}


		/// <summary>
		/// Checks if changing Device Name works
		/// </summary>
		[Fact]
		public void ChangeDeviceNameTest()
		{
			SettingsPageVM_NEW spVM = new SettingsPageVM_NEW();
			spVM.DeviceName = "New Device";
			spVM.ChangeDeviceNameCommand.Execute(null);
			//Should be null because atm no earables are connected
			Assert.Null(spVM.DeviceName);
		}

		/// <summary>
		/// Checks if reseting Steps works
		/// </summary>
		[Fact]
		public void ResetStepsTest()
		{
			SettingsPageVM_NEW spVM = new SettingsPageVM_NEW();
			spVM.ResetStepsCommand.Execute(null);
			Assert.Equal(0, SettingsHandler.SingletonSettingsHandler.Steps);
		}



		/// <summary>
		/// Checks if changing language works
		/// </summary>
		[Fact]
		public void ChangeLanguageTest()
		{
			SettingsPageVM spVM = new SettingsPageVM();
			spVM.SelectedLanguage = LangManager.SingletonLangManager.AvailableLangs[1];
			Assert.Equal(LangManager.SingletonLangManager.CurrentLang,
				LangManager.SingletonLangManager.AvailableLangs[1]);
		}


		/// <summary>
		/// Checks if changing Color works
		/// </summary>
		[Fact]
		public void ChangeColorTest()
		{
			SettingsPageVM spVM = new SettingsPageVM();
			spVM.CurrentColor = ColorManager.SingletonColorManager.Colors[0];
			Assert.Equal(SettingsHandler.SingletonSettingsHandler.CurrentColor,
				ColorManager.SingletonColorManager.Colors[0]);
		}

		public void Dispose()
		{
			//TO-DO: Tear Down
		}

		internal class SettingsHandler_NEW : SettingsHandler {

			protected override void InitProperties()
			{
				
			}
		}

		internal class SettingsPageVM_NEW : SettingsPageVM {

			public SettingsPageVM_NEW() {
				_settingsHandler = SettingsHandler_NEW.SingletonSettingsHandler;
			}

		}
	}
}
