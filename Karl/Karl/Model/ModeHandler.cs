using System.Collections.Generic;

namespace Karl.Model
{
	/// <summary>
	/// This class handles the modes. It can give you a List of all registered modes.
	/// </summary>
	public class ModeHandler 
	{
		private static ModeHandler _singletonModeHandler;

		/// <summary>
		/// The singleton object of ModeHandler
		/// </summary>
		public static ModeHandler SingletonModeHandler
		{
			get
			{
				if (_singletonModeHandler == null) { _singletonModeHandler = new ModeHandler(); }
				return _singletonModeHandler;
			}
		}

		/// <summary>
		/// List of all registered modes
		/// </summary>
		public List<Mode> Modes { get; private set; }

		/// <summary>
		/// Constructor of ModeHandler
		/// </summary>
		private ModeHandler()
		{
			Modes = new List<Mode> { new AutostopMode(), new MotivationMode() };
		}

		/// <summary>
		/// Reset all registered modes
		/// </summary>
		public void ResetModes()
		{
			if(Modes != null)
			{
				List<Mode> newModes = new List<Mode>(Modes);
				Modes.Clear();
				Modes = newModes;
			}
		}

	}
}
