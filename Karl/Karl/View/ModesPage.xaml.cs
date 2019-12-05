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
		private ModesPageVM modesPageVM;

		public ModesPage(ModesPageVM modesPageVM)
		{
			InitializeComponent();
			this.modesPageVM = modesPageVM;
		}

		private void MM_Switch_Toggled(object sender, ToggledEventArgs e)
		{
			if(e.Value == false)
			{
				modesPageVM.MotivationModeOff();
			}
			else
			{
				modesPageVM.MotivationModeOn();
			}
		}

		private void AM_Switch_Toggled(object sender, ToggledEventArgs e)
		{
			if (e.Value == false)
			{
				modesPageVM.AutostopModeOff();
			}
			else
			{
				modesPageVM.AutostopModeOn();
			}
		}
	}
}
