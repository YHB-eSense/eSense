using Karl.Model;
using Karl.View;
using StepDetectionLibrary;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Karl.ViewModel
{
	public class MainPageVM : INotifyPropertyChanged
	{
		protected NavigationHandler _navHandler;
		protected SettingsHandler _settingsHandler;
		protected ConnectivityHandler _connectivityHandler;
		//private OutputManager _outputManager;
		protected ImageSource _iconOn;
		protected ImageSource _iconOff;

		//Eventhandling
		public event PropertyChangedEventHandler PropertyChanged;

		//Properties binded to MainPage of View
		public string DeviceName
		{
			get
			{
				if (_connectivityHandler.EarableConnected)
				{
					return _settingsHandler.CurrentLang.Get("device_name") + ": " + _settingsHandler.DeviceName;
				}
				return null;
			}
		}
		public string StepsAmount
		{
			get
			{
				if (_connectivityHandler.EarableConnected)
				{
					//var spm = _outputManager.Log.AverageStepFrequency(TimeSpan.FromSeconds(10)) * 60;
					//return string.Format("{0}: {1} ({2} SPM)", _settingsHandler.CurrentLang.Get("steps"), _settingsHandler.Steps, spm);
					return _settingsHandler.CurrentLang.Get("steps") + ": " + _settingsHandler.Steps;
				}
				return null;
			}
		}
		public ImageSource Icon
		{
			get => _connectivityHandler.EarableConnected ? _iconOn : _iconOff;
		}
		public bool HelpVisible { get; set; }

		//Commands binded to MainPage of View
		public ICommand AudioPlayerPageCommand { get; }
		public ICommand AudioLibPageCommand { get; }
		public ICommand TryConnectCommand { get; }
		public ICommand ModesPageCommand { get; }
		public ICommand SettingsPageCommand { get; }
		public ICommand HelpCommand { get; }

		/// <summary>
		/// Initializises Commands, NavigationHandler and ConnectivityHandler, SettingsHandler of Model
		/// </summary>
		/// <param name="handler">For navigation</param>
		public MainPageVM()
		{
			InitializeSingletons();
			_settingsHandler = SettingsHandler.SingletonSettingsHandler;
			_settingsHandler.LangChanged += RefreshLang;
			_settingsHandler.DeviceNameChanged += RefreshDeviceName;
			_settingsHandler.StepsChanged += RefreshSteps;
			_connectivityHandler.ConnectionChanged += RefreshConnection;
			AudioPlayerPageCommand = new Command(GotoAudioPlayerPage);
			AudioLibPageCommand = new Command(GotoAudioLibPage);
			TryConnectCommand = new Command(TryConnect);
			ModesPageCommand = new Command(GotoModesPage);
			SettingsPageCommand = new Command(GotoSettingsPage);
			HelpCommand = new Command(HelpOnOff);
			_iconOn = ImageSource.FromResource("Karl.Resources.Images.bluetooth_on.png");
			_iconOff = ImageSource.FromResource("Karl.Resources.Images.bluetooth_off.png");
			HelpVisible = false;
		}

		private void RefreshLang(object sender, EventArgs args)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(StepsAmount)));
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DeviceName)));
		}

		private void RefreshDeviceName(object sender, EventArgs args)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DeviceName)));
		}
		private void RefreshSteps(object sender, EventArgs args)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(StepsAmount)));
		}

		private void RefreshConnection(object sender, EventArgs args)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Icon)));
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(StepsAmount)));
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DeviceName)));
		}

		private void GotoAudioPlayerPage()
		{
			_navHandler.GotoPage<AudioPlayerPage>();
		}

		private void GotoAudioLibPage()
		{
			_navHandler.GotoPage<AudioLibPage>();
		}

		private async void TryConnect()
		{
			if (_connectivityHandler.EarableConnected)
			{
				bool answer = await AlertWrapper();
				if (answer)
				{
					await _connectivityHandler.Disconnect();
					PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Icon)));
				}
			}
			else
			{
				var success = await _connectivityHandler.Connect();
				if (!success) { NavigateWrapper(); }
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Icon)));
			}

		}

		private void GotoModesPage()
		{
			_navHandler.GotoPage<ModesPage>();
		}

		private void GotoSettingsPage()
		{
			_navHandler.GotoPage<SettingsPage>();
		}

		private void HelpOnOff()
		{
			HelpVisible = !HelpVisible;
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(HelpVisible)));
		}

		[DoNotCover]
		protected virtual void InitializeSingletons()
		{
			_navHandler = NavigationHandler.SingletonNavHandler;
			_connectivityHandler = ConnectivityHandler.SingletonConnectivityHandler;
			//_outputManager = OutputManager.SingletonOutputManager;
		}

		[DoNotCover]
		protected virtual void NavigateWrapper()
		{
			INavToSettings navigator = DependencyService.Get<INavToSettings>();
			navigator.NavToSettings();
		}

		[DoNotCover]
		protected virtual async Task<bool> AlertWrapper()
		{
			return await Application.Current.MainPage.DisplayAlert(_settingsHandler.CurrentLang.Get("question_title"),
					_settingsHandler.CurrentLang.Get("question_text_2"), _settingsHandler.CurrentLang.Get("question_yes"),
					_settingsHandler.CurrentLang.Get("question_no"));
		}
	}
}
