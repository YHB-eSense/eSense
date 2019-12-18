using Karl.View;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Karl.ViewModel
{
	public class NavigationHandler
	{
		private ContentPage[] _pages;

		public void SetPages(ContentPage[] pages)
		{
			_pages = pages;
		}

		public async void GotoPage(String name)
		{
			for (int i = 0; i < _pages.Length; i++)
			{
				if (_pages[i].GetType().Name == name)
				{
					await Application.Current.MainPage.Navigation.PushAsync(_pages[i]);
					return;
				}
			}
			throw new Exception("Page not found!");
			
		}

		public async void GoBack()
		{
			await Application.Current.MainPage.Navigation.PopAsync();
		}

	}
}
