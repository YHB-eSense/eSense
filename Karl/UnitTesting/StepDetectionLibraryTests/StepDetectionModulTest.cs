using EarableLibrary;
using Moq;
using StepDetectionLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace UnitTesting.StepDetectionLibraryTests
{
	/// <summary>
	/// Class for Modul tests for Stepdetectionlibrary
	/// </summary>
	 public class StepDetectionModulTest
	{
		/// <summary>
		/// Tests if subscriber to output receive if valuechanges
		/// </summary>
		[Fact]
		public void InandOutTests()
		{
			Input TestInput = new Input();
			Mock<IObserver<Output>> MockObserver = new Mock<IObserver<Output>>();
			OutputManager.SingletonOutputManager.Subscribe(MockObserver.Object);
			TestInput.ValueChanged(this, new MotionSensorSample(new TripleShort(0, 0, 0), new TripleShort(1, 2, 3), 1));

			MockObserver.Verify(foo => foo.OnNext(It.IsAny<Output>()));
		}

		/// <summary>
		/// tests if no step doesnt get detected
		/// </summary>
		[Fact]
		public void NoStepTest()
		{
			Input TestInput = new Input();
			OutputManager.SingletonOutputManager.Log.Reset();
			TestInput.ValueChanged(this, new MotionSensorSample(new TripleShort(0, 0, 0), new TripleShort(10, 20, 30), 1));
			Assert.Equal(0, new Output().StepCount());
			Assert.Equal(0, new Output().Frequency());
		}
	}
}
