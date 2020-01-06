using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using Karl.Model;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Karl.ViewModel
{
	public class SettingsPageVM : INotifyPropertyChanged
	{
		private SettingsHandler _settingsHandler;
		private string _deviceName;

		/**
		 Properties binded to SettingsPage of View
		**/
		public List<Lang> Languages { get => _settingsHandler.Languages; }

		public Lang SelectedLanguage
		{
			get => _settingsHandler.CurrentLang;
			set
			{
				_settingsHandler.CurrentLang = value;
				OnPropertyChanged("SelectedLanguage");
			}
		}

		public string DeviceName
		{
			get => _settingsHandler.DeviceName;
			set
			{
				_deviceName = value;
			}
		}

		/**
		 Commands binded to SettingsPage of View
		**/
		public ICommand ChangeDeviceNameCommand { get; }
		public ICommand ResetStepsCommand { get; }

		/// <summary>
		/// Initializises Commands and SettingsHandler of Model
		/// </summary>
		public SettingsPageVM()
		{
			_settingsHandler = SettingsHandler.SingletonSettingsHandler;
			ChangeDeviceNameCommand = new Command(ChangeDeviceName);
			ResetStepsCommand = new Command(ResetSteps);
		}

		private void ChangeDeviceName()
		{
			_settingsHandler.DeviceName = _deviceName;
		}

		private void ResetSteps()
		{
			_settingsHandler.ResetSteps();
		}

		//Eventhandling

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

	}
}
