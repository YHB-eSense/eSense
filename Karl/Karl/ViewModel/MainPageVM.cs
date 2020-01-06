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
		private string _iconOn;
		private string _iconOff;
		private string _deviceName;
		private string _stepsAmount;
		private Boolean _connectBoolean;
		private string _icon;

		/**
		 Properties binded to MainPage of View
		**/
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
						Icon = _iconOn;
					}
					else
					{
						Icon = _iconOff;
					}
				}
			}
		}
		
		public string Icon
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
		 Commands binded to MainPage of View
		**/
		public ICommand AudioPlayerPageCommand { get; }
		public ICommand AudioLibPageCommand { get; }
		public ICommand ConnectionPageCommand { get; }
		public ICommand ModesPageCommand { get; }
		public ICommand SettingsPageCommand { get; }

		/// <summary>
		/// Initializises Commands, NavigationHandler and ConnectivityHandler, SettingsHandler of Model
		/// </summary>
		/// <param name="handler">For navigation</param>
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
			_iconOn = "bluetooth_on.png";
			_iconOff = "bluetooth_off.png";
			Icon = _iconOn;
		}

		private void GotoAudioPlayerPage()
		{
			_handler.GotoPage(_handler._pages[0]);
		}

		private void GotoAudioLibPage()
		{
			_handler.GotoPage(_handler._pages[1]);
		}

		private void GotoConnectionPage()
		{
			if(ConnectBoolean)
			{
				_connectivityHandler.Disconnect();
			}
			else
			{
				_handler.GotoPage(_handler._pages[2]);
			}
		}

		private void GotoModesPage()
		{
			_handler.GotoPage(_handler._pages[3]);
		}

		private void GotoSettingsPage()
		{
			_handler.GotoPage(_handler._pages[4]);
		}

		public void GetDeviceName()
		{
			//DeviceName = _connectivityHandler.CurrentDevice.Name;
		}

		public void GetStepsAmount()
		{
			//StepsAmount = Convert.ToString(_settingsHandler.Steps);
		}

		public void GetConnectBoolean()
		{
			//ConnectBoolean = _connectivityHandler.Connected;
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
