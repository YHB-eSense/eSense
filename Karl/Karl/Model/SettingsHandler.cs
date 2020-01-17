using StepDetectionLibrary;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Karl.Model
{
	/// <summary>
	/// The SettingsHandler is a class where you can change all App Settings and find the current Settings.
	/// </summary>
	public class SettingsHandler: IObserver<Output>
	{
		private LangManager _langManager;
		private static SettingsHandler _singletonSettingsHandler;
		private static readonly Object _padlock = new Object();
		private CustomColor _currentColor;
		private AudioModule _currentAudioModule;
		private int _steps;
		private OutputManager outputManager;
		private IDictionary<string, Object> _properties;
		internal IDictionary<string, AudioModule> AvailableAudioModules;

		//Delegates for EventHandling
		internal delegate void AudioModuleDelegate(AudioModule audioModule);

		//Eventhandling
		public event EventHandler SettingsChanged;
		internal event AudioModuleDelegate AudioModuleChanged;

		/// <summary>
		/// The List of registered Languages.
		/// </summary>
		public List<Lang> Languages { get => _langManager.AvailableLangs; }

		public List<CustomColor> Colors { get; }

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
				SettingsChanged?.Invoke(this, null);
			}
		}

		/// <summary>
		/// The Name of the currently connected Device.
		/// </summary>
		public String DeviceName
		{
			get => "Test"; //TODO;
			set
			{
				//TODO
				SettingsChanged?.Invoke(this, null);
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
				_steps = value;
				SettingsChanged?.Invoke(this, null);
			}
		}

		internal AudioModule CurrentAudioModule
		{
			get => _currentAudioModule;
			private set
			{
				_currentAudioModule = value;
				AudioModuleChanged.Invoke(value);
			}
		}

		public CustomColor CurrentColor
		{
			get => _currentColor;
			set
			{
				_currentColor = value;
				SettingsChanged?.Invoke(this, null);
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
		/// The Constructor that builds a new SettingsHandler
		/// </summary>
		private SettingsHandler()
		{
			outputManager = new OutputManager();
			outputManager.Subscribe(this);
			Steps = 0;
			_langManager = LangManager.SingletonLangManager;
			_properties = Application.Current.Properties;
			AvailableAudioModules = new Dictionary<string, AudioModule>();

			//Colors to use.
			Colors = new List<CustomColor>();
			Colors.Add(new CustomColor(Color.RoyalBlue));
			Colors.Add(new CustomColor(Color.SkyBlue));
			Colors.Add(new CustomColor(Color.DarkRed));
			CurrentColor = Colors[0];

			//Init AudioModules
			AvailableAudioModules.Add("basicAudioModule", new AudioModule(new BasicAudioLib(), new BasicAudioPlayer(), typeof(BasicAudioTrack)));

			//Load AudioModule
			Object val;
			if (_properties.TryGetValue("audioModule", out val))
			{
				AudioModule audioModule;
				if (AvailableAudioModules.TryGetValue(val.ToString(), out audioModule))
				{
					_currentAudioModule = audioModule;
					System.Diagnostics.Debug.WriteLine("AudioModule Chosen: " + val.ToString());
				}
				else
				{
					AvailableAudioModules.TryGetValue("basicAudioModule", out audioModule);
					_currentAudioModule = audioModule;
					System.Diagnostics.Debug.WriteLine("AudioModule Chosen: basicAudioModule");
					_properties.Remove("audioModule");
					_properties.Add("audioModule", "basicAudioModule");
				}
			}
			else
			{
				AudioModule audioModule;
				AvailableAudioModules.TryGetValue("basicAudioModule", out audioModule);
				_currentAudioModule = audioModule;
				System.Diagnostics.Debug.WriteLine("AudioModule Chosen: basicAudioModule");
				_properties.Add("audioModule", "basicAudioModule");
			}

			//Load Chosen Language
			if(_properties.TryGetValue("lang", out val))
			{
				if (_langManager.ChooseLang(val.ToString()))
					System.Diagnostics.Debug.WriteLine("LangChosen: " + val.ToString());
				else
				{
					_langManager.ChooseLang("lang_english");
					_properties.Add("lang", "lang_english");
				}
			} else
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
				} catch (FormatException e)
				{
					_steps = 0;
					_properties.Remove("steps");
					_properties.Add("steps", "0");
				}
			} else
			{
				_steps = 0;
				_properties.Add("steps", "0");
			}


			//TODO
		}

		/// <summary>
		/// Reset the Step counter.
		/// </summary>
		public void ResetSteps()
		{
			//todo
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
			Steps = Steps + value.StepCount;
		}
	}

	public struct CustomColor
	{
		public CustomColor(Color color)
		{
			Color = color;
		}
		public string Name
		{
			get => LangManager.SingletonLangManager.CurrentLang.Get("col_" + this.Color.ToHex());
		}
		public Color Color { get; }

		public static implicit operator Color(CustomColor v)
		{
			throw new NotImplementedException();
		}
	}

	internal struct AudioModule
	{
		internal AudioModule(IAudioLibImpl audioLib, IAudioPlayerImpl audioPlayer, Type audioTrack)
		{
			AudioLib = audioLib;
			AudioPlayer = audioPlayer;
			AudioTrack = audioTrack;
		}
		public IAudioLibImpl AudioLib;
		public IAudioPlayerImpl AudioPlayer;
		public Type AudioTrack;
	}
}
