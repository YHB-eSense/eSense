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
		//private Image IconOn;
		//private Image IconOff;
		private string deviceName;
		private string stepsAmount;
		private Boolean connectBoolean;
		//private Image icon;

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
					if (ConnectBoolean)
					{
						//Icon = IconOn;
					}
					else
					{
						//Icon = IconOff;
					}
				}
			}
		}
		/*
		public Image Icon
		{
			get
			{
				return icon;
			}
			set
			{
				if (icon != value)
				{
					icon = value;
					OnPropertyChanged("Icon");
				}
			}
		}
		*/
		public ICommand AudioPlayerPageCommand { get; }
		public ICommand AudioLibPageCommand { get; }
		public ICommand ConnectionPageCommand { get; }
		public ICommand ModesPageCommand { get; }
		public ICommand SettingsPageCommand { get; }

		public MainPageVM(AppLogic appLogic)
		{
			AppLogic = appLogic;
			AudioPlayerPageCommand = new Command(GotoAudioPlayerPage);
			AudioLibPageCommand = new Command(GotoAudioLibPage);
			ConnectionPageCommand = new Command(GotoConnectionPage);
			ModesPageCommand = new Command(GotoModesPage);
			SettingsPageCommand = new Command(GotoSettingsPage);
			//IconOn = new Image(); //fileLocation
			//IconOff = new Image(); //fileLocation
			ConnectBoolean = false;
		}

		public void GotoAudioPlayerPage()
		{
			NavigationHandler.GotoAudioPlayerPage();
		}

		public void GotoAudioLibPage()
		{
			NavigationHandler.GotoAudioLibPage();
		}

		public void GotoConnectionPage()
		{
			if(ConnectBoolean)
			{
				//AppLogic disconnect
			}
			else
			{
				NavigationHandler.GotoConnectionPage();
			}
		}

		public void GotoModesPage()
		{
			NavigationHandler.GotoModesPage();
		}

		public void GotoSettingsPage()
		{
			NavigationHandler.GotoSettingsPage();
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
			Boolean connectBoolean = false;
			//AppLogic
			ConnectBoolean = connectBoolean;
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
