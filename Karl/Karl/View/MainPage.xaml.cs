using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Karl.ViewModel;

namespace Karl.View
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MainPage : ContentPage
	{
		private MainPageVM mainPageVM;
		private AudioPlayerPage audioPlayerPage;
		private AudioLibPage audiolLibPage;
		private ConnectionPage connectionPage;
		private ModesPage modesPage;
		private SettingsPage settingsPage;

		public MainPage()
		{
			InitializeComponent();
			mainPageVM = new MainPageVM();
			audioPlayerPage = new AudioPlayerPage();
			audiolLibPage = new AudioLibPage();
			connectionPage = new ConnectionPage();
			modesPage = new ModesPage();
			settingsPage = new SettingsPage();
			this.BindingContext = mainPageVM;
		}

		private void AP_Button_Clicked(object sender, EventArgs e)
		{
			Navigation.PushAsync(audioPlayerPage);
		}

		private void AL_Button_Clicked(object sender, EventArgs e)
		{
			Navigation.PushAsync(audiolLibPage);
		}

		private void C_Button_Clicked(object sender, EventArgs e)
		{
			Navigation.PushAsync(connectionPage);
		}

		private void MM_Button_Clicked(object sender, EventArgs e)
		{
			Navigation.PushAsync(modesPage);
		}

		private void S_Button_Clicked(object sender, EventArgs e)
		{
			Navigation.PushAsync(settingsPage);
		}

	}
}
