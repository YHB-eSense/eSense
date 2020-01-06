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
		private Language _selectedLanguage;
		private string _deviceName;

		/**
		 Properties binded to SettingsPage of View
		**/
		public ObservableCollection<Language> Languages { get; set; }

		public Language SelectedLanguage
		{
			get
			{
				return _selectedLanguage;
			}
			set
			{
				if (!_selectedLanguage.Equals(value))
				{
					_selectedLanguage = value;
					ChangeLanguage(_selectedLanguage);
					OnPropertyChanged("SelectedLanguage");
				}
			}
		}

		public string DeviceName
		{
			get
			{
				return _deviceName;
			}
			set
			{
				if (_deviceName != value)
				{
					_deviceName = value;
					OnPropertyChanged("DeviceName");
				}
			}
		}

		/**
		 Commands binded to SettingsPage of View
		**/
		public ICommand ChangeDeviceNameCommand { get; }
		public ICommand ChangeLanguageCommand { get; }
		public ICommand ResetStepsCommand { get; }

		/// <summary>
		/// Initializises Commands and SettingsHandler of Model
		/// </summary>
		public SettingsPageVM()
		{
			_settingsHandler = SettingsHandler.SingletonSettingsHandler;
			ChangeDeviceNameCommand = new Command<String>(ChangeDeviceName);
			ChangeLanguageCommand = new Command<Language>(ChangeLanguage);
			ResetStepsCommand = new Command(ResetSteps);
		}

		private void ChangeDeviceName(String deviceName)
		{
			_settingsHandler.DeviceName = deviceName;
		}

		private void ChangeLanguage(Language language)
		{
			_settingsHandler.CurrentLang = language;
		}

		private void ResetSteps()
		{
			_settingsHandler.ResetSteps();
		}

		public void RefreshLanguages()
		{
			Languages = (ObservableCollection<Language>) _settingsHandler.Languages;
		}

		public void GetSelectedLanguage()
		{
			SelectedLanguage = _settingsHandler.CurrentLang;
		}

		public void GetDeviceName()
		{
			DeviceName = _settingsHandler.DeviceName;
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
