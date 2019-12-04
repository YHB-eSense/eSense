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
	public partial class ModesPage : ContentPage
	{
		ModesVM modesvm;

		public ModesPage()
		{
			InitializeComponent();
			modesvm = new ModesVM();
		}

		private void MM_Switch_Toggled(object sender, ToggledEventArgs e)
		{
			if(e.Value == true)
			{
				modesvm.MotivationModeOff();
			}
			else
			{
				modesvm.MotivationModeOn();
			}
		}

		private void AM_Switch_Toggled(object sender, ToggledEventArgs e)
		{
			if (e.Value == true)
			{
				modesvm.MotivationModeOff();
			}
			else
			{
				modesvm.MotivationModeOn();
			}
		}
	}
}
