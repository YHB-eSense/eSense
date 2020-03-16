using EarableLibrary;
using System;
using System.Threading.Tasks;
using UnitTesting.Mocks;
using Xunit;

namespace UnitTesting.EarableLibraryTests
{
	public class EventReceiver<T>
	{
		private TaskCompletionSource<T> ReceiverTaskSource;

		public void OnEvent(object sender, T eventArgs)
		{
			ReceiverTaskSource?.SetResult(eventArgs);
		}

		public Task<T> ReceiveOne()
		{
			ReceiverTaskSource = new TaskCompletionSource<T>();
			return ReceiverTaskSource.Task;
		}
	}

	internal class SubscriptionTest
	{
		public static async Task<T> Perform<T>(MockBLEConnection connection, Guid subscriptionId, ISubscribableSensor<T> sensor, BLEMessage message)
		{
			EventReceiver<T> receiver = new EventReceiver<T>();
			Task<T> receiveTask = receiver.ReceiveOne();

			sensor.ValueChanged += receiver.OnEvent;
			await sensor.StartSamplingAsync();
			connection.InvokeSubscription(subscriptionId, message);
			await sensor.StopSamplingAsync();
			sensor.ValueChanged -= receiver.OnEvent;

			Assert.True(receiveTask.IsCompleted);
			return receiveTask.Result;
		}
	}
}
