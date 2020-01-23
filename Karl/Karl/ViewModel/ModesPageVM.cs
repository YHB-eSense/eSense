using System.Windows.Input;
using Xamarin.Forms;
using Karl.Model;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Collections.Generic;
using System;
using Microcharts;

namespace Karl.ViewModel
{
	public class ModesPageVM : INotifyPropertyChanged
	{
		private SettingsHandler _settingsHandler;
		private ModeHandler _modeHandler;
		private LangManager _langManager;
		private ConnectivityHandler _connectivityHandler;

		//Eventhandling
		public event PropertyChangedEventHandler PropertyChanged;

		//Properties binded to ModesPage of View
		public CustomColor CurrentColor { get => _settingsHandler.CurrentColor; }
		public string ModesLabel { get => _langManager.CurrentLang.Get("modes"); }
		public List<Mode> Modes { get => _modeHandler.Modes; }
		public LineChart StepChart
		{
			get
			{
				if(_connectivityHandler.EarableConnected) { return new LineChart { Entries = _settingsHandler.ChartEntries }; }
				return null;
			}
		}

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
			_connectivityHandler = ConnectivityHandler.SingletonConnectivityHandler;
			ActivateModeCommand = new Command<Mode>(ActivateMode);
			_settingsHandler.SettingsChanged += Refresh;
			_connectivityHandler.ConnectionChanged += Refresh;
		}

		public void Refresh(object sender, SettingsEventArgs args)
		{
			switch (args.Value)
			{
				case nameof(_settingsHandler.CurrentLang):
					PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ModesLabel)));
					_modeHandler.ResetModes();
					PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Modes)));
					break;
				case nameof(_settingsHandler.CurrentColor):
					PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentColor)));
					PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(StepChart)));
					break;
				case nameof(_settingsHandler.ChartEntries):
					PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(StepChart)));
					break;
			}
		}

		private void Refresh(object sender, ConnectionEventArgs args)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(StepChart)));
		}

		private void ActivateMode(Mode mode)
		{
			mode.Activate();
		}

	}
}
