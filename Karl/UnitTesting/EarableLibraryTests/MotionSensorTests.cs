using EarableLibrary;
using System.Threading.Tasks;
using UnitTesting.Mocks;
using Xunit;

namespace UnitTesting.EarableLibraryTests
{
	public class MotionSensorTests
	{
		private static readonly TripleShort TS_ZERO = new TripleShort(0, 0, 0);
		private static readonly TripleShort TS_PLUS_ONE = new TripleShort(1, 1, 1);
		private static readonly TripleShort TS_MINUS_ONE = new TripleShort(-1, -1, -1);

		private static readonly byte[] DATA_ZERO = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
		private static readonly byte[] DATA_SEQUENCE = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
		private static readonly byte[] DATA_MAX = { 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255 };
		private static readonly int SAMPLE_RATE_INDEX = 4;
		private static readonly int ON_OFF_INDEX = 3;

		private readonly IndexedESenseMessage[] PrebuiltMessageArray =
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
			//MotionSensorSample result = await new MotionSensor(connection).ReadAsync();
			//Assert.Equal(expected.SampleId, result.SampleId);
			//Assert.Equal(expected.Acc, result.Acc);
			//Assert.Equal(expected.Gyro, result.Gyro);
		}

		[Theory]
		[InlineData(10)]
		[InlineData(20)]
		[InlineData(50)]
		[InlineData(100)]
		public async Task StartStopSamplingTest(int rate)
		{
			MockBLEConnection connection = new MockBLEConnection();
			MotionSensor sensor = new MotionSensor(connection)
			{
				SamplingRate = rate
			};
			byte[] data;

			await sensor.StartSamplingAsync();
			Assert.True(connection.Storage.ContainsKey(MotionSensor.CHAR_IMU_ENABLE));
			data = connection.Storage[MotionSensor.CHAR_IMU_ENABLE];
			Assert.Equal(rate, data[SAMPLE_RATE_INDEX]);
			Assert.Equal(0x01, data[ON_OFF_INDEX]);

			await sensor.StopSamplingAsync();
			Assert.True(connection.Storage.ContainsKey(MotionSensor.CHAR_IMU_ENABLE));
			data = connection.Storage[MotionSensor.CHAR_IMU_ENABLE];
			Assert.Equal(0x00, data[ON_OFF_INDEX]);
		}

		[Fact]
		public void MotionSensorSampleTest()
		{
			MotionSensorSample sample1 = new MotionSensorSample(TS_ZERO, TS_ZERO, 0);
			MotionSensorSample sample2 = new MotionSensorSample(TS_ZERO, TS_ZERO, 1);
			MotionSensorSample sample3 = new MotionSensorSample(TS_PLUS_ONE, TS_MINUS_ONE, 1);

			Assert.NotEqual(sample1, sample2);
			Assert.NotEqual(sample2, sample3);
			Assert.NotEqual(sample3, sample1);

			Assert.NotEqual(sample1.ToString(), sample2.ToString());

			Assert.Equal(sample1, new MotionSensorSample(new TripleShort(0, 0, 0), new TripleShort(0, 0, 0), 0));

			Assert.False(sample1.Equals(null));
			Assert.False(sample1.Equals(new object()));
		}

		[Fact]
		public void TripleShortTest()
		{
			Assert.Equal(TS_ZERO, new TripleShort(0, 0, 0));
			Assert.NotEqual(TS_ZERO, TS_PLUS_ONE);
			Assert.False(TS_ZERO.Equals(null));
			Assert.False(TS_ZERO.Equals(new object()));

			Assert.NotEqual(TS_ZERO.ToString(), TS_PLUS_ONE.ToString());
		}
	}
}
