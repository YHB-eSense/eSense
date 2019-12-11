using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Karl.View;
using Karl.ViewModel;
using Karl.Model;

namespace Karl
{
    public partial class App : Application
    {
		public App()
        {
            InitializeComponent();

			AppLogic appLogic = new AppLogic();

			AudioPlayerPageVM audioPlayerPageVM = new AudioPlayerPageVM(appLogic);
			AudioLibPageVM audioLibPageVM = new AudioLibPageVM(appLogic);
			ConnectionPageVM connectionPageVM = new ConnectionPageVM(appLogic);
			ModesPageVM modesPageVM = new ModesPageVM(appLogic);
			SettingsPageVM settingsPageVM = new SettingsPageVM(appLogic);
			MainPageVM mainPageVM = new MainPageVM(appLogic);
			AudioPlayerPage audioPlayerPage = new AudioPlayerPage(audioPlayerPageVM);
			AudioLibPage audioLibPage = new AudioLibPage(audioLibPageVM);
			ConnectionPage connectionPage = new ConnectionPage(connectionPageVM);
			ModesPage modesPage = new ModesPage(modesPageVM);
			SettingsPage settingsPage = new SettingsPage(settingsPageVM);
			MainPage mainPage = new MainPage(mainPageVM);

			NavigationHandler.SetPages(audioPlayerPage, audioLibPage, connectionPage, modesPage, settingsPage, mainPage);

			MainPage = new NavigationPage(mainPage);
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
