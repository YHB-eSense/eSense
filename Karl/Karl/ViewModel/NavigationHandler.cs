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
		static public AddSongPage AddSongPage;
		static public MainPage MainPage;

		public static void SetPages(AudioPlayerPage audioPlayerPage, AudioLibPage audioLibPage, ConnectionPage connectionPage,
			ModesPage modesPage, SettingsPage settingsPage, AddSongPage addSongPage, MainPage mainPage)
		{
			AudioPlayerPage = audioPlayerPage;
			AudioLibPage = audioLibPage;
			ConnectionPage = connectionPage;
			ModesPage = modesPage;
			SettingsPage = settingsPage;
			AddSongPage = addSongPage;
			MainPage = mainPage;
		}

		public static async void GotoAudioPlayerPage()
		{
			await Application.Current.MainPage.Navigation.PushAsync(AudioPlayerPage);
		}

		public static async void GotoAudioLibPage()
		{
			await Application.Current.MainPage.Navigation.PushAsync(AudioLibPage);
		}

		public static async void GotoConnectionPage()
		{
			await Application.Current.MainPage.Navigation.PushAsync(ConnectionPage);
		}

		public static async void GotoModesPage()
		{
			await Application.Current.MainPage.Navigation.PushAsync(ModesPage);
		}

		public static async void GotoSettingsPage()
		{
			await Application.Current.MainPage.Navigation.PushAsync(SettingsPage);
		}

		public static async void GotoAddSongPage()
		{
			await Application.Current.MainPage.Navigation.PushAsync(AddSongPage);
		}

		public static async void GotoMainPage()
		{
			await Application.Current.MainPage.Navigation.PushAsync(MainPage);
		}

		public static async void GoBack()
		{
			await Application.Current.MainPage.Navigation.PopAsync();
		}

	}
}
