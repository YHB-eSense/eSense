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
		private NavigationHandler _handler;
		private ConnectivityHandler _connectivityHandler;
		private LangManager _langManager;
		public ObservableCollection<EarableHandle> Devices { get => _connectivityHandler.DiscoveredDevices; }

		public string DevicesLabel { get => _langManager.CurrentLang.Get("devices"); }

		/**
		 Commands binded to ConnectionPage of View
		**/
		public ICommand RefreshDevicesCommand { get; }
		public ICommand ConnectToDeviceCommand { get; }

		/// <summary>
		/// Initializises Commands, NavigationHandler and ConnectivityHandler of Model
		/// </summary>
		/// <param name="handler"> For navigation</param>
		public ConnectionPageVM(NavigationHandler handler)
		{
			_handler = handler;
			_connectivityHandler = ConnectivityHandler.SingletonConnectivityHandler;
			_langManager = LangManager.SingletonLangManager;
			RefreshDevicesCommand = new Command(RefreshDevices);
			ConnectToDeviceCommand = new Command<EarableHandle>(ConnectToDevice);
		}

		public void RefreshPage()
		{
			OnPropertyChanged("DevicesLabel");
			RefreshDevices();
		}

		private void RefreshDevices()
		{
			_connectivityHandler.SearchDevices();
		}

		/// <summary>
		/// Connects to device
		/// </summary>
		/// <param name="device">Selected device to connect to</param>
		private void ConnectToDevice(EarableHandle device)
		{
			_connectivityHandler.ConnectDevice(device);
			_handler.GoBack();
		}

		//Eventhandling

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
