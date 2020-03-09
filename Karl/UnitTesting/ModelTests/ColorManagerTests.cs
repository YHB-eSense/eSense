using Karl.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace UnitTesting.ModelTests
{
	public class ColorManagerTests
	{
		[Fact]
		public void ResetColorsTest()
		{
			//setup
			var mo = ColorManager.SingletonColorManager;
			List<CustomColor> oldColors = new List<CustomColor>(mo.Colors);
			//test
			mo.ResetColors();
			for(int i = 0; i < mo.Colors.Count; i++)
			{
				Assert.Equal(oldColors[i].Color, mo.Colors[i].Color);
			}
		}

		[Fact]
		public void CurrentColorTest()
		{
			//setup
			var mo = ColorManager.SingletonColorManager;
			//test
			mo.CurrentColor = mo.Colors[0];
			Assert.Equal(mo.Colors[0].Color, mo.CurrentColor.Color);
		}
	}
}
