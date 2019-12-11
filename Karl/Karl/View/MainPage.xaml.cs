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
		private MainPageVM MainPageVM;

		public MainPage(MainPageVM mainPageVM)
		{
			InitializeComponent();
			MainPageVM = mainPageVM;
			MainPageVM.Navigation = Navigation;
			BindingContext = MainPageVM;
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();
			MainPageVM.GetDeviceName();
			MainPageVM.GetStepsAmount();
		}

		private void GotoAudioPlayerPage(object sender, EventArgs e)
		{
			MainPageVM.AudioPlayerPageCommand.Execute(Navigation);
		}

		private void GotoAudioLibPage(object sender, EventArgs e)
		{
			MainPageVM.AudioLibPageCommand.Execute(Navigation);
		}

		private void GotoConnectionPage(object sender, EventArgs e)
		{	
			MainPageVM.ConnectionPageCommand.Execute(Navigation);
		}

		private void GotoModesPage(object sender, EventArgs e)
		{
			MainPageVM.ModesPageCommand.Execute(Navigation);
		}

		private void GotoSettingsPage(object sender, EventArgs e)
		{
			MainPageVM.SettingsPageCommand.Execute(Navigation);
		}
		
	}
}
