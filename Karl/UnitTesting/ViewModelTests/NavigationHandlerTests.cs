using Karl.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xamarin.Forms;

namespace UnitTesting.ViewModelTests
{
	public class NavigationHandlerTests
	{
		[Fact]
		public void SetPagesTest()
		{
			NavigationHandler_NEW handler1 = new NavigationHandler_NEW();
			ContentPage page1 = new ContentPage();
			ContentPage page2 = new ContentPage();
			ContentPage[] pages = new ContentPage[] { page1, page2 };
			handler1.SetPages(pages);
		}

		[Fact]
		public void GotoPageTest()
		{
			NavigationHandler_NEW handler1 = new NavigationHandler_NEW();
			ContentPage page1 = new ContentPage();
			ContentPage page2 = new ContentPage();
			ContentPage[] pages = new ContentPage[] { page1, page2 };
			handler1.SetPages(pages);
			//TODO
		}

		[Fact]
		public void GoBackTest()
		{
			NavigationHandler_NEW handler1 = new NavigationHandler_NEW();
			ContentPage page1 = new ContentPage();
			ContentPage page2 = new ContentPage();
			ContentPage[] pages = new ContentPage[] { page1, page2 };
			handler1.SetPages(pages);
			//TODO
		}

		internal class NavigationHandler_NEW : NavigationHandler
		{
			private List<Page> _list;
			public NavigationHandler_NEW()
			{
				_list = new List<Page>();
			}
			protected override IReadOnlyList<Page> GetNavigationStackWrapper()
			{
				return _list;
			}
			protected override Task PushAsyncWrapper(Page toBePushed)
			{
				_list.Add(toBePushed);
				return new Task(null);
			}
			protected override Task GoBackWrapper()
			{
				if (_list.Count != 0)
				{
					_list.RemoveAt(_list.Count - 1);
				}
				return new Task(null);
			}
		}

	}
}
