using Karl.View;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Karl.ViewModel
{
	public static class NavigationHandler
	{
		static private AudioPlayerPage _audioPlayerPage;
		static private AudioLibPage _audioLibPage;
		static private ConnectionPage _connectionPage;
		static private ModesPage _modesPage;
		static private SettingsPage _settingsPage;
		static private AddSongPage _addSongPage;
		static private MainPage _mainPage;

		public static void SetPages(AudioPlayerPage audioPlayerPage, AudioLibPage audioLibPage, ConnectionPage connectionPage,
			ModesPage modesPage, SettingsPage settingsPage, AddSongPage addSongPage, MainPage mainPage)
		{
			_audioPlayerPage = audioPlayerPage;
			_audioLibPage = audioLibPage;
			_connectionPage = connectionPage;
			_modesPage = modesPage;
			_settingsPage = settingsPage;
			_addSongPage = addSongPage;
			_mainPage = mainPage;
		}

		public static async void GotoAudioPlayerPage()
		{
			await Application.Current.MainPage.Navigation.PushAsync(_audioPlayerPage);
		}

		public static async void GotoAudioLibPage()
		{
			await Application.Current.MainPage.Navigation.PushAsync(_audioLibPage);
		}

		public static async void GotoConnectionPage()
		{
			await Application.Current.MainPage.Navigation.PushAsync(_connectionPage);
		}

		public static async void GotoModesPage()
		{
			await Application.Current.MainPage.Navigation.PushAsync(_modesPage);
		}

		public static async void GotoSettingsPage()
		{
			await Application.Current.MainPage.Navigation.PushAsync(_settingsPage);
		}

		public static async void GotoAddSongPage()
		{
			await Application.Current.MainPage.Navigation.PushAsync(_addSongPage);
		}

		public static async void GotoMainPage()
		{
			await Application.Current.MainPage.Navigation.PushAsync(_mainPage);
		}

		public static async void GoBack()
		{
			await Application.Current.MainPage.Navigation.PopAsync();
		}

	}
}
