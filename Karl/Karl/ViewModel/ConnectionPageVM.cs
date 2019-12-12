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
		private ObservableCollection<BluetoothDevice> devices;

		/**Contains available Bluetooth Devices**/
		public ObservableCollection<BluetoothDevice> Devices
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

		public ICommand RefreshDevicesCommand { get; }
		public ICommand ConnectToDeviceCommand { get; }

		public ConnectionPageVM(AppLogic appLogic)
		{
			AppLogic = appLogic;
			Devices = new ObservableCollection<BluetoothDevice>();
			RefreshDevicesCommand = new Command(RefreshDevices);
			ConnectToDeviceCommand = new Command<BluetoothDevice>(ConnectToDevice);
		}

		public void RefreshDevices()
		{
			ObservableCollection<BluetoothDevice> devices = new ObservableCollection<BluetoothDevice>();
			/*
			string[] devices = appLogic.
			for(int i = 0; i < devices.Length; i++)
			{
				deviceList.Add(devices[i]);
			}
			*/
			devices.Add(new BluetoothDevice("Ear1"));
			devices.Add(new BluetoothDevice("Ear2"));
			Devices = devices;
		}

		private void ConnectToDevice(BluetoothDevice bluetoothDevice)
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
