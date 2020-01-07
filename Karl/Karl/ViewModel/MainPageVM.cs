using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using Karl.Model;
using Karl.View;
using System.Windows.Input;
using Xamarin.Forms;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;

namespace Karl.ViewModel
{
	public class MainPageVM : INotifyPropertyChanged
	{
		private NavigationHandler _handler;
		private ConnectivityHandler _connectivityHandler;
		private SettingsHandler _settingsHandler;
		private LangManager _langManager;
		private string _iconOn;
		private string _iconOff;
		private string _icon;

		/**
		 Properties binded to MainPage of View
		**/
		public string StepsLabel { get => _langManager.CurrentLang.Get("steps"); }
		public string DeviceNameLabel { get => _langManager.CurrentLang.Get("device_name"); }
		public string DeviceName
		{
			get
			{
				if (_connectivityHandler.Connected) { return _settingsHandler.DeviceName; }
				return null;
			}
		}

		public string StepsAmount
		{
			get
			{
				if (_connectivityHandler.Connected) { return Convert.ToString(_settingsHandler.Steps); }
				return null;
			}
		}

		public bool ConnectBoolean
		{
			get => _connectivityHandler.Connected; 
		}
		
		public string Icon
		{
			get => _icon;
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
			_langManager = LangManager.SingletonLangManager;
			AudioPlayerPageCommand = new Command(GotoAudioPlayerPage);
			AudioLibPageCommand = new Command(GotoAudioLibPage);
			ConnectionPageCommand = new Command(GotoConnectionPage);
			ModesPageCommand = new Command(GotoModesPage);
			SettingsPageCommand = new Command(GotoSettingsPage);
			_iconOn = "bluetooth_on.png";
			_iconOff = "bluetooth_off.png";
			Icon = _iconOff;
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
			if(_connectivityHandler.Connected) { _connectivityHandler.Disconnect(); }
			else { _handler.GotoPage(_handler._pages[2]); }
		}

		private void GotoModesPage()
		{
			_handler.GotoPage(_handler._pages[3]);
		}

		private void GotoSettingsPage()
		{
			_handler.GotoPage(_handler._pages[4]);
		}

		public async void RefreshPage()
		{
			var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Location);
			if (status != PermissionStatus.Granted)
			{
				await CrossPermissions.Current.RequestPermissionsAsync(Permission.Location);
			}
			if (_connectivityHandler.Connected) { Icon = _iconOn; }
			else { Icon = _iconOff; }
			OnPropertyChanged("StepsAmount");
			OnPropertyChanged("DeviceName");
			OnPropertyChanged("StepsLabel");
			OnPropertyChanged("DeviceNameLabel");
		}

		//Eventhandling

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
