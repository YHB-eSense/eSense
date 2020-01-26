using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Karl.Model
{
	public class ColorManager
	{
		private static ColorManager _singletonColorManager;
		public static ColorManager SingletonColorManager
		{
			get
			{
				{
					if (_singletonColorManager == null) { _singletonColorManager = new ColorManager(); }
					return _singletonColorManager;
				}
			}
		}
		public List<CustomColor> Colors { get; set; }
		public CustomColor CurrentColor { get; set; }

		private ColorManager()
		{
			Colors = new List<CustomColor>();
			Colors.Add(new CustomColor(Color.RoyalBlue));
			Colors.Add(new CustomColor(Color.SkyBlue));
			Colors.Add(new CustomColor(Color.DarkRed));
		}

		public void ResetColors()
		{
			if (Colors != null)
			{
				List<CustomColor> newColors = new List<CustomColor>(Colors);
				Colors.Clear();
				Colors = newColors;
			}
		}

	}

	public class CustomColor
	{
		public Color Color { get; }
		public string Name
		{
			get => LangManager.SingletonLangManager.CurrentLang.Get("col_" + Color.ToHex());
		}

		public CustomColor(Color color)
		{
			Color = color;
		}
	}

}
