using EarableLibrary;
using System;
using System.Threading.Tasks;

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
			await TestReconnection(earable);
		}

		private async Task TestReconnection(IEarable earable)
		{
			/*Assert.False(earable.IsConnected());

			foreach (var op in operations)
			{
				if (op == ConnectOperation.CONNECT)
				{
					Assert.NotEqual(earable.IsConnected(), await earable.ConnectAsync());
					Assert.True(earable.IsConnected());
				}
				else if (op == ConnectOperation.DISCONNECT)
				{
					Assert.Equal(earable.IsConnected(), await earable.DisconnectAsync());
					Assert.False(earable.IsConnected());
				}
			}*/
		}
	}
}
