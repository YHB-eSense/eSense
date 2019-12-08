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

		public MainPage(AudioPlayerPage audioPlayerPage, AudioLibPage audiolLibPage, ConnectionPage connectionPage,
			ModesPage modesPage, SettingsPage settingsPage, MainPageVM mainPageVM)
		{
			InitializeComponent();
			this.audioPlayerPage = audioPlayerPage;
			this.audiolLibPage = audiolLibPage;
			this.connectionPage = connectionPage;
			this.modesPage = modesPage;
			this.settingsPage = settingsPage;
			this.mainPageVM = mainPageVM;
			this.BindingContext = this. mainPageVM;
		}

		public AudioLibPage AudioLibPage
		{
			get => default;
			set
			{
			}
		}

		public AudioPlayerPage AudioPlayerPage
		{
			get => default;
			set
			{
			}
		}

		public ConnectionPage ConnectionPage
		{
			get => default;
			set
			{
			}
		}

		public ModesPage ModesPage
		{
			get => default;
			set
			{
			}
		}

		public SettingsPage SettingsPage
		{
			get => default;
			set
			{
			}
		}

		public MainPageVM MainPageVM
		{
			get => default;
			set
			{
			}
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
