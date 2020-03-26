using Karl.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace UnitTesting.ModelTests
{
	public class BPMCalculatorTests
	{
		[Fact]
		public void CalculateBPMTest()
		{
			var calc = new BPMCalculator(Path.Combine(Environment.CurrentDirectory, @"Data\Song2.wav"));
			Assert.Equal(120, calc.Calculate());
		}
	}
}
