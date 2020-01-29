using SkiaSharp;
using StepDetectionLibrary;
using System;
using System.Collections.Generic;
using System.Timers;
using Xamarin.Forms;

namespace Karl.Model
{
	/// <summary>
	/// The SettingsHandler is a class where you can change all App Settings and find the current Settings.
	/// </summary>
	public class SettingsHandler
	{
		private static SettingsHandler _singletonSettingsHandler;
		private static readonly Object _padlock = new Object();

		private LangManager _langManager;
		private ConnectivityHandler _connectivityHandler;
		private ColorManager _colorManager;
<<<<<<< HEAD
		private static SettingsHandler _singletonSettingsHandler;
		private static readonly Object _padlock = new Object();
		private int _steps;
		private int _frequency;
		private OutputManager _outputManager;
		private IDictionary<string, Object> _properties;
=======
		private OutputManager _outputManager;
		private IDictionary<string, Object> _properties;
		internal IDictionary<string, AudioModule> AvailableAudioModules;
		private int _steps;
>>>>>>> 36ac8751646f7b8ce6ffb32804c1f5056050225e
		private int _stepslastmin;
		private Timer timer;
		private AudioModule _currentAudioModule;

		//Delegates for EventHandling
		internal delegate void AudioModuleDelegate(AudioModule audioModule);

		/// <summary>
		/// List with Microchartentries to get a chart with steps in the last few minutes
		/// </summary>
		public List<Microcharts.Entry> ChartEntries;
<<<<<<< HEAD
		/// <summary>
		/// timer to set time between each microchart entry
		/// </summary>
		private void InitTimer()
		{
			timer = new Timer(TimeSpan.FromMinutes(1).TotalMilliseconds);
			timer.AutoReset = true;
			timer.Elapsed += new ElapsedEventHandler(AddChartEvent);
			timer.Start();
		}

		/// <summary>
		/// method to add microchartentries
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void AddChartEvent(object sender, ElapsedEventArgs e)
		{
			if (_connectivityHandler.EarableConnected)
			{
				Microcharts.Entry entry = new Microcharts.Entry(_stepslastmin)
				{
					Color = SKColor.Parse(CurrentColor.Color.ToHex()),
					Label = DateTime.Now.ToString("HH:mm"),
					ValueLabel = _stepslastmin.ToString()
				};
				ChartEntries.Add(entry);
				_stepslastmin = 0;
				if (ChartEntries.Count > 10)
				{
					ChartEntries.RemoveAt(0);
				}
				ChartChanged?.Invoke(this, null);
			}
		}


=======
	
>>>>>>> 36ac8751646f7b8ce6ffb32804c1f5056050225e
		//Eventhandling
		public delegate void LangEventHandler(object source, EventArgs e);
		public event LangEventHandler LangChanged;
		public delegate void DeviceNameEventHandler(object source, EventArgs e);
		public event DeviceNameEventHandler DeviceNameChanged;
		public delegate void StepsEventHandler(object source, EventArgs e);
		public event StepsEventHandler StepsChanged;
		public delegate void ColorEventHandler(object source, EventArgs e);
		public event ColorEventHandler ColorChanged;
		public delegate void ChartEventHandler(object source, EventArgs e);
		public event ChartEventHandler ChartChanged;
		public delegate void AudioModuleEventHandler(object source, EventArgs e);
		public event AudioModuleEventHandler AudioModuleChanged;

		//internal event AudioModuleDelegate AudioModuleChanged;
		public bool UsingBasicAudio { get; set; }
		public bool UsingSpotifyAudio { get; set; }
		/// <summary>
		/// The List of registered Languages.
		/// </summary>
		public List<Lang> Languages { get => _langManager.AvailableLangs; }

		public List<CustomColor> Colors { get => _colorManager.Colors; }

		/// <summary>
		/// The currently selected Language.
		/// </summary>
		public Lang CurrentLang
		{
			get => _langManager.CurrentLang;
			set
			{
				if (_properties.ContainsKey("lang")) _properties.Remove("lang");
				_properties.Add("lang", value.Tag);
				_langManager.CurrentLang = value;
				_colorManager.ResetColors();
				LangChanged?.Invoke(this, null);
			}
		}

		/// <summary>
		/// The Name of the currently connected Device.
		/// </summary>
		public string DeviceName
		{
			get
			{
				if (_connectivityHandler.EarableConnected) { return _connectivityHandler.EarableName; }
				return null;
			}
			set
			{
				_connectivityHandler.SetDeviceNameAsync(value).GetAwaiter().OnCompleted(() =>
				{
					DeviceNameChanged?.Invoke(this, null);
				});
			}
		}

		/// <summary>
		/// The total Steps done.
		/// </summary>
		public int Steps
		{
			get => _steps;
			private set
			{
				if (_properties.ContainsKey("steps")) _properties.Remove("steps");
				_properties.Add("steps", value.ToString());
				_steps = value;
				StepsChanged?.Invoke(this, null);
			}
		}

		/// <summary>
		/// The current step frequency (in steps per minute).
		/// </summary>
		public int StepFrequency { get; private set; }

		public CustomColor CurrentColor
		{
			get => _colorManager.CurrentColor;
			set
			{
				if (_properties.ContainsKey("color")) _properties.Remove("color");
				_properties.Add("color", value.Color.ToHex());
				_colorManager.CurrentColor = value;
				foreach (Microcharts.Entry entry in ChartEntries)
				{
					entry.Color = SKColor.Parse(_colorManager.CurrentColor.Color.ToHex());
				}
				ColorChanged?.Invoke(this, null);
			}
		}

		public static SettingsHandler SingletonSettingsHandler
		{
			get
			{
				lock (_padlock)
				{
					if (_singletonSettingsHandler == null) { _singletonSettingsHandler = new SettingsHandler(); }
					return _singletonSettingsHandler;
				}
			}
		}

		/// <summary>
		/// Resets the Step Counter.
		/// </summary>
		public void ResetSteps()
		{
			Steps = 0;
			StepFrequency = 0;
		}

		public void changeAudioModuleToSpotify()
		{
			AudioPlayer.SingletonAudioPlayer.changeToSpotifyPlayer();
			AudioLib.SingletonAudioLib.changeToSpotifyLib();
			UsingBasicAudio = false;
			UsingSpotifyAudio = true;
			AudioModuleChanged?.Invoke(this, null);
		}

		public void changeAudioModuleToBasic() {
			AudioPlayer.SingletonAudioPlayer.changeToBasicPlayer();
			AudioLib.SingletonAudioLib.changeToBasicLib();
			UsingBasicAudio = true;
			UsingSpotifyAudio = false;
			AudioModuleChanged?.Invoke(this, null);

		}

		/// <summary>
		/// The Constructor that builds a new SettingsHandler
		/// </summary>
		private SettingsHandler()
		{
			UsingSpotifyAudio = false;
			UsingBasicAudio = true;
			_connectivityHandler = ConnectivityHandler.SingletonConnectivityHandler;
			_outputManager = OutputManager.SingletonOutputManager;
			_outputManager.Subscribe(new StepDetectionObserver(this));
			_langManager = LangManager.SingletonLangManager;
			_colorManager = ColorManager.SingletonColorManager;
			_properties = Application.Current.Properties;
			_stepslastmin = 0;
			ChartEntries = new List<Microcharts.Entry>();
			InitTimer();

			
			//Load Color
			Object val;
			if (_properties.TryGetValue("color", out val))
			{
				bool FoundColor = false;
				foreach (CustomColor CC in Colors)
				{
					if (CC.Color.ToHex().Equals(val.ToString()))
					{
						CurrentColor = CC;
						FoundColor = true;
						break;
					}
				}
				if (!FoundColor)
				{
					_properties.Remove("color");
					_properties.Add("color", Colors[0].Color.ToHex());
					CurrentColor = Colors[0];
				}
			}
			else
			{
				_properties.Add("color", Colors[0].Color.ToHex());
				CurrentColor = Colors[0];
			}	
			//Load Chosen Language
			if (_properties.TryGetValue("lang", out val))
			{
				if (_langManager.ChooseLang(val.ToString()))
					System.Diagnostics.Debug.WriteLine("LangChosen: " + val.ToString());
				else
				{
					_langManager.ChooseLang("lang_english");
					_properties.Add("lang", "lang_english");
				}
			}
			else
			{
				_langManager.ChooseLang("lang_english");
				_properties.Add("lang", "lang_english");
			}

			//Load Steps
			if (_properties.TryGetValue("steps", out val))
			{
				string Value = val.ToString();
				try
				{
					_steps = int.Parse(Value);
				}
				catch (FormatException)
				{
					_steps = 0;
					_properties.Remove("steps");
					_properties.Add("steps", "0");
				}
			}
			else
			{
				_steps = 0;
				_properties.Add("steps", "0");
			}

			//CurrentAudioModule = AvailableAudioModules["spotifyAudioModule"];
		}

		/// <summary>
		/// timer to set time between each microchart entry
		/// </summary>
		private void InitTimer()
		{
			timer = new Timer(TimeSpan.FromMinutes(1).TotalMilliseconds);
			timer.AutoReset = true;
			timer.Elapsed += new ElapsedEventHandler(AddChartEvent);
			timer.Start();
		}

		/// <summary>
		/// method to add microchartentries
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void AddChartEvent(object sender, ElapsedEventArgs e)
		{
			if (_connectivityHandler.EarableConnected)
			{
				Microcharts.Entry entry = new Microcharts.Entry(_stepslastmin)
				{
					Color = SKColor.Parse(CurrentColor.Color.ToHex()),
					Label = DateTime.Now.ToString("HH:mm"),
					ValueLabel = _stepslastmin.ToString()
				};
				ChartEntries.Add(entry);
				_stepslastmin = 0;
				if (ChartEntries.Count > 10)
				{
					ChartEntries.RemoveAt(0);
				}
				ChartChanged?.Invoke(this, null);
			}
		}

		private class StepDetectionObserver : IObserver<Output>
		{
			private SettingsHandler _parent;
			public StepDetectionObserver(SettingsHandler parent)
			{
				_parent = parent;
			}
			public void OnCompleted()
			{
				throw new NotImplementedException();
			}

			public void OnError(Exception error)
			{
				throw new NotImplementedException();
			}

			public void OnNext(Output value)
			{
				_parent.Steps = _parent._steps + value.StepCount;
				_parent.StepFrequency = (int) (value.Frequency * 60);
				_parent._stepslastmin = _parent._stepslastmin + value.StepCount;
			}
		}

		
	}
}
