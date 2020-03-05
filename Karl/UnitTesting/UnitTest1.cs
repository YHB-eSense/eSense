using Karl;
using System;
using System.Linq;
using Xunit;
using Karl.ViewModel;

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

		[Fact]
		public void AddSongResets()
		{
			NavigationHandler handler = new NavigationHandler();
			AddSongPageVM addSongPageVM = new AddSongPageVM(handler);
			addSongPageVM.AddSongCommand.Execute(null);
			Assert.Null(addSongPageVM.NewSongArtist);
			Assert.Null(addSongPageVM.NewSongBPM);
			Assert.Null(addSongPageVM.NewSongTitle);
		}
	}
}
