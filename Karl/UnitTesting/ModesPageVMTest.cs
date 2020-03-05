using Karl.Model;
using Karl.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace UnitTesting
{
	public class ModesPageVMTest
	{
		[Fact]
		public void ActivateAutoStopTest() {
			ModesPageVM mpVM = new ModesPageVM();
			mpVM.Modes[0].Active = true;
			Assert.True(ModeHandler.SingletonModeHandler.Modes[0].Active);
		}

		[Fact]
		public void ActivateMotivationModeTest()
		{
			ModesPageVM mpVM = new ModesPageVM();
			mpVM.Modes[1].Active = true;
			Assert.True(ModeHandler.SingletonModeHandler.Modes[1].Active);
		}
	}
}
