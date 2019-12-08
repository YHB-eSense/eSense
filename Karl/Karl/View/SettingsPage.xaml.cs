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

		private SettingsPageVM settingsPageVM;

		public SettingsPage(SettingsPageVM settingsPageVM)
		{
			InitializeComponent();
			this.settingsPageVM = settingsPageVM;
		}

		public SettingsPageVM SettingsPageVM
		{
			get => default;
			set
			{
			}
		}

		public void OnChangedLanguage(object sender, EventArgs args)
		{
			settingsPageVM.changedLanguage("eng");
		}
		public void OnChangedDeviceName(object sender, EventArgs args)
		{
			settingsPageVM.changedDeviceName("Name");
		}
		public void OnResetSteps(object sender, EventArgs args)
		{
			settingsPageVM.resetSteps();
		}
	}
}
