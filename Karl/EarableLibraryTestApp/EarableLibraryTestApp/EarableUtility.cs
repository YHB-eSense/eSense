using EarableLibrary;
using System.Threading.Tasks;
using Xunit;

namespace EarableLibraryTestApp
{
	public class EarableUtility
	{
		public static async Task Reconnect(IEarable earable)
		{
			await earable.DisconnectAsync();
			Assert.True(!earable.IsConnected());
			await earable.ConnectAsync();
			Assert.True(earable.IsConnected());
		}
	}
}
