using System;
using System.Collections.Generic;
using System.Text;
using Karl.Model;

namespace Karl.ViewModel
{
	public class SettingsPageVM
	{
		private AppLogic appLogic;

		public SettingsPageVM(AppLogic appLogic)
		{
			this.appLogic = appLogic;
		}

		public AppLogic AppLogic
		{
			get => default;
			set
			{
			}
		}

		public void changedDeviceName(String deviceName) {

		}

		public void changedLanguage(String language)
		{

		}

		public void resetSteps()
		{

		}

	}
}
