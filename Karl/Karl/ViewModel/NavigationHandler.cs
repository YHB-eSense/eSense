using Karl.View;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Karl.ViewModel
{
	public static class NavigationHandler
	{
		static public AudioPlayerPage AudioPlayerPage;
		static public AudioLibPage AudioLibPage;
		static public ConnectionPage ConnectionPage;
		static public ModesPage ModesPage;
		static public SettingsPage SettingsPage;
		static public MainPage MainPage;

		public static void SetPages(AudioPlayerPage audioPlayerPage, AudioLibPage audioLibPage, ConnectionPage connectionPage,
			ModesPage modesPage, SettingsPage settingsPage, MainPage mainPage)
		{
			AudioPlayerPage = audioPlayerPage;
			AudioLibPage = audioLibPage;
			ConnectionPage = connectionPage;
			ModesPage = modesPage;
			SettingsPage = settingsPage;
			MainPage = mainPage;
		}

		public static void GotoAudioPlayerPage(INavigation navigation)
		{
			navigation.PushAsync(AudioPlayerPage);
		}

		public static void GotoAudioLibPage(INavigation navigation)
		{
			navigation.PushAsync(AudioLibPage);
		}

		public static void GotoConnectionPage(INavigation navigation)
		{
			navigation.PushAsync(ConnectionPage);
		}

		public static void GotoModesPage(INavigation navigation)
		{
			navigation.PushAsync(ModesPage);
		}

		public static void GotoSettingsPage(INavigation navigation)
		{
			navigation.PushAsync(SettingsPage);
		}

		public static void GotoMainPage(INavigation navigation)
		{
			navigation.PushAsync(MainPage);
		}

	}
}
