using EarableLibrary;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UnitTesting.EarableLibraryTests
{
	public class MockEarableManager : IEarableManager
	{
		public List<IEarable> Available { get; }

		public MockEarableManager()
		{
			Available = new List<IEarable>();
			Available.Add(new ESense(new MockBLEConnection()));
		}

		public async Task<IEarable> ConnectEarableAsync()
		{
			IEarable earable = ListEarables()[0];
			await earable.ConnectAsync();
			return earable;
		}

		public List<IEarable> ListEarables()
		{
			return Available;
		}
	}
}
