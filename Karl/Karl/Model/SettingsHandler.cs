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
		private static LangManager _langManager;
		private static SettingsHandler _singletonSettingsHandler;

		/// <summary>
		/// The List of registered Languages.
		/// </summary>
		public List<Lang> Languages { get => _langManager.AvailableLangs; }

		/// <summary>
		/// The currently selected Language.
		/// </summary>
		public Lang CurrentLang {
			get => _langManager.CurrentLang;
			set { _langManager.CurrentLang = value; }
		}

		/// <summary>
		/// The Name of the currently connected Device.
		/// </summary>
		public String DeviceName { get; set; }

		/// <summary>
		/// The total Steps done.
		/// </summary>
		public int Steps { get => 0; }

		public List<CustomColor> Colors { get; }

		public CustomColor CurrentColor { get; set; }

		public static SettingsHandler SingletonSettingsHandler
		{
			get
			{
				if (_singletonSettingsHandler == null)
				{
					_singletonSettingsHandler = new SettingsHandler();
				}
				return _singletonSettingsHandler;
			}
		}

		/// <summary>
		/// The Constructor that builds a new SettingsHandler
		/// </summary>
		private SettingsHandler()
		{
			_langManager = LangManager.SingletonLangManager;
			Colors = new List<CustomColor>();
			Colors.Add(new CustomColor(Color.RoyalBlue, "RoyalBlue"));
			Colors.Add(new CustomColor(Color.SkyBlue, "SkyBlue"));
			Colors.Add(new CustomColor(Color.DarkRed, "DarkRed"));
			CurrentColor = Colors[0];
		}

		/// <summary>
		/// Reset the Step counter.
		/// </summary>
		public void ResetSteps()
		{
			//todo
		}
	}

	public class CustomColor
	{
		public CustomColor(Color color, string name)
		{
			Color = color;
			Name = name;
		}
		public string Name { get; }

		public Color Color { get; }

		public static implicit operator Color(CustomColor v)
		{
			throw new NotImplementedException();
		}
	}
}
