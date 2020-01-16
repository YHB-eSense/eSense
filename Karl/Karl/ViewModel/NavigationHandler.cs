using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Karl.ViewModel
{
	public class NavigationHandler
	{
		private Dictionary<Type, ContentPage> Pages { get; set; }

		public NavigationHandler()
		{
			Pages = new Dictionary<Type, ContentPage>();
		}

		public void SetPages(ContentPage[] pages)
		{
			foreach (var page in pages) Pages.Add(page.GetType(), page);
		}

		public async void GotoPage<T>() where T : ContentPage
		{
			await GotoPage(typeof(T));
		}

		private async Task GotoPage(Type pageType)
		{
			if (!Pages.ContainsKey(pageType))
			{
				throw new ArgumentException("NavigationHandler does not know of any page with that type");
			}
			var toBePushed = Pages[pageType];
			var stack = Application.Current.MainPage.Navigation.NavigationStack;
			foreach (var page in stack)
			{
				if (page.Equals(toBePushed)) return; // Page already on stack; do nothing
			}
			await Application.Current.MainPage.Navigation.PushAsync(toBePushed);
		}

		public async void GoBack()
		{
			await Application.Current.MainPage.Navigation.PopAsync();
		}

	}
}
