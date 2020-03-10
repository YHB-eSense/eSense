using Karl.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xamarin.Forms;
using Karl.View;
using Moq;

namespace UnitTesting.ViewModelTests
{
	public class NavigationHandlerTests
	{

		[Fact]
		public void SetPagesTest() { }

		[Fact]
		public void GotoPageTest()
		{
			//setup
			var handler = new NavigationHandler_NEW();
			var page1 = new ContentPageMock1();
			var page2 = new ContentPageMock2();
			var pages = new ContentPage[] { page1, page2 };
			handler.SetPages(pages);
			//test
			handler.GotoPage<ContentPageMock1>();
			Assert.Equal(page1, handler.List.Last<Page>());
		}

		[Fact]
		public void GoBackTest()
		{
			//setup
			var handler = new NavigationHandler_NEW();
			var page1 = new ContentPageMock1();
			var page2 = new ContentPageMock2();
			var pages = new ContentPage[] { page1, page2 };
			handler.SetPages(pages);
			handler.GotoPage<ContentPageMock1>();
			handler.GotoPage<ContentPageMock2>();
			//test
			handler.GoBack();
			Assert.Equal(page1, handler.List.Last<Page>());
		}

		internal class NavigationHandler_NEW : NavigationHandler
		{
			public List<Page> List;
			public NavigationHandler_NEW()
			{
				List = new List<Page>();
			}
			protected override IReadOnlyList<Page> GetNavigationStackWrapper()
			{
				return List;
			}
			protected override async Task PushAsyncWrapper(Page toBePushed)
			{
				List.Add(toBePushed);
			}
			protected override async Task GoBackWrapper()
			{
				if (List.Count != 0)
				{
					List.RemoveAt(List.Count - 1);
				}
			}
		}

		internal class ContentPageMock1 : ContentPage { }

		internal class ContentPageMock2 : ContentPage { }

	}
}
