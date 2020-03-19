using EarableLibrary;
using System.Threading.Tasks;
using Xunit;

namespace EarableLibraryTestApp
{
	public class EarableUtility
	{
		public static async Task Reconnect(IEarable earable)
		{
			Assert.True(await earable.DisconnectAsync(), "Earable is either disconnected or DisconnectAsync succeeds");
			Assert.True(!earable.IsConnected(), "Earable disconnected now");
			await Task.Delay(2000);
			Assert.True(await earable.ConnectAsync(), "ConnectAsync succeeds");
			Assert.True(earable.IsConnected(), "Earable connected after connect");
		}
	}
}
