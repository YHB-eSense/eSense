using System.Windows.Input;
using Xamarin.Forms;
using Karl.Model;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Collections.Generic;
using System;

namespace Karl.ViewModel
{
	public class ModesPageVM : INotifyPropertyChanged
	{
		private SettingsHandler _settingsHandler;
		private ModeHandler _modeHandler;
		private LangManager _langManager;

		//Eventhandling
		public event PropertyChangedEventHandler PropertyChanged;

		//Properties binded to ModesPage of View
		public CustomColor CurrentColor { get => _settingsHandler.CurrentColor; }
		public string ModesLabel { get => _langManager.CurrentLang.Get("modes"); }
		public List<Mode> Modes { get => _modeHandler.Modes; }

		//Commands binded to ModesPage of View
		public ICommand ActivateModeCommand { get; }

		/// <summary>
		/// Initializises Commands, NavigationHandler and ModeHandler of Model
		/// </summary>
		public ModesPageVM()
		{
			_modeHandler = ModeHandler.SingletonModeHandler;
			_settingsHandler = SettingsHandler.SingletonSettingsHandler;
			_langManager = LangManager.SingletonLangManager;
			ActivateModeCommand = new Command<Mode>(ActivateMode);
			_settingsHandler.SettingsChanged += Refresh;
		}

		public void Refresh(object sender, EventArgs args)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ModesLabel)));
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentColor)));
		}

		private void ActivateMode(Mode mode)
		{
			mode.Activate();
		}

	}
}
