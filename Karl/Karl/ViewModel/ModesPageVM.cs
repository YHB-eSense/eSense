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
		public ICommand MotivationModeOnCommand;
		public ICommand MotivationModeOffCommand;
		public ICommand AutostopModeOnCommand;
		public ICommand AutostopModeOffCommand;

		public ModesPageVM(AppLogic appLogic)
		{
			AppLogic = appLogic;
			MotivationModeOnCommand = new Command(MotivationModeOn);
			MotivationModeOffCommand = new Command(MotivationModeOff);
			AutostopModeOnCommand = new Command(AutostopModeOn);
			AutostopModeOffCommand = new Command(AutostopModeOff);
		}

		public void MotivationModeOn()
		{
			//AppLogic
		}

		public void MotivationModeOff()
		{
			//AppLogic
		}

		public void AutostopModeOn()
		{
			//AppLogic
		}

		public void AutostopModeOff()
		{
			//AppLogic
		}
	}
}
