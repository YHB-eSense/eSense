using System.Windows.Input;
using Xamarin.Forms;
using Karl.Model;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Collections.Generic;

namespace Karl.ViewModel
{
	public class ModesPageVM
	{
		private ModeHandler _modeHandler;

		/**
		 Properties binded to ModesPage of View
		**/
		public List<Mode> Modes { get => _modeHandler.Modes; }

		/**
		 Commands binded to ModesPage of View
		**/
		public ICommand ActivateModeCommand { get; }

		/// <summary>
		/// Initializises Commands, NavigationHandler and ModeHandler of Model
		/// </summary>
		public ModesPageVM()
		{
			_modeHandler = ModeHandler.SingletonModeHandler;
			ActivateModeCommand = new Command<Mode>(ActivateMode);
		}

		/// <summary>
		/// Actives mode
		/// </summary>
		/// <param name="mode">Mode to be activated</param>
		private void ActivateMode(Mode mode)
		{
			mode.Activate();
		}

	}
}
