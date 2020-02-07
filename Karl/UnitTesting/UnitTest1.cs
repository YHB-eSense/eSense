using Karl;
using System;
using System.Linq;
using Xunit;

namespace UnitTesting
{
    public class UnitTest1
    {
        public UnitTest1()
        {
			Tests.Xamarin.Forms.Mocks.MockForms.Init();
		}

		[Fact]
		public void ApplicationIsNotNull()
		{
			var app = new App();
			Assert.NotNull(app);
		}
	}
}
