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
	/// TestClass for Input.cs
	/// </summary>
	public class InputTest
	{
		/// <summary>
		/// tests subscribe and unsubscribe
		/// </summary>
		[Fact]
		public void SubscribeTest()
		{
			Mock<IObserver<AccelerationSample>> MockObserver = new Mock<IObserver<AccelerationSample>>();
			Input TestInput = new Input();
			IDisposable TestDisposable = TestInput.Subscribe(MockObserver.Object);
			FieldInfo field = typeof(Input).GetField("_observer", BindingFlags.NonPublic | BindingFlags.Instance);
			object Oservers = field.GetValue(TestInput);
			//PropertyInfo prop = typeof(Input).GetProperty("_observers", BindingFlags.NonPublic | BindingFlags.Instance);
			//MethodInfo getter = prop.GetGetMethod(nonPublic: true);
			//object Oservers = getter.Invoke(TestInput, null);
			List<IObserver<AccelerationSample>> Observers = (List<IObserver<AccelerationSample>>)Oservers;
			Assert.Contains(MockObserver.Object, Observers);
			TestDisposable.Dispose();
			Assert.DoesNotContain(MockObserver.Object, Observers);
		}

		/// <summary>
		/// tests if subscribers get update
		/// </summary>
		[Fact]
		public void UpdateTest()
		{
			Mock<IObserver<AccelerationSample>> MockObserver = new Mock<IObserver<AccelerationSample>>();
			Input TestInput = new Input();
			TestInput.Subscribe(MockObserver.Object);
			TestInput.Update(new AccelerationSample());
			MockObserver.Verify(foo => foo.OnNext(It.IsAny<AccelerationSample>()));
		}

		/// <summary>
		/// tests if subscribers gets the update after ValueChanged
		/// </summary>
		[Fact]
		public void ValueChangedTest()
		{
			Mock<IObserver<AccelerationSample>> MockObserver = new Mock<IObserver<AccelerationSample>>();
			Input TestInput = new Input();
			TestInput.Subscribe(MockObserver.Object);

			MotionSensorSample TestSample = new MotionSensorSample(new TripleShort(0,0,0), new TripleShort(1, 2, 3), 1);
			
			TestInput.ValueChanged(this, TestSample);
			MockObserver.Verify(foo => foo.OnNext(It.IsAny<AccelerationSample>()));
		}

	}
}
