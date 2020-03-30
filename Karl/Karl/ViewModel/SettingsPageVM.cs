using Karl.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace Karl.ViewModel
{
	public class SettingsPageVM : INotifyPropertyChanged
	{
		internal static readonly int MAX_NAME_LENGTH = 11;

		protected SettingsHandler _settingsHandler;
		private ConnectivityHandler _connectivityHandler;
		private string _deviceName;

		//Eventhandling
		public event PropertyChangedEventHandler PropertyChanged;

		//Properties binded to SettingsPage of View
		public string LanguageLabel { get => _settingsHandler.CurrentLang.Get("language"); }
		public string ColorLabel { get => _settingsHandler.CurrentLang.Get("color"); }
		public string DeviceNameLabel { get => _settingsHandler.CurrentLang.Get("device_name"); }
		public string ChangeDeviceNameLabel { get => _settingsHandler.CurrentLang.Get("change_device_name"); }
		public string ResetStepsLabel { get => _settingsHandler.CurrentLang.Get("reset_steps"); }
		public string UseAudioModuleLabel
		{
			get
			{
				if (_settingsHandler.UsingBasicAudio) { return _settingsHandler.CurrentLang.Get("use_spotify"); }
				return _settingsHandler.CurrentLang.Get("use_basic");
			}
		}
		public Color UseAudioModuleColor
		{
			get
			{
				if (_settingsHandler.UsingBasicAudio) { return Color.FromHex("#1ed761"); }
				return CurrentColor.Color;
			}
		}
		public List<Lang> Languages { get => _settingsHandler.Languages; }
		public List<CustomColor> Colors { get => _settingsHandler.Colors; }
		public Lang SelectedLanguage
		{
			get => _settingsHandler.CurrentLang;
			set => _settingsHandler.CurrentLang = value;
		}
		public CustomColor CurrentColor
		{
			get => _settingsHandler.CurrentColor;
			set { if (value != null) _settingsHandler.CurrentColor = value; }
		}
		public string DeviceName
		{
			get
			{
				if (_connectivityHandler.EarableConnected)
				{
					return _settingsHandler.DeviceName;
				}
				return null;
			}
			set
			{
				if (value.Length > MAX_NAME_LENGTH)
				{
					Application.Current.MainPage.DisplayAlert(
						_settingsHandler.CurrentLang.Get("name_length_limited_title"),
						_settingsHandler.CurrentLang.Get("name_length_limited_text"),
						_settingsHandler.CurrentLang.Get("name_length_limited_okay"));
				}
				_deviceName = value.Substring(0, MAX_NAME_LENGTH);
			}
		}

		//Commands binded to SettingsPage of View
		public ICommand ChangeDeviceNameCommand { get; }
		public ICommand ResetStepsCommand { get; }
		public ICommand ChangeAudioModuleCommand { get; }

		/// <summary>
		/// Initializises Commands and SettingsHandler of Model
		/// </summary>
		public SettingsPageVM()
		{
			_settingsHandler = SettingsHandler.SingletonSettingsHandler;
			_connectivityHandler = ConnectivityHandler.SingletonConnectivityHandler;
			ChangeDeviceNameCommand = new Command(ChangeDeviceName);
			ResetStepsCommand = new Command(ResetSteps);
			ChangeAudioModuleCommand = new Command(ChangeAudioModule);
			_settingsHandler.LangChanged += RefreshLang;
			_settingsHandler.DeviceNameChanged += RefreshDeviceName;
			_settingsHandler.ColorChanged += RefreshColor;
			_settingsHandler.AudioModuleChanged += RefreshAudioModule;
			_connectivityHandler.ConnectionChanged += RefreshConnection;
		}

		private void RefreshLang(object sender, EventArgs args)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LanguageLabel)));
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DeviceNameLabel)));
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ChangeDeviceNameLabel)));
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ResetStepsLabel)));
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ColorLabel)));
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Colors)));
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentColor)));
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(UseAudioModuleLabel)));
		}

		private void RefreshDeviceName(object sender, EventArgs args)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DeviceName)));
		}

		private void RefreshColor(object sender, EventArgs args)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentColor)));
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(UseAudioModuleColor)));
		}

		private void RefreshAudioModule(object sender, EventArgs args)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(UseAudioModuleLabel)));
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(UseAudioModuleColor)));
		}

		private void RefreshConnection(object sender, EventArgs args)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DeviceName)));
		}

		private void ChangeDeviceName()
		{
			_settingsHandler.DeviceName = _deviceName;
		}

		private void ResetSteps()
		{
			_settingsHandler.ResetSteps();
		}

		private void ChangeAudioModule()
		{
			if (_settingsHandler.UsingSpotifyAudio)
			{
				_settingsHandler.ChangeAudioModuleToBasic();
			}
			else
			{
				eSenseSpotifyWebAPI.WebApiSingleton.Auth();
				eSenseSpotifyWebAPI.WebApiSingleton.authentificationFinished += (sender, args) =>
				{
					SettingsHandler.SingletonSettingsHandler.ChangeAudioModuleToSpotify();
				};
			}
		}

	}
}
