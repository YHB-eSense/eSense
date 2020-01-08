using Karl.View;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Karl.ViewModel
{
	public class NavigationHandler
	{
		public ContentPage[] _pages { get; set; }

		public void SetPages(ContentPage[] pages)
		{
			_pages = pages;
		}

		public async void GotoPage(ContentPage page)
		{
			await Application.Current.MainPage.Navigation.PushAsync(page, true);
		}

		public async void GoBack()
		{
			await Application.Current.MainPage.Navigation.PopAsync();
		}

	}
}
