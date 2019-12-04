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

		public SettingsPage()
		{
			InitializeComponent();
			svm = new SettingsPageVM();
		}

		public void OnChangedLanguage(object sender, EventArgs args)
		{
			svm.changedLanguage("eng");
		}
		public void OnChangedDeviceName(object sender, EventArgs args)
		{
			svm.changedDeviceName("Name");
		}
		public void OnResetSteps(object sender, EventArgs args)
		{
			svm.resetSteps();
		}
	}
}
