using System;
using System.Collections.Generic;
using System.Text;

namespace Karl.Model
{
	public class ModeHandler
	{
		private AudioPlayer audioPlayer;
		List<Mode> modes;

		internal ModeHandler(AudioPlayer audioPlayer)
		{
			this.audioPlayer = audioPlayer;
		}
	}
}
