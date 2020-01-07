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

		private SettingsPageVM _settingsPageVM;

		public SettingsPage(SettingsPageVM settingsPageVM)
		{
			InitializeComponent();
			_settingsPageVM = settingsPageVM;
			BindingContext = _settingsPageVM;
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();
			_settingsPageVM.RefreshPage();
		}

	}
}
