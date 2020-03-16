using EarableLibrary;
using System.Threading.Tasks;
using UnitTesting.Mocks;
using Xunit;

namespace UnitTesting.EarableLibraryTests
{
	public class MotionSensorTests
	{
		private static readonly byte[] DATA_ZERO = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
		private static readonly byte[] DATA_SEQUENCE = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
		private static readonly byte[] DATA_MAX = { 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255 };

		private IndexedESenseMessage[] PrebuiltMessageArray =
		{
			new IndexedESenseMessage(header: 0x55, packetIndex: 0, data: DATA_ZERO),
			new IndexedESenseMessage(header: 0x55, packetIndex: 1, data: DATA_SEQUENCE),
			new IndexedESenseMessage(header: 0x55, packetIndex: 2, data: DATA_MAX)
		};

		public enum PrebuiltMessage
		{
			EMPTY = 0,
			SEQUENCE = 1,
			MAX = 2
		};

		public IndexedESenseMessage GetPrebuiltMessage(PrebuiltMessage messageId)
		{
			return PrebuiltMessageArray[(int)messageId];
		}

		public MotionSensorSample GetSample(PrebuiltMessage messageId)
		{
			IndexedESenseMessage message = GetPrebuiltMessage(messageId);
			byte index = message.PacketIndex;
			short[] shortArray = message.DataAsShortArray();
			TripleShort gyro = new TripleShort(shortArray[0], shortArray[1], shortArray[2]);
			TripleShort acc = new TripleShort(shortArray[3], shortArray[4], shortArray[5]);
			return new MotionSensorSample(gyro, acc, index);
		}

		[Theory]
		[InlineData(PrebuiltMessage.EMPTY)]
		[InlineData(PrebuiltMessage.SEQUENCE)]
		[InlineData(PrebuiltMessage.MAX)]
		public async Task TestSubscription(PrebuiltMessage messageId)
		{
			MockBLEConnection connection = new MockBLEConnection();
			ESenseMessage message = GetPrebuiltMessage(messageId);
			MotionSensorSample expected = GetSample(messageId);

			MotionSensorSample result = await SubscriptionTest.Perform(
				connection,
				MotionSensor.CHAR_IMU_DATA,
				new MotionSensor(connection),
				message);
			Assert.Equal(expected.SampleId, result.SampleId);
			Assert.Equal(expected.Acc, result.Acc);
			Assert.Equal(expected.Gyro, result.Gyro);
		}

		[Theory]
		[InlineData(PrebuiltMessage.EMPTY)]
		[InlineData(PrebuiltMessage.SEQUENCE)]
		[InlineData(PrebuiltMessage.MAX)]
		public async Task TestRead(PrebuiltMessage messageId)
		{
			MockBLEConnection connection = new MockBLEConnection();
			ESenseMessage message = GetPrebuiltMessage(messageId);
			MotionSensorSample expected = GetSample(messageId);

			connection.Storage[MotionSensor.CHAR_IMU_DATA] = message;
			MotionSensorSample result = await new MotionSensor(connection).ReadAsync();
			Assert.Equal(expected.SampleId, result.SampleId);
			Assert.Equal(expected.Acc, result.Acc);
			Assert.Equal(expected.Gyro, result.Gyro);
		}
	}
}
