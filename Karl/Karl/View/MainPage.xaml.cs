using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Karl.View
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MainPage : ContentPage
	{
		public MainPage()
		{
			InitializeComponent();
		}

		private void AP_Button_Clicked(object sender, EventArgs e)
		{
			Navigation.PushAsync(new AudioPlayerPage());
		}
		private void AL_Button_Clicked(object sender, EventArgs e)
		{
			Navigation.PushAsync(new AudioLibPage());
		}

		private void C_Button_Clicked(object sender, EventArgs e)
		{
			Navigation.PushAsync(new ConnectionPage());
		}

		private void MM_Button_Clicked(object sender, EventArgs e)
		{
			Navigation.PushAsync(new ModesPage());
		}

		private void S_Button_Clicked(object sender, EventArgs e)
		{
			Navigation.PushAsync(new SettingsPage());
		}

	}
}
