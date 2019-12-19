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
		private NavigationHandler _handler;
		private ConnectivityHandler _connectivityHandler;
		private SettingsHandler _settingsHandler;
		private Image _iconOn;
		private Image _iconOff;
		private string _deviceName;
		private string _stepsAmount;
		private Boolean _connectBoolean;
		private Image _icon;

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

		public string StepsAmount
		{
			get
			{
				return _stepsAmount;
			}
			set
			{
				if (_stepsAmount != value)
				{
					_stepsAmount = value;
					OnPropertyChanged("StepsAmount");
				}
			}
		}

		public Boolean ConnectBoolean
		{
			get
			{
				return _connectBoolean;
			}
			set
			{
				if (_connectBoolean != value)
				{
					_connectBoolean = value;
					if (ConnectBoolean)
					{
						//Icon = _iconOn;
					}
					else
					{
						//Icon = _iconOff;
					}
				}
			}
		}
		
		public Image Icon
		{
			get
			{
				return _icon;
			}
			set
			{
				if (_icon != value)
				{
					_icon = value;
					OnPropertyChanged("Icon");
				}
			}
		}
		

		/**
		 Commands were called from Elements in Connection Page
		**/
		public ICommand AudioPlayerPageCommand { get; }
		public ICommand AudioLibPageCommand { get; }
		public ICommand ConnectionPageCommand { get; }
		public ICommand ModesPageCommand { get; }
		public ICommand SettingsPageCommand { get; }


		/// <summary>
		/// Initializises App Logic and all available Commands
		/// </summary>
		/// <param name="appLogic"> For needed functions in Model</param>
		public MainPageVM(NavigationHandler handler)
		{
			_handler = handler;
			_connectivityHandler = ConnectivityHandler.SingletonConnectivityHandler;
			_settingsHandler = SettingsHandler.SingletonSettingsHandler;
			AudioPlayerPageCommand = new Command(GotoAudioPlayerPage);
			AudioLibPageCommand = new Command(GotoAudioLibPage);
			ConnectionPageCommand = new Command(GotoConnectionPage);
			ModesPageCommand = new Command(GotoModesPage);
			SettingsPageCommand = new Command(GotoSettingsPage);
			//IconOn = new Image(); //fileLocation
			//IconOff = new Image(); //fileLocation
		}

		private void GotoAudioPlayerPage()
		{
			_handler.GotoPage("AudioPlayerPage");
		}

		private void GotoAudioLibPage()
		{
			_handler.GotoPage("AudioLibPage");
		}

		private void GotoConnectionPage()
		{
			if(ConnectBoolean)
			{
				//AppLogic disconnect
			}
			else
			{
				_handler.GotoPage("ConnectionPage");
			}
		}

		private void GotoModesPage()
		{
			_handler.GotoPage("ModesPage");
		}

		private void GotoSettingsPage()
		{
			_handler.GotoPage("SettingsPage");
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
