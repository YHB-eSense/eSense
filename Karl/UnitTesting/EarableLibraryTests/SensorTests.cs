using EarableLibrary;
using System;
using System.Threading.Tasks;
using Xunit;

namespace UnitTesting.EarableLibraryTests
{
	public class MotionSensorTests
	{
		[Theory]
		[InlineData(0x55, 0, new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0})]
		[InlineData(0x55, 1, new byte[] { 1, 2, 3, 4, 5, 6, 1, 2, 3, 4, 5, 6 })]
		public async Task TestSubscription(byte header, byte packetIndex, byte[] data)
		{
			MockBLEConnection connection = new MockBLEConnection();
			ESenseMessage message = new IndexedESenseMessage(header, data, packetIndex);
			short[] shortArray = message.DataAsShortArray();
			TripleShort acc = new TripleShort(shortArray[0], shortArray[1], shortArray[2]);
			TripleShort gyro = new TripleShort(shortArray[3], shortArray[4], shortArray[5]);
			MotionSensorSample result = await SubscriptionTest.Perform(
				connection,
				MotionSensor.CHAR_IMU_DATA,
				new MotionSensor(connection),
				message);
			Assert.Equal(packetIndex, result.SampleId);
			Assert.Equal(acc, result.Acc);
			Assert.Equal(gyro, result.Gyro);
		}
	}

	public class PushButtonTests
	{
		[Theory]
		[InlineData(0x54, new byte[] { 0 }, false)]
		[InlineData(0x54, new byte[] { 1 }, true)]
		public async Task TestSubscription(byte header, byte[] data, bool expected)
		{
			MockBLEConnection connection = new MockBLEConnection();
			ButtonState result = await SubscriptionTest.Perform(
				connection,
				PushButton.CHAR_BUTTON,
				new PushButton(connection),
				new ESenseMessage(header, data));
			Assert.Equal(expected, result.Pressed);
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
