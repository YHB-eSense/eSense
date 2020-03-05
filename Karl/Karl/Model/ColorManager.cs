using System.Collections.Generic;
using Xamarin.Forms;

namespace Karl.Model
{
	/// <summary>
	/// 
	/// </summary>
	public class ColorManager
	{
		private static ColorManager _singletonColorManager;

		/// <summary>
		/// 
		/// </summary>
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
		/// <summary>
		/// 
		/// </summary>
		public List<CustomColor> Colors { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public CustomColor CurrentColor { get; set; }

		private ColorManager()
		{
			Colors = new List<CustomColor>();
			Colors.Add(new CustomColor(Color.RoyalBlue));
			Colors.Add(new CustomColor(Color.SkyBlue));
			Colors.Add(new CustomColor(Color.DarkRed));
			Colors.Add(new CustomColor(Color.ForestGreen));
			Colors.Add(new CustomColor(Color.Orange));
		}

		/// <summary>
		/// 
		/// </summary>
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

	/// <summary>
	/// 
	/// </summary>
	public class CustomColor
	{
		/// <summary>
		/// 
		/// </summary>
		public Color Color { get; }

		/// <summary>
		/// 
		/// </summary>
		public string Name
		{
			get => LangManager.SingletonLangManager.CurrentLang.Get("col_" + Color.ToHex());
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="color"></param>
		public CustomColor(Color color)
		{
			Color = color;
		}
	}

}
