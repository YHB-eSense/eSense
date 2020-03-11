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
			Assert.NotEqual(oldName, newName);

			// Set new name
			await earable.SetNameAsync(newName);
			Assert.Equal(earable.Name, newName);
			await earable.DisconnectAsync();
			await earable.ConnectAsync();
			Assert.Equal(earable.Name, newName);

			// Restore old name
			await earable.SetNameAsync(oldName);
			Assert.Equal(earable.Name, oldName);
			await earable.DisconnectAsync();
			await earable.ConnectAsync();
			Assert.Equal(earable.Name, oldName);
		}
	}
}
