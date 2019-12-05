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
		private AppLogic appLogic;

		private MainPageVM mainPageVM;
		private AudioPlayerPageVM audioPlayerPageVM;
		private AudioLibPageVM audioLibPageVM;
		private ConnectionPageVM connectionPageVM;
		private ModesPageVM modesPageVM;
		private SettingsPageVM settingsPageVM;

		private MainPage mainPage;
		private AudioPlayerPage audioPlayerPage;
		private AudioLibPage audioLibPage;
		private ConnectionPage connectionPage;
		private ModesPage modesPage;
		private SettingsPage settingsPage;

		public App()
        {
            InitializeComponent();

			appLogic = new AppLogic();

			mainPageVM = new MainPageVM(appLogic);
			audioPlayerPageVM = new AudioPlayerPageVM(appLogic);
			audioLibPageVM = new AudioLibPageVM(appLogic);
			connectionPageVM = new ConnectionPageVM(appLogic);
			modesPageVM = new ModesPageVM(appLogic);
			settingsPageVM = new SettingsPageVM(appLogic);

			audioPlayerPage = new AudioPlayerPage(audioPlayerPageVM);
			audioLibPage = new AudioLibPage(audioLibPageVM);
			connectionPage = new ConnectionPage(connectionPageVM);
			modesPage = new ModesPage(modesPageVM);
			settingsPage = new SettingsPage(settingsPageVM);

			mainPage = new MainPage(audioPlayerPage, audioLibPage, connectionPage, modesPage, settingsPage, mainPageVM);

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
