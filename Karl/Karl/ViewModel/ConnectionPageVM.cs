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

		public ConnectionPageVM(AppLogic appLogic)
		{
			this.appLogic = appLogic;
		}

		public event PropertyChangedEventHandler PropertyChanged;

		private ObservableCollection<string> devices;

		public ObservableCollection<string> Devices
		{
			get
			{
				return devices;
			}
		}

		public ConnectionPageVM()
		{
			devices = new ObservableCollection<string>();
		}

		public void AddDevice(String name)
		{
			if(!devices.Contains(name))
			{
				devices.Add(name);
				OnPropertyChanged("Devices");
			}
		}
		public void RefreshDevices() {
			//devices.Clear();
			//load new device list
			OnPropertyChanged("Devices");
		}

		private void OnPropertyChanged(string propertyName)
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

	}
}
