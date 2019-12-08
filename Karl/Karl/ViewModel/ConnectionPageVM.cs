using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Collections.ObjectModel;
using Karl.Model;

namespace Karl.ViewModel
{
	public class ConnectionPageVM : INotifyPropertyChanged
	{
		private AppLogic appLogic;
		private ObservableCollection<string> devices;

		public ObservableCollection<string> Devices
		{
			get
			{
				return devices;
			}
		}

		public ConnectionPageVM(AppLogic appLogic)
		{
			devices = new ObservableCollection<string>();
			this.appLogic = appLogic;
		}

		private ObservableCollection<string> GetDevices()
		{
			ObservableCollection<string> deviceList = new ObservableCollection<string>();
			/*
			string[] devices = appLogic.
			for(int i = 0; i < devices.Length; i++)
			{
				deviceList.Add(devices[i]);
			}
			*/
			return deviceList;
		}

		public void RefreshDevices() {
			devices = GetDevices();
			OnPropertyChanged("Devices");
		}

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChanged(string propertyName)
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		public AppLogic AppLogic
		{
			get => default;
			set
			{
			}
		}
	}
}
