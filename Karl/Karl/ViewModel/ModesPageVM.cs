using System.Windows.Input;
using Xamarin.Forms;
using Karl.Model;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;

namespace Karl.ViewModel
{
	public class ModesPageVM
	{
		private ModeHandler _modeHandler;

		/**
		 Properties binded to ModesPage of View
		**/
		public ObservableCollection<Mode> Modes { get; set; }

		/**
		 Commands binded to ModesPage of View
		**/
		public ICommand ActivateModeCommand { get; }

		/// <summary>
		/// Initializises Commands, NavigationHandler and ModeHandler of Model
		/// </summary>
		/// <param name="handler">For navigation</param>
		public ModesPageVM()
		{
			_modeHandler = ModeHandler.SingletonModeHandler;
			ActivateModeCommand = new Command<Mode>(ActivateMode);
			Modes = new ObservableCollection<Mode>();
		}

		/// <summary>
		/// Actives mode
		/// </summary>
		/// <param name="mode">Mode to be activated</param>
		private void ActivateMode(Mode mode)
		{
			mode.Activate();
		}

		/// <summary>
		/// Loads Modes
		/// </summary>
		public void GetModes()
		{
			ObservableCollection<Mode> modes = new ObservableCollection<Mode>();
			foreach (Mode mode in _modeHandler.Modes)
			{
				modes.Add(mode);
			}
			Modes = modes;
		}
	}
}
