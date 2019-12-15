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
		private AppLogic _appLogic;
		private ObservableCollection<string> _languages;
		private string _selectedLanguage;
		private string _deviceName;

		/// <summary>
		/// List contains all available languages
		/// </summary>
		public ObservableCollection<string> Languages
		{
			get
			{
				return _languages;
			}
			set
			{
				if (_languages != value)
				{
					_languages = value;
					OnPropertyChanged("Languages");
				}
			}
		}

		/// <summary>
		/// Contains the language of the app
		/// </summary>
		public string SelectedLanguage
		{
			get
			{
				return _selectedLanguage;
			}
			set
			{
				if (_selectedLanguage != value)
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

		public SettingsPageVM(AppLogic appLogic)
		{
			_appLogic = appLogic;
			ChangeDeviceNameCommand = new Command<String>(ChangeDeviceName);
			ChangeLanguageCommand = new Command<String>(ChangeLanguage);
			ResetStepsCommand = new Command(ResetSteps);
		}

		public void ChangeDeviceName(String deviceName)
		{
			//AppLogic
		}

		public void ChangeLanguage(String language)
		{
			//AppLogic
		}

		public void ResetSteps()
		{
			//AppLogic
		}

		/// <summary>
		/// Gets Languages from Model and refreshs the Listview afterwards
		/// </summary>
		public void RefreshLanguages()
		{
			ObservableCollection<string> languages = new ObservableCollection<string>();
			/*
			string[] languages = appLogic.
			for(int i = 0; i < devices.Length; i++)
			{
				languagesList.Add(devices[i]);
			}
			*/
			languages.Add("English");
			languages.Add("Deutsch");
			Languages = languages;
		}

		public void GetSelectedLanguage()
		{
			string selectedLanguage = "English";
			//AppLogic
			SelectedLanguage = selectedLanguage;
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
