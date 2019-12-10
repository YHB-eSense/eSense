using System;
using System.Collections.Generic;
using System.Text;
using Karl.Model;

namespace Karl.ViewModel
{
	public class AudioLibPageVM
	{
		private AppLogic appLogic;

		public AudioLibPageVM(AppLogic appLogic)
		{
			this.appLogic = appLogic;
		}

		public string[] GetTitles()
		{
			return null;
		}

		public void OnTitleSelected()
		{

		}
	}
}
