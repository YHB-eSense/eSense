using EarableLibrary;
using System.Threading.Tasks;
using Xunit;

namespace EarableLibraryTestApp
{
	public enum ConnectOperation
	{
		CONNECT,
		DISCONNECT
	}

	public class ConnectionTest : Test<IEarable>
	{
		public override async Task Run(IEarable earable)
		{
			await TestConnection(earable, ConnectOperation.CONNECT, ConnectOperation.DISCONNECT); // just connect & disconnect
			await TestConnection(earable, ConnectOperation.DISCONNECT, ConnectOperation.CONNECT, ConnectOperation.DISCONNECT, ConnectOperation.CONNECT); // reconnect
			await TestConnection(earable, ConnectOperation.DISCONNECT, ConnectOperation.DISCONNECT); // disconnect when not connected
			await TestConnection(earable, ConnectOperation.CONNECT, ConnectOperation.CONNECT); // connect when connected
		}

		private async Task TestConnection(IEarable earable, params ConnectOperation[] operations)
		{
			foreach (var op in operations)
			{
				if (op == ConnectOperation.CONNECT)
				{
					Status.StatusUpdate("Connecting...");
					Assert.NotEqual(earable.IsConnected(), await earable.ConnectAsync());
					Assert.True(earable.IsConnected());
				}
				else if (op == ConnectOperation.DISCONNECT)
				{
					Status.StatusUpdate("Disconnecting...");
					Assert.Equal(earable.IsConnected(), await earable.DisconnectAsync());
					Assert.False(earable.IsConnected());
				}
			}
		}
	}
}
