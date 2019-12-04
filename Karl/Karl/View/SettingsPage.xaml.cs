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

		private SettingsPageVM svm;

		public SettingPage()
		{
			InitializeComponent();
			svm = new SettingsPageVM();
		}

		async void OnChangedLanguage(object sender, EventArgs args)
		{
			svm.changedLanguage("eng");
		}
		async void OnChangedDeviceName(object sender, EventArgs args)
		{
			svm.changedDeviceName("Name");
		}
		async void OnResetSteps(object sender, EventArgs args)
		{
			svm.resetSteps();
		}
	}
}
