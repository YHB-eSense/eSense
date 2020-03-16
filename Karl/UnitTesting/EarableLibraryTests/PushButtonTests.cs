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
	}
}
