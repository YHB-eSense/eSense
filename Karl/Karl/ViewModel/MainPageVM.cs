using System;
using System.ComponentModel;
using Karl.Model;
using System.Windows.Input;
using Xamarin.Forms;
using Karl.View;

namespace Karl.ViewModel
{
	public class MainPageVM : INotifyPropertyChanged
	{
		private NavigationHandler _handler;
		private SettingsHandler _settingsHandler;
		private ConnectivityHandler _connectivityHandler;
		private LangManager _langManager;
		private string _iconOn;
		private string _iconOff;

		//Eventhandling
		public event PropertyChangedEventHandler PropertyChanged;

		//Properties binded to MainPage of View
		public CustomColor CurrentColor { get => _settingsHandler.CurrentColor; }
		public string StepsLabel { get => _langManager.CurrentLang.Get("steps"); }
		public string DeviceNameLabel { get => _langManager.CurrentLang.Get("device_name"); }
		public string DeviceName
		{
			get
			{
				if (_connectivityHandler.EarableConnected) { return _settingsHandler.DeviceName; }
				return null;
			}
		}
		public string StepsAmount
		{
			get
			{
				if (_connectivityHandler.EarableConnected) { return Convert.ToString(_settingsHandler.Steps); }
				return null;
			}
		}
		public string Icon
		{
			get
			{
				if (_connectivityHandler.EarableConnected) { return _iconOn; }
				return _iconOff;
			}
		}

		//Commands binded to MainPage of View
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
			ConnectionPageCommand = new Command(TryConnect);
			ModesPageCommand = new Command(GotoModesPage);
			SettingsPageCommand = new Command(GotoSettingsPage);
			_iconOn = "bluetooth_on.png";
			_iconOff = "bluetooth_off.png";
			_settingsHandler.SettingsChanged += Refresh;
		}

		private void Refresh(object sender, EventArgs args)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(StepsAmount)));
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DeviceName)));
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(StepsLabel)));
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DeviceNameLabel)));
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentColor)));
		}

		private void GotoAudioPlayerPage()
		{
			_handler.GotoPage<AudioPlayerPage>();
		}

		private void GotoAudioLibPage()
		{
			_handler.GotoPage<AudioLibPage>();
		}

		private async void TryConnect()
		{
			/*
			// Check for (required) location permission first
			var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Location);
			if (status != PermissionStatus.Granted)
			{
				await CrossPermissions.Current.RequestPermissionsAsync(Permission.Location);
			}
			*/

			if (_connectivityHandler.EarableConnected)
			{
				await _connectivityHandler.Disconnect();
			}
			else
			{
				var success = await _connectivityHandler.Connect();
				if (!success)
				{
					INavToSettings navigator = DependencyService.Get<INavToSettings>();
					navigator.NavToSettings();
				}

			}
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Icon)));
		}

		private void GotoModesPage()
		{
			_handler.GotoPage<ModesPage>();
		}

		private void GotoSettingsPage()
		{
			_handler.GotoPage<SettingsPage>();
		}

	}
}
