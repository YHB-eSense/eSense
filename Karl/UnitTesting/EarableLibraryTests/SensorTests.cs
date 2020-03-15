using EarableLibrary;
using System.Threading.Tasks;
using Xunit;

namespace UnitTesting.EarableLibraryTests
{
	public class MotionSensorTests
	{
		[Theory]
		[InlineData(0x55, 0, new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0})]
		public async Task TestSubscription(byte header, byte packetIndex, byte[] data)
		{
			var connection = new MockBLEConnection();
			MotionSensor sensor = new MotionSensor(connection);
			EventReceiver<MotionSensorSample> receiver = new EventReceiver<MotionSensorSample>();
			Task receiveTask = receiver.ReceiveOne();
			BLEMessage message = new IndexedESenseMessage(header, data, packetIndex);

			sensor.ValueChanged += receiver.OnEvent;
			await sensor.StartSamplingAsync();
			connection.InvokeSubscription(MotionSensor.CHAR_IMU_DATA, message);
			await sensor.StopSamplingAsync();
			sensor.ValueChanged -= receiver.OnEvent;

			Assert.True(receiveTask.IsCompleted);
		}
	}
}
