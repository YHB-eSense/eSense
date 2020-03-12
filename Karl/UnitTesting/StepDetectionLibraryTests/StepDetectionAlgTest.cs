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
	/// Testclass for StepDetectionAlg.cs
	/// </summary>
	public class StepDetectionAlgTest
	{
		/// <summary>
		/// tests subscribe and unsubscribe
		/// </summary>
		[Fact]
		public void SubscribeTest()
		{
			Mock<IObserver<Output>> MockObserver = new Mock<IObserver<Output>>();
			StepDetectionAlg TestSDAlg = new StepDetectionAlg();
			IDisposable TestDisposable = TestSDAlg.Subscribe(MockObserver.Object);
			FieldInfo field = typeof(StepDetectionAlg).GetField("_observer", BindingFlags.NonPublic | BindingFlags.Instance);
			object Oservers = field.GetValue(TestSDAlg);

			List<IObserver<Output>> Observers = (List<IObserver<Output>>)Oservers;
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
			Mock<IObserver<Output>> MockObserver = new Mock<IObserver<Output>>();
			StepDetectionAlg TestSDAlg = new StepDetectionAlg();
			TestSDAlg.Subscribe(MockObserver.Object);
			TestSDAlg.Update(new Output());
			MockObserver.Verify(foo => foo.OnNext(It.IsAny<Output>()));
		}

		/// <summary>
		/// tests the method calculate intensity
		/// </summary>
		[Fact]
		public void CalculateIntensityTest()
		{
			StepDetectionAlg TestSDAlg = new StepDetectionAlg();
			double TestValue = TestSDAlg.CalculateIntensity(new TripleShort(6043, 3458, 2950));
			Assert.InRange<double>(TestValue, 7551, 7552);
		}

		/// <summary>
		/// Tests method onerror
		/// </summary>
		[Fact]
		public void OnErrorTest()
		{
			StepDetectionAlg TestSDAlg = new StepDetectionAlg();
			Assert.Throws<Exception>(() => TestSDAlg.OnError(new Exception()));
		}

	}
}
