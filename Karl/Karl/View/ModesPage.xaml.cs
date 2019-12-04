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
		ModesPageVM modesvm;

		public ModesPage()
		{
			InitializeComponent();
			modesvm = new ModesPageVM();
		}

		private void MM_Switch_Toggled(object sender, ToggledEventArgs e)
		{
			if(e.Value == false)
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
			if (e.Value == false)
			{
				modesvm.AutostopModeOff();
			}
			else
			{
				modesvm.AutostopModeOn();
			}
		}
	}
}
