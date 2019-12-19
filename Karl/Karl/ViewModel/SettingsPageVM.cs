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

		/// <summary>
		/// List contains all available languages
		/// </summary>
		public ObservableCollection<Language> Languages { get; set; }

		/// <summary>
		/// Contains the language of the app
		/// </summary>
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

		/// <summary>
		/// Contains name of the connected Device
		/// </summary>
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


		public ICommand ChangeDeviceNameCommand { get; }
		public ICommand ChangeLanguageCommand { get; }
		public ICommand ResetStepsCommand { get; }

		/// <summary>
		/// Initializises App Logic and all available Commands
		/// </summary>
		/// <param name="appLogic"> For needed functions in Model</param>
		public SettingsPageVM()
		{
			_settingsHandler = SettingsHandler.SingletonSettingsHandler;
			ChangeDeviceNameCommand = new Command<String>(ChangeDeviceName);
			ChangeLanguageCommand = new Command<Language>(ChangeLanguage);
			ResetStepsCommand = new Command(ResetSteps);
		}

		private void ChangeDeviceName(String deviceName)
		{
			//AppLogic
		}

		private void ChangeLanguage(Language language)
		{
			//AppLogic
		}

		private void ResetSteps()
		{
			//AppLogic
		}

		/// <summary>
		/// Gets Languages from Model and refreshs the Listview afterwards
		/// </summary>
		public void RefreshLanguages()
		{
			ObservableCollection<Language> languages = new ObservableCollection<Language>();
			/*
			string[] languages = appLogic.
			for(int i = 0; i < devices.Length; i++)
			{
				languagesList.Add(devices[i]);
			}
			*/
			//languages.Add("English");
			//languages.Add("Deutsch");
			Languages = languages;
		}

		public void GetSelectedLanguage()
		{
			//string selectedLanguage = "English";
			//AppLogic
			//SelectedLanguage = selectedLanguage;
		}

		public void GetDeviceName()
		{
			string deviceName = "Earables";
			//AppLogic
			DeviceName = deviceName;
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
