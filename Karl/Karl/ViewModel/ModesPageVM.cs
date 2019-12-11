using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using Karl.Model;

namespace Karl.ViewModel
{
	public class ModesPageVM
	{
		private AppLogic AppLogic;
		public ICommand MotivationModeCommand;
		public ICommand AutostopModeCommand;
		
		public ModesPageVM(AppLogic appLogic)
		{
			AppLogic = appLogic;
			MotivationModeCommand = new Command<bool>(MotivationMode);
			AutostopModeCommand = new Command<bool>(AutostopMode);
		}

		public void MotivationMode(bool value)
		{
			//AppLogic
		}


		public void AutostopMode(bool value)
		{
			//AppLogic
		}

	}
}
