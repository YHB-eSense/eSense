using EarableLibrary;
using System;
using System.Threading.Tasks;
using Xunit;

namespace EarableLibraryTestApp
{
	public class NameChangeTest : Test<IEarable>
	{
		public async override Task Run(IEarable earable)
		{
			if (!earable.IsConnected()) await earable.ConnectAsync();

			string oldName = earable.Name;
			string newName = DateTime.Now.Ticks.ToString();
			Status.StatusUpdate("Old name: {0}, New name: {1}", oldName, newName);
			Assert.NotEqual(oldName, newName);

			Status.StatusUpdate("Setting new name...");
			await SetName(earable, newName);

			Status.StatusUpdate("Restoring old name...");
			await SetName(earable, oldName);
		}

		private async Task SetName(IEarable earable, string name)
		{
			await earable.SetNameAsync(name);
			Assert.Equal(earable.Name, name);
			await EarableUtility.Reconnect(earable);
			Assert.Equal(earable.Name, name);
		}
	}
}
