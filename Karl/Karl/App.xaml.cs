using Xamarin.Forms;
using Karl.View;
using Karl.ViewModel;
using FormsControls.Base;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System.Diagnostics;
using Karl.Model;

namespace Karl
{
	public partial class App : Application
	{
		public App()
		{
			InitializeComponent();
		
			NavigationHandler handler = new NavigationHandler();

			AudioPlayerPageVM audioPlayerPageVM = new AudioPlayerPageVM();
			AudioLibPageVM audioLibPageVM = new AudioLibPageVM(handler);
			ModesPageVM modesPageVM = new ModesPageVM();
			SettingsPageVM settingsPageVM = new SettingsPageVM();
			AddSongPageVM addSongPageVM = new AddSongPageVM(handler);
			MainPageVM mainPageVM = new MainPageVM(handler);

			AudioPlayerPage audioPlayerPage = new AudioPlayerPage(audioPlayerPageVM);
			AudioLibPage audioLibPage = new AudioLibPage(audioLibPageVM);
			ModesPage modesPage = new ModesPage(modesPageVM);
			SettingsPage settingsPage = new SettingsPage(settingsPageVM);
			AddSongPage addSongPage = new AddSongPage(addSongPageVM);
			MainPage mainPage = new MainPage(mainPageVM);

			ContentPage[] pages = {audioPlayerPage, audioLibPage, modesPage, settingsPage, addSongPage, mainPage};
			handler.SetPages(pages);

			MainPage = new AnimationNavigationPage(mainPage);

			GetPermissions();
		}

		protected override void OnStart()
		{
			// Handle when your app starts
		}

		protected override void OnSleep()
		{
			// Handle when your app sleeps
			Debug.WriteLine("OnSleep");
		}

		protected override void OnResume()
		{
			Debug.WriteLine("OnResume");
			ConnectivityHandler.SingletonConnectivityHandler.RefreshAfterSleep();
			AudioPlayer.SingletonAudioPlayer.RefreshAfterSleep();
		}

		private async void GetPermissions()
		{
			var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Storage);
			if (status != PermissionStatus.Granted)
			{
				await CrossPermissions.Current.RequestPermissionsAsync(Permission.Storage);
			}
		}

	}
}
