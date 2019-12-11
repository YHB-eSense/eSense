using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using Karl.Model;
using Karl.View;
using System.Windows.Input;
using Xamarin.Forms;

namespace Karl.ViewModel
{
	public class MainPageVM : INotifyPropertyChanged
	{
		private AppLogic AppLogic;
		private Image icon; 
		public ICommand AudioPlayerPageCommand;
		public ICommand AudioLibPageCommand;
		public ICommand ConnectionPageCommand;
		public ICommand ModesPageCommand;
		public ICommand SettingsPageCommand;
		public INavigation Navigation { get; set; }
		private string deviceName;
		private string stepsAmount;
		private Boolean connectBoolean;

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

		public string StepsAmount
		{
			get
			{
				return stepsAmount;
			}
			set
			{
				if (stepsAmount != value)
				{
					stepsAmount = value;
					OnPropertyChanged("StepsAmount");
				}
			}
		}

		public Boolean ConnectBoolean
		{
			get
			{
				return connectBoolean;
			}
			set
			{
				if (connectBoolean != value)
				{
					connectBoolean = value;
					OnPropertyChanged("Icon");
				}
			}
		}

		public MainPageVM(AppLogic appLogic)
		{
			AppLogic = appLogic;
			AudioPlayerPageCommand = new Command(GotoAudioPlayerPage);
			AudioLibPageCommand = new Command(GotoAudioLibPage);
			ConnectionPageCommand = new Command(GotoConnectionPage);
			ModesPageCommand = new Command(GotoModesPage);
			SettingsPageCommand = new Command(GotoSettingsPage);
		}

		private void GotoAudioPlayerPage()
		{
			NavigationHandler.GotoAudioPlayerPage();
			//NavigationHandler.GotoAudioPlayerPage(Navigation);
		}

		private void GotoAudioLibPage()
		{
			NavigationHandler.GotoAudioLibPage(Navigation);
		}

		private void GotoConnectionPage()
		{
			if(ConnectBoolean)
			{
				//AppLogic disconnect
			}
			else
			{
			NavigationHandler.GotoConnectionPage(Navigation);
			}
		}

		private void GotoModesPage()
		{
			NavigationHandler.GotoModesPage(Navigation);
		}

		private void GotoSettingsPage()
		{
			NavigationHandler.GotoSettingsPage(Navigation);
		}

		public void GetDeviceName()
		{
			string deviceName = "";
			//AppLogic
			DeviceName = deviceName;
		}

		public void GetStepsAmount()
		{
			string stepsAmount = "";
			//AppLogic
			StepsAmount = stepsAmount;
		}

		public void GetConnectBoolean()
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
