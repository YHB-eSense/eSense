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
		private ConnectivityHandler _connectivityHandler;

		//Eventhandling
		public event PropertyChangedEventHandler PropertyChanged;

		//Properties binded to ModesPage of View
		public CustomColor CurrentColor { get => _settingsHandler.CurrentColor; }
		public string ModesLabel { get => _settingsHandler.CurrentLang.Get("modes"); }
		public List<Mode> Modes { get => _modeHandler.Modes; }
		public LineChart StepChart
		{		
			get
			{
				if(_connectivityHandler.EarableConnected) { return new LineChart { Entries = _settingsHandler.ChartEntries }; }
				return null;
			}
		}

		/// <summary>
		/// Initializises Commands, NavigationHandler and ModeHandler of Model
		/// </summary>
		public ModesPageVM()
		{
			_modeHandler = ModeHandler.SingletonModeHandler;
			_settingsHandler = SettingsHandler.SingletonSettingsHandler;
			_connectivityHandler = ConnectivityHandler.SingletonConnectivityHandler;
			_settingsHandler.LangChanged += RefreshLang;
			_settingsHandler.ColorChanged += RefreshColor;
			_settingsHandler.ChartChanged += RefreshChart;
			_connectivityHandler.ConnectionChanged += RefreshConnection;
		}

		private void RefreshLang(object sender, EventArgs args)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ModesLabel)));
			_modeHandler.ResetModes();
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Modes)));
		}

		private void RefreshColor(object sender, EventArgs args)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentColor)));
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(StepChart)));
		}

		private void RefreshChart(object sender, EventArgs args)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(StepChart)));
		}

		private void RefreshConnection(object sender, EventArgs args)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(StepChart)));
		}

	}
}
