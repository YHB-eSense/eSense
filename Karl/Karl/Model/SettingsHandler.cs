using SkiaSharp;
using StepDetectionLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Timers;
using Xamarin.Forms;
using static Karl.Model.ColorManager;
using static Karl.Model.ConnectivityHandler;
using static Karl.Model.LangManager;
using static StepDetectionLibrary.OutputManager;

namespace Karl.Model
{
	/// <summary>
	/// The SettingsHandler is a class where you can change all App Settings and find the current Settings.
	/// </summary>
	public class SettingsHandler
	{

		private static SettingsHandler _singletonSettingsHandler;

		/// <summary>
		/// The singleton object of SettingsHandler
		/// </summary>
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

		private static readonly Object _padlock = new Object();

		protected readonly IDictionary<string, Object> _properties;
		private int _steps;
		private Timer timer;

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

		/// <summary>
		/// List with Microchartentries to get a chart with steps in the last few minutes
		/// </summary>
		public List<Microcharts.Entry> ChartEntries;

		/// <summary>
		/// Gives info whether BasicAudioPlayer and BasicAudioLib are used
		/// </summary>
		public bool UsingBasicAudio { get; private set; }

		/// <summary>
		/// Gives info whether SpotifyAudioPlayer and SpotifyAudioLib are used
		/// </summary>
		public bool UsingSpotifyAudio { get; private set; }

		/// <summary>
		/// The List of registered languages
		/// </summary>
		public List<Lang> Languages { get => SingletonLangManager.AvailableLangs; }

		/// <summary>
		/// List of available colors
		/// </summary>
		public List<CustomColor> Colors { get => SingletonColorManager.Colors; }

		/// <summary>
		/// The currently selected language
		/// </summary>
		public Lang CurrentLang
		{
			get => SingletonLangManager.CurrentLang;
			set
			{
				if (_properties.ContainsKey("lang")) _properties.Remove("lang");
				_properties.Add("lang", value.Tag);

				//Tested elsewhere
				if (!_testing) SingletonLangManager.CurrentLang = value;
				if (!_testing) SingletonColorManager.ResetColors();

				LangChanged?.Invoke(this, null);
			}
		}

		/// <summary>
		/// The Name of the currently connected device
		/// </summary>
		public string DeviceName
		{
			get
			{
				if (SingletonConnectivityHandler.EarableConnected) { return SingletonConnectivityHandler.EarableName; }
				return null;
			}
			set
			{
				SingletonConnectivityHandler.SetDeviceNameAsync(value).GetAwaiter().OnCompleted(() =>
				{
					DeviceNameChanged?.Invoke(this, null);
				});
				DeviceNameChanged?.Invoke(this, null);
			}
		}

		/// <summary>
		/// The total steps done
		/// </summary>
		public int Steps
		{
			get => _steps;
			private set
			{
				if (_properties == null) return;
				if (_properties.ContainsKey("steps")) _properties.Remove("steps");
				_properties.Add("steps", value.ToString());
				_steps = value;
				StepsChanged?.Invoke(this, null);
			}
		}

		/// <summary>
		/// The color currently selected
		/// </summary>
		public CustomColor CurrentColor
		{
			get => SingletonColorManager.CurrentColor;
			set
			{
				//if (_properties == null) return;

				if (_properties.ContainsKey("color")) _properties.Remove("color");
				_properties.Add("color", value.Color.ToHex());

				//Tested elsewhere
				if (!_testing) SingletonColorManager.CurrentColor = value;

				foreach (Microcharts.Entry entry in ChartEntries)
				{
					entry.Color = SKColor.Parse(value.Color.ToHex());
				}

				ColorChanged?.Invoke(this, null);
			}
		}

		/// <summary>
		/// Resets the step counter
		/// </summary>
		public void ResetSteps()
		{
			//Disable Connections for Unit Testing
			if (!_testing) SingletonOutputManager.Log.Reset();
			Steps = 0;
		}

		/// <summary>
		/// The constructor that builds a new SettingsHandler
		/// </summary>
		private SettingsHandler()
		{
			_properties = (_propertiesInjection == null | !_testing) ? Application.Current.Properties : _propertiesInjection;
			UsingSpotifyAudio = false;
			UsingBasicAudio = true;
			SingletonOutputManager.Subscribe(new StepDetectionObserver(this));
			// _stepslastmin = 0;
			ChartEntries = new List<Microcharts.Entry>();
			InitTimer();
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
			//Load chosen language
			if (_properties.TryGetValue("lang", out val))
			{
				if (SingletonLangManager.ChooseLang(val.ToString()))
					System.Diagnostics.Debug.WriteLine("LangChosen: " + val.ToString());
				else
				{
					SingletonLangManager.ChooseLang("lang_english");
					_properties.Remove("lang");
					_properties.Add("lang", "lang_english");
				}
			}
			else
			{
				SingletonLangManager.ChooseLang("lang_english");
				_properties.Add("lang", "lang_english");
			}

			//Load steps
			if (_properties.TryGetValue("steps", out val))
			{
				string Value = val.ToString();
				try
				{
					_steps = int.Parse(Value);
				}
				catch (FormatException e)
				{
					System.Diagnostics.Debug.WriteLine("[Exception] Value of steps: " + Value + " could not be parsed.");
					System.Diagnostics.Debug.WriteLine(e.StackTrace);
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
		}

		/// <summary>
		/// Changes active audioplayer and -lib to SpotifyAudioPlayer and SpotifyAudioLib
		/// </summary>
		public void ChangeAudioModuleToSpotify()
		{
			AudioPlayer.SingletonAudioPlayer.ChangeToSpotifyPlayer();
			AudioLib.SingletonAudioLib.ChangeToSpotifyLib();
			UsingBasicAudio = false;
			UsingSpotifyAudio = true;
			AudioModuleChanged?.Invoke(this, null);
		}

		/// <summary>
		/// Changes active audioplayer and -lib to BasicAudioPlayer and BasicAudioLib
		/// </summary>
		public void ChangeAudioModuleToBasic()
		{
			UsingBasicAudio = true;
			UsingSpotifyAudio = false;
			AudioLib.SingletonAudioLib.ChangeToBasicLib();
			AudioPlayer.SingletonAudioPlayer.ChangeToBasicPlayer();
			AudioModuleChanged?.Invoke(this, null);
		}

		/// <summary>
		/// Timer to set time between each microchart entry
		/// </summary>
		private void InitTimer()
		{
			timer = new Timer(TimeSpan.FromMinutes(1).TotalMilliseconds);
			timer.AutoReset = true;
			timer.Elapsed += new ElapsedEventHandler(AddChartEvent);
			timer.Start();
		}

		/// <summary>
		/// Method to add microchartentries
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void AddChartEvent(object sender, ElapsedEventArgs e)
		{
			if (SingletonConnectivityHandler.EarableConnected)
			{
				var stepslastmin = SingletonOutputManager.Log.CountSteps(duration: TimeSpan.FromMinutes(1));
				Microcharts.Entry entry = new Microcharts.Entry(stepslastmin)
				{
					Color = SKColor.Parse(CurrentColor.Color.ToHex()),
					Label = DateTime.Now.ToString("HH:mm"),
					ValueLabel = stepslastmin.ToString()
				};
				ChartEntries.Add(entry);
				if (ChartEntries.Count > 10)
				{
					ChartEntries.RemoveAt(0);
				}
				ChartChanged?.Invoke(this, null);
			}
		}


		//Testing Attributes
		private static bool _testing = false;
		private static IDictionary<string, Object> _propertiesInjection = null;

		/// <summary>
		/// This injects an Properties Dictionary instead of the Application.Current.Properties
		/// </summary>
		/// <param name="injection">The Mock Ditionary</param>
		[Conditional("TESTING")]
		internal static void PropertiesInjection(IDictionary<string, object> injection)
		{
			_propertiesInjection = injection;
		}
		/// <summary>
		/// Enables the Testing Mode, disabling many Connections for UnitTesting.
		/// </summary>
		/// <param name="testing">Testing parameter</param>
		[Conditional("TESTING")]
		internal static void Testing(bool testing)
		{
			_testing = testing;
		}

		/// <summary>
		/// Observer for the stepdetection
		/// </summary>
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
				_parent.Steps = value.Log.CountSteps();
			}
		}


	}
}
