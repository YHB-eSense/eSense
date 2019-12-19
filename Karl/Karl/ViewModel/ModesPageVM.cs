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
		/// <summary>
		/// Contains all available modes of the App
		/// </summary>
		public ObservableCollection<Mode> Modes { get; set; }

		public ICommand ActivateModeCommand { get; }

		/// <summary>
		/// Initializises App Logic and all available Commands
		/// </summary>
		/// <param name="appLogic"> For needed functions in Model</param>
		public ModesPageVM()
		{
			_modeHandler = ModeHandler.SingletonModeHandler;
			ActivateModeCommand = new Command<Mode>(ActivateMode);
			Modes = new ObservableCollection<Mode>();
		}

		/// <summary>
		/// Actives the Mode "mode"
		/// </summary>
		/// <param name="mode">Activated Mode</param>
		private void ActivateMode(Mode mode)
		{
			//AppLogic
		}

		/// <summary>
		/// Loads Modes from App Logic
		/// </summary>
		public void GetModes()
		{
			//Modes = new ObservableCollection<Mode>(_appLogic.ModeHandler.Modes);
			foreach (var data in Modes) Debug.WriteLine(data.Name);
		}
	}
}
