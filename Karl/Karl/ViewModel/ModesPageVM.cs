using Karl.Model;
using Microcharts;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Karl.ViewModel
{
	public class ModesPageVM : INotifyPropertyChanged
	{
		private SettingsHandler _settingsHandler;
		private ModeHandler _modeHandler;
		protected ConnectivityHandler _connectivityHandler;

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
				if (_connectivityHandler.EarableConnected) { return new LineChart { Entries = _settingsHandler.ChartEntries }; }
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

		[DoNotCover]
		protected virtual void InitializeSingletons()
		{
			_connectivityHandler = ConnectivityHandler.SingletonConnectivityHandler;
		}

	}
}
