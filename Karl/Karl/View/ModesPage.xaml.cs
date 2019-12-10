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
		private ModesPageVM ModesPageVM;

		public ModesPage(ModesPageVM modesPageVM)
		{
			InitializeComponent();
			ModesPageVM = modesPageVM;
		}

		private void MMSwitchToggled(object sender, ToggledEventArgs e)
		{
			if(e.Value)
			{
				ModesPageVM.MotivationModeOnCommand.Execute(null);
			}
			else
			{
				ModesPageVM.MotivationModeOffCommand.Execute(null);
			}
		}

		private void AMSwitchToggled(object sender, ToggledEventArgs e)
		{
			if (e.Value)
			{
				ModesPageVM.AutostopModeOnCommand.Execute(null);
			}
			else
			{
				ModesPageVM.AutostopModeOffCommand.Execute(null);
			}
		}
	}
}
