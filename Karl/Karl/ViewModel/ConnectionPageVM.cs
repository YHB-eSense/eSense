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
		private AppLogic AppLogic;
		public ICommand RefreshDevicesCommand;
		public ICommand ConnectToDeviceCommand;
		private ObservableCollection<string> devices;

		/**Contains available Bluetooth Devices**/
		public ObservableCollection<string> Devices
		{
			get
			{
				return devices;
			}
			set
			{
				devices = value;
				OnPropertyChanged("Devices");
			}
		}

		public ConnectionPageVM(AppLogic appLogic)
		{
			AppLogic = appLogic;
			Devices = new ObservableCollection<string>();
			RefreshDevicesCommand = new Command(RefreshDevices);
			ConnectToDeviceCommand = new Command<string>(ConnectToDevice);
		}

		public void RefreshDevices() {
			ObservableCollection<string> devices = new ObservableCollection<string>();
			/*
			string[] devices = appLogic.
			for(int i = 0; i < devices.Length; i++)
			{
				deviceList.Add(devices[i]);
			}
			*/
			Devices = devices;
		}

		private void ConnectToDevice(string deviceName)
		{
			//AppLogic
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
