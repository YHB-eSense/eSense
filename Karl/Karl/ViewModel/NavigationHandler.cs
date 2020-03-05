using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Karl.ViewModel
{
	public class NavigationHandler
	{
		private Dictionary<Type, ContentPage> _pages { get; set; }
		private static NavigationHandler _singletonNavHandler;

		public static NavigationHandler SingletonNavHandler
		{
			get
			{
				if (_singletonNavHandler == null)
				{
					_singletonNavHandler = new NavigationHandler();
					return _singletonNavHandler;
				}
				else
				{
					return _singletonNavHandler;
				}
			}
		}

		protected NavigationHandler()
		{
			_pages = new Dictionary<Type, ContentPage>();
		}

		public void SetPages(ContentPage[] pages)
		{
			foreach (var page in pages) _pages.Add(page.GetType(), page);
		}

		public async void GotoPage<T>() where T : ContentPage
		{
			await GotoPage(typeof(T));
		}

		private async Task GotoPage(Type pageType)
		{
			if (!_pages.ContainsKey(pageType)) { throw new ArgumentException("Type of page not found"); }
			var toBePushed = _pages[pageType];
			var stack = GetNavigationStackWrapper();
			foreach (var page in stack) { if (page.Equals(toBePushed)) return; } // Page already on stack; do nothing
			await PushAsyncWrapper(toBePushed);
		}

		public async void GoBack()
		{
			await GoBackWrapper();
		}

		protected virtual IReadOnlyList<Page> GetNavigationStackWrapper()
		{
			return Application.Current.MainPage.Navigation.NavigationStack;
		}

		protected virtual async Task PushAsyncWrapper(Page toBePushed)
		{
			await Application.Current.MainPage.Navigation.PushAsync(toBePushed);
		}

		protected virtual async Task GoBackWrapper()
		{
			await Application.Current.MainPage.Navigation.PopAsync();
		}
	}
}
