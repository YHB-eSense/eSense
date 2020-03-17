#define TESTING
using Karl.Model;
using Karl.ViewModel;
using System;
using System.Collections.Generic;
using System.Threading;
using Xunit;

namespace UnitTesting.ViewModelTests
{
	public class SettingsPageVMTests 
	{

		public SettingsPageVMTests()
		{
			//Before
			Mocks.TestDictionary testDictionary = new Mocks.TestDictionary();
			testDictionary.Add("lang", "TestLang");
			SettingsHandler.PropertiesInjection(testDictionary);
			SettingsHandler.Testing(true);
		}


		/// <summary>
		/// Checks if changing Device Name works
		/// </summary>
		[Fact]
		public void ChangeDeviceNameTest()
		{
			SettingsPageVM spVM = new SettingsPageVM();
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
			SettingsPageVM spVM = new SettingsPageVM();
			spVM.ResetStepsCommand.Execute(null);
			Assert.Equal(0, SettingsHandler.SingletonSettingsHandler.Steps);
		}



		/// <summary>
		/// Checks if changing language works
		/// </summary>
		[Fact]
		public void ChangeLanguageTest()
		{
			new Thread(() =>
			{
				SettingsPageVM spVM = new SettingsPageVM();
				spVM.SelectedLanguage = LangManager.SingletonLangManager.AvailableLangs[0];
				Assert.Equal(LangManager.SingletonLangManager.CurrentLang,
					LangManager.SingletonLangManager.AvailableLangs[0]);
			}).Start();
		}


		/// <summary>
		/// Checks if changing Color works
		/// </summary>
		[Fact]
		public void ChangeColorTest()
		{
			SettingsPageVM spVM = new SettingsPageVM();
			spVM.CurrentColor = ColorManager.SingletonColorManager.Colors[0];
			Assert.Equal(SettingsHandler.SingletonSettingsHandler.CurrentColor.Name,
				ColorManager.SingletonColorManager.Colors[0].Name);
		}

	}
}
