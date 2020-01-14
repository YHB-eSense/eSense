using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Xamarin.Forms;
using Karl.Model;

namespace Karl.ViewModel
{
	public class ConnectionPageVM : INotifyPropertyChanged
	{
		private SettingsHandler _settingsHandler;
		private NavigationHandler _handler;
		private ConnectivityHandler _connectivityHandler;
		private LangManager _langManager;

		//Eventhandling
		public event PropertyChangedEventHandler PropertyChanged;

		//Properties binded to ConnectionPage of View
		public ObservableCollection<EarableHandle> Devices { get => _connectivityHandler.DiscoveredDevices; }
		public string DevicesLabel { get => _langManager.CurrentLang.Get("devices"); }
		public CustomColor CurrentColor { get => _settingsHandler.CurrentColor; }

		//Commands binded to ConnectionPage of View
		public ICommand RefreshDevicesCommand { get; }
		public ICommand ConnectToDeviceCommand { get; }

		/// <summary>
		/// Initializises Commands, NavigationHandler and ConnectivityHandler of Model
		/// </summary>
		/// <param name="handler"> For navigation</param>
		public ConnectionPageVM(NavigationHandler handler)
		{
			_handler = handler;
			_settingsHandler = SettingsHandler.SingletonSettingsHandler;
			_connectivityHandler = ConnectivityHandler.SingletonConnectivityHandler;
			_langManager = LangManager.SingletonLangManager;
			RefreshDevicesCommand = new Command(RefreshDevices);
			ConnectToDeviceCommand = new Command<EarableHandle>(ConnectToDevice);
			_settingsHandler.SettingsChanged += Refresh;
		}

		public void Refresh(object sender, EventArgs args)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DevicesLabel)));
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentColor)));
		}

		private void RefreshDevices()
		{
			_connectivityHandler.SearchDevices();
		}

		private void ConnectToDevice(EarableHandle device)
		{
			_connectivityHandler.ConnectDevice(device);
			_handler.GoBack();
		}

	}
}
