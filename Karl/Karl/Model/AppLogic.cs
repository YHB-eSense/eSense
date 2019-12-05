using System;
using System.Collections.Generic;
using System.Text;

namespace Karl.Model
{
	public class AppLogic
	{

		private AudioLib audioLib { get; }
		private AudioPlayer audioPlayer { get; }
		private SettingsHandler settingsHandler { get; }
		private ModeHandler modeHandler { get; }
		private ConnectionHandler connection;

		public AppLogic()
		{
			audioLib = new AudioLib();
			settingsHandler = new SettingsHandler(audioLib);
			audioPlayer = new AudioPlayer();
			modeHandler = new ModeHandler(audioPlayer);
			connection = new ConnectionHandler();
		}
	}
}
