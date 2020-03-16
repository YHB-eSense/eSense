using EarableLibraryTestApp;
using System.Threading.Tasks;
using UnitTesting.Mocks;
using Xunit;

namespace UnitTesting.EarableLibraryTests
{
	public class TestAppTest
	{
		[Fact]
		public async Task TestTheTest()
		{
			TestRunner runner = new TestRunner(new MockEarableManager(), new DebugStatus());
			await runner.RunTestsAsync();
		}
	}
}
