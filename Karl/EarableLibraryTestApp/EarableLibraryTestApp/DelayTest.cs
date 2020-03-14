using EarableLibrary;
using System.Threading.Tasks;
using Xunit;

namespace EarableLibraryTestApp
{
	internal class DelayTest : Test<IEarable>
	{
		public async override Task Run(IEarable earable)
		{
			if (!earable.IsConnected()) await earable.ConnectAsync();
			Assert.True(earable.IsConnected(), "Earable is connected before delay");
			await Task.Delay(500);
			Assert.True(earable.IsConnected(), "Earable is connected after delay");
		}

		internal class Reconnect : Test<IEarable>
		{
			public async override Task Run(IEarable earable)
			{
				await earable.DisconnectAsync();
				Assert.False(earable.IsConnected());
				await earable.ConnectAsync();
				Assert.True(earable.IsConnected());
			}
		}
	}
}
