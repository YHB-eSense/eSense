using EarableLibrary;
using System;
using System.Threading.Tasks;
using UnitTesting.Mocks;
using Xunit;

namespace UnitTesting.EarableLibraryTests
{
	public class ESenseTests
	{
		[Theory]
		[InlineData("eSense-0001")]
		public async Task NameChangeTest(string name)
		{
			MockBLEConnection connection = new MockBLEConnection();
			ESense earable = new ESense(connection);
			await earable.ConnectAsync();

			Assert.True(await earable.SetNameAsync(name));
			connection.Storage[EarableName.CHAR_NAME_R] = connection.Storage[EarableName.CHAR_NAME_W];
			Assert.Equal(name, earable.Name);
			await earable.DisconnectAsync();
			await earable.ConnectAsync();
			Assert.Equal(name, earable.Name);
		}

		[Fact]
		public async Task ConnectionTest()
		{
			MockBLEConnection connection = new MockBLEConnection();
			ESense earable = new ESense(connection);

			Assert.False(earable.IsConnected());
			await earable.ConnectAsync();
			Assert.True(earable.IsConnected());
			await earable.DisconnectAsync();
			Assert.False(earable.IsConnected());
		}

		[Fact]
		public void ConnectionLossTest()
		{
			MockBLEConnection connection = new MockBLEConnection();
			ESense earable = new ESense(connection);

			var receiver = new EventReceiver<EventArgs>();
			Task receiverTask = receiver.ReceiveOne();
			Assert.False(receiverTask.IsCompleted);
			earable.ConnectionLost += receiver.OnEvent;
			connection.InvokeConnectionLostEvent(null);
			Assert.True(receiverTask.IsCompleted);
		}

		[Fact]
		public void GetSensorTest()
		{
			MockBLEConnection connection = new MockBLEConnection();
			ESense earable = new ESense(connection);

			Assert.True(earable.GetSensor<MotionSensor>() is MotionSensor);
			Assert.True(earable.GetSensor<PushButton>() is PushButton);
			Assert.True(earable.GetSensor<VoltageSensor>() is VoltageSensor);
		}

		[Fact]
		public void GuidTest()
		{
			MockBLEConnection connection = new MockBLEConnection();
			ESense earable = new ESense(connection);
			connection.Id = new System.Guid();
			Assert.Equal(connection.Id, earable.Id);
		}
	}
}
