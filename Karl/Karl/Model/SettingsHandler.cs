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
	public class SettingsHandler
	{
		private LangManager _langManager;
		private ConnectivityHandler _connectivityHandler;
		private static SettingsHandler _singletonSettingsHandler;
		private static readonly Object _padlock = new Object();
		private CustomColor _currentColor;
		private AudioModule _currentAudioModule;
		private int _steps;
		private OutputManager _outputManager;
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
			get
			{
				if (_connectivityHandler.EarableConnected) { return _connectivityHandler.EarableName; }
				return null;
			}
			set
			{
				_connectivityHandler.SetDeviceName(value);
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
				if (_properties.ContainsKey("steps")) _properties.Remove("steps");
				_properties.Add("steps", value.ToString());
				_steps = value;
				SettingsChanged?.Invoke(this, null);
			}
		}

		internal AudioModule CurrentAudioModule
		{
			get => _currentAudioModule;
			private set
			{
				if (_properties.ContainsKey("audioModule")) _properties.Remove("audioModule");
				_properties.Add("audioModule", value.Tag);
				_currentAudioModule = value;
				AudioModuleChanged.Invoke(value);
			}
		}

		public CustomColor CurrentColor
		{
			get => _currentColor;
			set
			{
				if (_properties.ContainsKey("color")) _properties.Remove("color");
				_properties.Add("color", value.Color.ToHex());
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
		/// Resets the Step Counter.
		/// </summary>
		public void ResetSteps()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// The Constructor that builds a new SettingsHandler
		/// </summary>
		private SettingsHandler()
		{
			_connectivityHandler = ConnectivityHandler.SingletonConnectivityHandler;
			_outputManager = new OutputManager();
			_outputManager.Subscribe(new StepDetectionObserver(this));
			_langManager = LangManager.SingletonLangManager;
			_properties = Application.Current.Properties;
			AvailableAudioModules = new Dictionary<string, AudioModule>();

			//Colors to use.
			Colors = new List<CustomColor>();
			Colors.Add(new CustomColor(Color.RoyalBlue));
			Colors.Add(new CustomColor(Color.SkyBlue));
			Colors.Add(new CustomColor(Color.DarkRed));

			//Init AudioModules
			AvailableAudioModules.Add("basicAudioModule",
				new AudioModule(new BasicAudioLib(), new BasicAudioPlayer(), typeof(BasicAudioTrack), "basicAudioModule"));

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
			} else
			{
				_properties.Add("color", Colors[0].Color.ToHex());
				CurrentColor = Colors[0];
			}
			//Load AudioModule
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
			}
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
		internal AudioModule(IAudioLibImpl audioLib, IAudioPlayerImpl audioPlayer, Type audioTrack, string tag)
		{
			Tag = tag;
			AudioLib = audioLib;
			AudioPlayer = audioPlayer;
			AudioTrack = audioTrack;
		}
		public string Tag;
		public IAudioLibImpl AudioLib;
		public IAudioPlayerImpl AudioPlayer;
		public Type AudioTrack;
	}
}
