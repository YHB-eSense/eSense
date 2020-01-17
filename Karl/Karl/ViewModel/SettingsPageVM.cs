using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using Karl.Model;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

namespace Karl.ViewModel
{
	public class SettingsPageVM : INotifyPropertyChanged
	{
		private SettingsHandler _settingsHandler;
		private string _deviceName;
		private LangManager _langManager;
		private string _currentColorName;

		//Eventhandling
		public event PropertyChangedEventHandler PropertyChanged;

		//Properties binded to SettingsPage of View
		public string LanguageLabel { get => _langManager.CurrentLang.Get("language"); }
		public string ColorLabel { get => _langManager.CurrentLang.Get("color"); }
		public string DeviceNameLabel { get => _langManager.CurrentLang.Get("device_name"); }
		public string ChangeDeviceNameLabel { get => _langManager.CurrentLang.Get("change_device_name"); }
		public string ResetStepsLabel { get => _langManager.CurrentLang.Get("reset_steps"); }
		public List<Lang> Languages { get => _settingsHandler.Languages; }
		public List<CustomColor> Colors { get => _settingsHandler.Colors; }
		public Lang SelectedLanguage
		{
			get => _settingsHandler.CurrentLang;
			set
			{
				_settingsHandler.CurrentLang = value;
			}
		}
		public CustomColor CurrentColor
		{
			get => _settingsHandler.CurrentColor;
			set { if (value != null) { _settingsHandler.CurrentColor = value; } }
		}
		public string DeviceName
		{
			get => _settingsHandler.DeviceName;
			set
			{
				_deviceName = value;
			}
		}

		//Commands binded to SettingsPage of View
		public ICommand ChangeDeviceNameCommand { get; }
		public ICommand ResetStepsCommand { get; }

		/// <summary>
		/// Initializises Commands and SettingsHandler of Model
		/// </summary>
		public SettingsPageVM()
		{
			_settingsHandler = SettingsHandler.SingletonSettingsHandler;
			_currentColorName = _settingsHandler.CurrentColor.Name;
			_langManager = LangManager.SingletonLangManager;
			ChangeDeviceNameCommand = new Command(ChangeDeviceName);
			ResetStepsCommand = new Command(ResetSteps);
			_settingsHandler.SettingsChanged += Refresh;
		}

		public void Refresh(object sender, EventArgs args)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LanguageLabel)));
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DeviceNameLabel)));
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ChangeDeviceNameLabel)));
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ResetStepsLabel)));
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ColorLabel)));
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Colors)));
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentColor)));
		}

		private void ChangeDeviceName()
		{
			_settingsHandler.DeviceName = _deviceName;
		}

		private void ResetSteps()
		{
			_settingsHandler.ResetSteps();
		}
		
	}
}
