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
		private AppLogic AppLogic;
		public ICommand ChangeDeviceNameCommand;
		public ICommand ChangeLanguageCommand;
		public ICommand ResetStepsCommand;
		private ObservableCollection<string> languages;
		private string selectedLanguage;
		private string deviceName;

		/**List contains all available languages*/
		public ObservableCollection<string> Languages
		{
			get
			{
				return languages;
			}
			set
			{
				if (languages != value)
				{
					languages = value;
					OnPropertyChanged("Languages");
				}
			}
		}

		/**Contains the language of the app**/
		public string SelectedLanguage
		{
			get
			{
				return selectedLanguage;
			}
			set
			{
				if (selectedLanguage != value)
				{
					selectedLanguage = value;
					ChangeLanguage(selectedLanguage);
					OnPropertyChanged("SelectedLanguage");
				}
			}
		}

		/**Contains name of the connected Device**/
		public string DeviceName
		{
			get
			{
				return deviceName;
			}
			set
			{
				if (deviceName != value)
				{
					deviceName = value;
					OnPropertyChanged("DeviceName");
				}
			}
		}

		public SettingsPageVM(AppLogic appLogic)
		{
			AppLogic = appLogic;
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

		/**Gets Languages from Model and refreshs the Listview afterwards**/
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
