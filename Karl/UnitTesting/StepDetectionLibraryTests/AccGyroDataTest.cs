using StepDetectionLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace UnitTesting.StepDetectionLibraryTests
{
	public class AccGyroDataTest
	{
		[Fact]
		public void AttributeTest()
		{
			AccGyroData TestObject = new AccGyroData(60,60);
			Assert.Equal(1, TestObject.LengthInSeconds);
			Assert.Equal(60, TestObject.DataLength);
			TestObject.SamplingRate = 120;
			Assert.Equal(0.5, TestObject.LengthInSeconds);
		}

		[Fact]
		public void AccTest()
		{
			AccGyroData TestObject = new AccGyroData(60, 60);
			TestObject.AccData.Xacc[0] = 1.5;
			Assert.Equal(1.5, TestObject.AccData.Xacc[0]);
			

		}

		[Fact]
		public void GyroTest()
		{
			AccGyroData TestObject = new AccGyroData(60, 60);
			TestObject.GyroData.Xgyro[0] = 1.5;
			Assert.Equal(1.5, TestObject.GyroData.Xgyro[0]);
		}


	}
}
