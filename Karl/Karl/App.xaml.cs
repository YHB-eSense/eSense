using Xamarin.Forms;
using Karl.View;
using Karl.ViewModel;
using FormsControls.Base;
using Karl.Model;
using System.Threading.Tasks;

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
