using System;
using System.Collections.Generic;
using System.Text;

namespace Karl.Model
{
	public class ModeHandler
	{
		private AudioPlayer audioPlayer;
		public List<Mode> Modes { get; }


		internal ModeHandler(AudioPlayer audioPlayer)
		{
			this.audioPlayer = audioPlayer;
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
