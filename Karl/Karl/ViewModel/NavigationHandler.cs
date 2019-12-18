using Karl.View;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Karl.ViewModel
{
	public static class NavigationHandler
	{
		static private ContentPage[] _pages;

		public static void SetPages(ContentPage[] pages)
		{
			_pages = pages;
		}

		public static async void GotoPage(String name)
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

		public static async void GoBack()
		{
			await Application.Current.MainPage.Navigation.PopAsync();
		}

	}
}
