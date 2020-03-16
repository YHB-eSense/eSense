using EarableLibrary;
using System.Threading.Tasks;
using UnitTesting.Mocks;
using Xunit;

namespace UnitTesting.EarableLibraryTests
{
	public class VoltageSensorTests
	{
		[Theory]
		[InlineData(0x56, 255, 255, 1)]
		[InlineData(0x56, 0, 0, 0)]
		[InlineData(0x56, 1, 0, 1)]
		[InlineData(0x56, 0, 1, 0)]
		public async Task TestRead(byte header, byte vol_h, byte vol_l, byte charging)
		{
			MockBLEConnection connection = new MockBLEConnection();
			var data = new byte[] { vol_h, vol_l, charging };
			connection.Storage[VoltageSensor.CHAR_VOLTAGE] = new ESenseMessage(header, data);
			BatteryState result = await new VoltageSensor(connection).ReadAsync();
			Assert.Equal(charging == 1, result.Charging);
			Assert.Equal((vol_h * 256 + vol_l) / 1000.0, result.Voltage, 2);
		}

		[Fact]
		public void BatteryStateTest()
		{
			BatteryState state = new BatteryState();
			Assert.NotNull(state.ToString());
		}
	}
}
