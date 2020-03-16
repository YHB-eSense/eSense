using EarableLibrary;
using System.Threading.Tasks;
using UnitTesting.Mocks;
using Xunit;

namespace UnitTesting.EarableLibraryTests
{
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

		[Theory]
		[InlineData(0x54, new byte[] { 0 }, false)]
		[InlineData(0x54, new byte[] { 1 }, true)]
		public async Task TestRead(byte header, byte[] data, bool expected)
		{
			MockBLEConnection connection = new MockBLEConnection();
			connection.Storage[PushButton.CHAR_BUTTON] = new ESenseMessage(header, data);
			ButtonState result = await new PushButton(connection).ReadAsync();
			Assert.Equal(expected, result.Pressed);
		}

		[Fact]
		public void SamplingRateTest()
		{
			MockBLEConnection connection = new MockBLEConnection();
			PushButton sensor = new PushButton(connection);
			Assert.Equal(-1, sensor.SamplingRate);
			sensor.SamplingRate = 1;
			Assert.Equal(-1, sensor.SamplingRate);
		}

		[Fact]
		public void ButtonStateTest()
		{
			ButtonState on = new ButtonState(true);
			ButtonState off = new ButtonState(false);

			Assert.NotEqual(on, off);
			Assert.Equal(on, new ButtonState(true));
			Assert.NotEqual(on.ToString(), off.ToString());
			Assert.Equal(off.ToString(), new ButtonState(false).ToString());

			Assert.False(on.Equals(null));
			Assert.False(off.Equals(new object()));
		}
	}
}
