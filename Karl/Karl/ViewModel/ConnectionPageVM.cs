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
	public class ConnectionPageVM
	{
		private NavigationHandler _handler;
		private ConnectivityHandler _connectivityHandler;
		public ObservableCollection<EarableHandle> Devices { get => _connectivityHandler.DiscoveredDevices; }

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
			RefreshDevicesCommand = new Command(RefreshDevices);
			ConnectToDeviceCommand = new Command<EarableHandle>(ConnectToDevice);
		}

		/// <summary>
		/// Refreshes the search for devices
		/// </summary>
		public void RefreshDevices()
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
	}
}
