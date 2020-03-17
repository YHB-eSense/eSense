using EarableLibrary;
using System;
using System.Threading.Tasks;
using Xunit;

namespace EarableLibraryTestApp
{
	public class NameChangeTest : Test<IEarable>
	{
		public NameChangeTest(IStatus status) : base(status) { }

		public async override Task Run(IEarable earable)
		{
			if (!earable.IsConnected()) await earable.ConnectAsync();

			string name = "";

			for (int i = 1; i <= 30; i++)
			{
				name += ('a' + name.Length); 
				Status.StatusUpdate("Setting name with length {0}", name.Length);
				await SetName(earable, name);
			}

			/*string oldName = earable.Name;
			string newName = string.Format("ESense-{0}", DateTime.Now.Millisecond);
			Status.StatusUpdate("Old name: {0}, New name: {1}", oldName, newName);
			Assert.NotEqual(oldName, newName);

			Status.StatusUpdate("Setting new name...");
			await SetName(earable, newName);

			Status.StatusUpdate("Restoring old name...");
			await SetName(earable, oldName);*/
		}

		private async Task SetName(IEarable earable, string name)
		{
			await earable.SetNameAsync(name);
			Assert.Equal(name, earable.Name);
			await EarableUtility.Reconnect(earable);
			Assert.Equal(name, earable.Name);
		}
	}
}
