using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Karl.ViewModel;
using Karl.Model;
using Xunit;

namespace UnitTesting.ViewModelTests
{
	public class SettingsPageVMTests : IDisposable
	{

		public SettingsPageVMTests() {
			//TO-DO: Setup
		}

		/// <summary>
		/// Checks if reseting Steps works
		/// </summary>
		[Fact]
		public void resetStepTest() {
			SettingsPageVM spVM = new SettingsPageVM();
			spVM.ResetStepsCommand.Execute(null);
			Assert.Equal(0,SettingsHandler.SingletonSettingsHandler.Steps);
		}

		/// <summary>
		/// Checks if changing the DeviceName works
		/// </summary>
		[Fact]
		public void changeDeviceNameTest()
		{
			SettingsPageVM spVM = new SettingsPageVM();
			_ = SettingsHandler.SingletonSettingsHandler;
			spVM.ChangeDeviceNameCommand.Execute("New Device");
			Assert.Equal("New Device", SettingsHandler.SingletonSettingsHandler.DeviceName);
		}

		
		public void Dispose(){
			//TO-DO: Tear Down
		}
	}
}
