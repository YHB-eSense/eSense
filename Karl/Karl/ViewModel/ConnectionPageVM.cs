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
		private AppLogic AppLogic { get; }
		public ObservableCollection<string> Devices { get; set; }
		public ICommand RefreshCommand { get; }

		public ConnectionPageVM(AppLogic appLogic)
		{
			AppLogic = appLogic;
			Devices = new ObservableCollection<string>();
			RefreshCommand = new Command(RefreshDevices);

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
			Devices = GetDevices();
			OnPropertyChanged("Devices");
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
