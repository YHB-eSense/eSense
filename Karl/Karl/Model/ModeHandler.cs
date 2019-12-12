using System;
using System.Collections.Generic;
using System.Text;

namespace Karl.Model
{
	/// <summary>
	/// This class handles the modes. It can give you a List of all registered modes.
	/// </summary>
	public class ModeHandler 
	{
		private AudioPlayer _audioPlayer;
		/// <summary>
		/// All registered modes.
		/// </summary>
		public List<Mode> Modes { get; private set; }

		/// <summary>
		/// Constructor of the mode handler.
		/// </summary>
		/// <param name="audioPlayer">The audioplayer that is the modes do stuff on.</param>
		internal ModeHandler(AudioPlayer audioPlayer)
		{
			this._audioPlayer = audioPlayer;
			Modes = new List<Mode>();
			Modes.Add(new AutostopMode());
			Modes.Add(new MotivateMode());
		}
	}
}
