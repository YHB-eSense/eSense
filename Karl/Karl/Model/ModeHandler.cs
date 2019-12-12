using System;
using System.Collections.Generic;
using System.Text;

namespace Karl.Model
{
	public class ModeHandler 
	{
		private AudioPlayer audioPlayer;

		public List<Mode> Modes { get; private set; }


		internal ModeHandler(AudioPlayer audioPlayer)
		{
			this.audioPlayer = audioPlayer;
			Modes = new List<Mode>();
			Modes.Add(new AutostopMode());
			Modes.Add(new MotivateMode());
		}

		public void ActivateMode()
		{
			//todo
		}

		public void DeactivateMode()
		{
			//todo
		}
	}
}
