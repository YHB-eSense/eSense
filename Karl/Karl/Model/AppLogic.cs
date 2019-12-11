using System;
using System.Collections.Generic;
using System.Text;

namespace Karl.Model
{
	public class AppLogic
	{

		private AudioLib AudioLib { get; }
		private AudioPlayer AudioPlayer { get; }
		private SettingsHandler SettingsHandler { get; }
		private ModeHandler ModeHandler { get; }
		private ConnectionHandler connection;
		public BluetoothDevice CurrentDevice { get; }
		public int Steps { get; }
		public bool Connected { get; }

		public AppLogic()
		{
			AudioLib = new AudioLib();
			AudioPlayer = new AudioPlayer(AudioLib);
			SettingsHandler = new SettingsHandler(AudioLib, AudioPlayer);
			ModeHandler = new ModeHandler(AudioPlayer);
			connection = new ConnectionHandler();
		}

		public List<BluetoothDevice> SearchDevices()
		{
			//todo
			return connection.SearchDevices();
		}

		public void ConnectDevice(BluetoothDevice device)
		{
			//todo
			connection.ConnectDevice(device);
		}

	}
}
