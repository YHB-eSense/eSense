using EarableLibrary;
using UnitTesting.Mocks;
using Xunit;

namespace UnitTesting.EarableLibraryTests
{
	public class ESenseTest
	{
		[Fact]
		public void GuidTest()
		{
			MockBLEConnection connection = new MockBLEConnection();
			ESense earable = new ESense(connection);
			connection.Id = new System.Guid();
			Assert.Equal(connection.Id, earable.Id);
		}
	}
}
