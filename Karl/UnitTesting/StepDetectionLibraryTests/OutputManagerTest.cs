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
	/// TestClass for OutputManager
	/// </summary>
	public class OutputManagerTest
	{
		/// <summary>
		/// tests subscribe and unsubscribe
		/// </summary>
		[Fact]
		public void SubscribeTest()
		{
			
			Mock<IObserver<Output>> MockObserver = new Mock<IObserver<Output>>();
			IDisposable TestDisposable = OutputManager.SingletonOutputManager.Subscribe(MockObserver.Object);
			FieldInfo field = typeof(OutputManager).GetField("_observer", BindingFlags.NonPublic | BindingFlags.Instance);
			object Oservers = field.GetValue(OutputManager.SingletonOutputManager);
			List<IObserver<Output>> Observers = (List<IObserver<Output>>)Oservers;
			Assert.Contains(MockObserver.Object, Observers);
			TestDisposable.Dispose();
			Assert.DoesNotContain(MockObserver.Object, Observers);

			Observers.Clear();
			OutputManager.SingletonOutputManager.Log.Reset();
		}

		/// <summary>
		/// tests if subscribers get update
		/// </summary>
		[Fact]
		public void UpdateTest()
		{
			Mock<IObserver<Output>> MockObserver = new Mock<IObserver<Output>>();
			IDisposable TestDisposable = OutputManager.SingletonOutputManager.Subscribe(MockObserver.Object);
			OutputManager.SingletonOutputManager.Update(new Output());
			MockObserver.Verify(foo => foo.OnNext(It.IsAny<Output>()));

			FieldInfo field = typeof(OutputManager).GetField("_observer", BindingFlags.NonPublic | BindingFlags.Instance);
			object Oservers = field.GetValue(OutputManager.SingletonOutputManager);
			List<IObserver<Output>> Observers = (List<IObserver<Output>>)Oservers;
			Observers.Clear();
			OutputManager.SingletonOutputManager.Log.Reset();
		}

		/// <summary>
		/// Tests method onerror
		/// </summary>
		[Fact]
		public void OnErrorTest()
		{
			Assert.Throws<Exception>(() => OutputManager.SingletonOutputManager.OnError(new Exception()));
		}
	}
}
