using System;
using System.Collections.Generic;
using System.Text;

namespace Karl.Model
{
	/// <summary>
	/// This is the Class that coordinates the model.
	/// From here you can access the main interface classes and handle the earable connection.
	/// </summary>
	public class AppLogic
	{
		/// <summary>
		/// This is the AudioLib Wrapper. Use only this.
		/// If the used Audiomodule is changed, the implementation will be changed without the interface changing.
		/// </summary>
		public AudioLib AudioLib { get; private set; }
		/// <summary>
		///This is the AudioPlayer Wrapper.Use only this.
		/// If the used Audiomodule is changed, the implementation will be changed without the interface changing.
		/// </summary>
		public AudioPlayer AudioPlayer { get; private set; }
		/// <summary>
		/// This is the SettingsHandler. It manages the app settings.
		/// </summary>
		public SettingsHandler SettingsHandler { get; private set; }
		/// <summary>
		/// This is the Mode Handler. It will give you a List of available modes.
		/// </summary>
		public ModeHandler ModeHandler { get; private set; }
		//This handles the connection to the earables.
		private ConnectionHandler Connection;
		public BluetoothDevice CurrentDevice { get; }
		public int Steps { get; }
		public bool Connected { get; }

		public AppLogic()
		{
			AudioLib = new AudioLib();
			AudioPlayer = new AudioPlayer(AudioLib);
			SettingsHandler = new SettingsHandler(AudioLib, AudioPlayer);
			ModeHandler = new ModeHandler(AudioPlayer);
			Connection = new ConnectionHandler();
		}

		public List<BluetoothDevice> SearchDevices()
		{
			//todo
			return Connection.SearchDevices();
		}

		public void ConnectDevice(BluetoothDevice device)
		{
			//todo
			Connection.ConnectDevice(device);
		}
		public void DisconnectDevice()
		{
			//todo
		}

	}
}
