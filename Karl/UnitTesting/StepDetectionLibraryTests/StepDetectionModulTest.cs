using EarableLibrary;
using Moq;
using StepDetectionLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

			FieldInfo field = typeof(OutputManager).GetField("_observer", BindingFlags.NonPublic | BindingFlags.Instance);
			object Oservers = field.GetValue(OutputManager.SingletonOutputManager);
			List<IObserver<Output>> Observers = (List<IObserver<Output>>)Oservers;
			Observers.Clear();
			OutputManager.SingletonOutputManager.Log.Reset();
		}

		/// <summary>
		/// tests if no step doesnt get detected
		/// </summary>
		[Fact]
		public void NoStepTest()
		{
			Input TestInput = new Input();
			TestInput.ValueChanged(this, new MotionSensorSample(new TripleShort(0, 0, 0), new TripleShort(10, 20, 30), 1));
			Assert.Equal(0, new Output().StepCount());
			Assert.Equal(0, new Output().Frequency());

			OutputManager.SingletonOutputManager.Log.Reset();

		}

		/// <summary>
		/// 
		/// </summary>
		[Fact]
		public void StepTest()
		{
			Input TestInput = new Input();
			TestInput.ValueChanged(this, new MotionSensorSample(new TripleShort(0, 0, 0), new TripleShort(1390, 3220, 8830), 1));
			TestInput.ValueChanged(this, new MotionSensorSample(new TripleShort(0, 0, 0), new TripleShort(1390, 3220, 8830), 1));

			OutputManager.SingletonOutputManager.Log.Reset();
		}
	}
}
