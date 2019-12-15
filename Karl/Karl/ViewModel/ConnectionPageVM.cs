using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Xamarin.Forms;
using Karl.Model;
using EarableLibrary;

namespace Karl.ViewModel
{
	public class ConnectionPageVM : INotifyPropertyChanged
	{
		private AppLogic _appLogic;
		public ObservableCollection<IEarable> Devices { get; }

		/**
		 Commands were called from Elements in Connection Page
		**/
		public ICommand RefreshDevicesCommand { get; }
		public ICommand ConnectToDeviceCommand { get; }


		/// <summary>
		/// Initializises App Logic and all available Commands
		/// </summary>
		/// <param name="appLogic"> For needed functions in Model</param>
		public ConnectionPageVM(AppLogic appLogic)
		{
			_appLogic = appLogic;
			Devices = new ObservableCollection<IEarable>();
			RefreshDevicesCommand = new Command(RefreshDevices);
			ConnectToDeviceCommand = new Command<IEarable>(ConnectToDevice);
		}

		/// <summary>
		/// Loads names from found Bluetooth Devices
		/// </summary>
		public void RefreshDevices()
		{
			Devices.Clear();
			EarableConnectionHandler handler = new EarableConnectionHandler();
			handler.EarableDiscovered += (s, e) =>
			{
				Devices.Add(e.Earable);
				OnPropertyChanged("Devices");
			};
			handler.StartScanning();
		}

		/// <summary>
		/// Connects to the device "IEarable"
		/// </summary>
		/// <param name="IEarable">Connect Device</param>
		private void ConnectToDevice(IEarable IEarable)
		{
			//AppLogic
			NavigationHandler.GoBack();
		} 

		//Eventhandling

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChanged(string propertyName)
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
}
