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
		public ObservableCollection<BluetoothDevice> Devices { get => _connectivityHandler.FoundDevices; } 

		/**
		 Commands were called from Elements in Connection Page
		**/
		public ICommand RefreshDevicesCommand { get; }
		public ICommand ConnectToDeviceCommand { get; }


		/// <summary>
		/// Initializises App Logic and all available Commands
		/// </summary>
		/// <param name="appLogic"> For needed functions in Model</param>
		public ConnectionPageVM(NavigationHandler handler)
		{
			_handler = handler;
			_connectivityHandler = ConnectivityHandler.SingletonConnectivityHandler
			RefreshDevicesCommand = new Command(RefreshDevices);
			ConnectToDeviceCommand = new Command<BluetoothDevice>(ConnectToDevice);
		}

		/// <summary>
		/// Loads names from found Bluetooth Devices
		/// </summary>
		public void RefreshDevices()
		{
			ConnectivityHandler.SingletonConnectivityHandler.SearchDevices();
		}

		/// <summary>
		/// Connects to the device "IEarable"
		/// </summary>
		/// <param name="IEarable">Connect Device</param>
		private void ConnectToDevice(BluetoothDevice IEarable)
		{
			//AppLogic
			_handler.GoBack();
		} 
	}
}
