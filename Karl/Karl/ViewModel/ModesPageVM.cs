using System.Windows.Input;
using Xamarin.Forms;
using Karl.Model;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Collections.Generic;

namespace Karl.ViewModel
{
	public class ModesPageVM : INotifyPropertyChanged
	{
		private ModeHandler _modeHandler;
		private LangManager _langManager;

		/**
		 Properties binded to ModesPage of View
		**/
		public string ModesLabel { get => _langManager.CurrentLang.Get("modes"); }
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
			_langManager = LangManager.SingletonLangManager;
			ActivateModeCommand = new Command<Mode>(ActivateMode);
		}

		public void RefreshPage()
		{
			OnPropertyChanged("ModesLabel");
		}

		/// <summary>
		/// Actives mode
		/// </summary>
		/// <param name="mode">Mode to be activated</param>
		private void ActivateMode(Mode mode)
		{
			mode.Activate();
		}

		//Eventhandling

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

	}
}
