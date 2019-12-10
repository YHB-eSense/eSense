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
	public partial class SettingsPage : ContentPage
	{

		private SettingsPageVM SettingsPageVM;

		public SettingsPage(SettingsPageVM settingsPageVM)
		{
			InitializeComponent();
			SettingsPageVM = settingsPageVM;
			BindingContext = SettingsPageVM;
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();
			SettingsPageVM.GetLanguages();
			SettingsPageVM.GetSelectedLanguage();
			SettingsPageVM.GetDeviceName();
		}

	}
}
