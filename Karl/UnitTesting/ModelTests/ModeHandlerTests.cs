using Karl.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace UnitTesting.ModelTests
{
	public class ModeHandlerTests
	{
		[Fact]
		public void ResetModesTest()
		{
			//setup
			var handler = ModeHandler.SingletonModeHandler;
			var list = new List<Mode>(handler.Modes);
			//test
			handler.ResetModes();
			Assert.Equal(list, handler.Modes);
		}
	}
}
