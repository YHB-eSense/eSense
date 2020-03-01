using Karl;
using Karl.Model;
using Karl.ViewModel;
using Moq;
using System;
using System.Linq;
using Xunit;

namespace UnitTesting
{
    public class AddSongPageVMTests
	{
        public AddSongPageVMTests()
        {
			Tests.Xamarin.Forms.Mocks.MockForms.Init();
		}

		[Fact]
		public void GetBPMCommandTest()
		{
			var vm = new AddSongPageVM(new NavigationHandler());
			var mock = new Mock<IBPMCalculator>();
			mock.Setup(x => x.Calculate()).Returns(0);
			vm.GetBPMCommand.Execute(mock.Object);
			Assert.Equal("0", vm.NewSongBPM);
		}
	}
}
