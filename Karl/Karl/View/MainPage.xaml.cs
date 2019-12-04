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
		MainPageVM mainpagevm;

		public MainPage()
		{
			InitializeComponent();
			mainpagevm = new MainPageVM();
			this.BindingContext = mainpagevm;
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
