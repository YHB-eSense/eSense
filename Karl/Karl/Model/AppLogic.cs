using System;
using System.Collections.Generic;
using System.Text;
using EarableLibrary;

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
		public AudioLib AudioLib
		{
			get
			{
				return AudioLib.SingletonAudioLib;
			}
		}
		/// <summary>
		///This is the AudioPlayer Wrapper.Use only this.
		/// If the used Audiomodule is changed, the implementation will be changed without the interface changing.
		/// </summary>
		public AudioPlayer AudioPlayer
		{
			get
			{
				return AudioPlayer.SingletonAudioPlayer;
			}
		}
		/// <summary>
		/// This is the SettingsHandler. It manages the app settings.
		/// </summary>
		public SettingsHandler SettingsHandler
		{
			get
			{
				return SettingsHandler.SingletonSettingsHandler;
			}
		}
		/// <summary>
		/// This is the Mode Handler. It will give you a List of available modes.
		/// </summary>
		public ModeHandler ModeHandler
		{
			get
			{
				return ModeHandler.SingletonModeHandler;
			}
		}
		//This handles the connection to the earables.
		private ConnectionHandler Connection;
		/// <summary>
		/// The current Device for access from VM
		/// </summary>
		public BluetoothDevice CurrentDevice { get; }
		/// <summary>
		/// The total steps.
		/// </summary>
		public int Steps { get; }
		/// <summary>
		/// Is a Device connected?
		/// </summary>
		public bool Connected { get; }
		/// <summary>
		/// Create a new Model.
		/// </summary>
		public AppLogic()
		{
			Connection = new ConnectionHandler();
		}
		/// <summary>
		/// Find Bluetooth Devices in Range.
		/// </summary>
		/// <returns>List of Bluetooth Devices</returns>
		public List<BluetoothDevice> SearchDevices()
		{
			//todo
			return Connection.SearchDevices();
		}
		/// <summary>
		/// Connect with this Device
		/// </summary>
		/// <param name="device">Device to connect with.</param>
		public void ConnectDevice(BluetoothDevice device)
		{
			//todo
			Connection.ConnectDevice(device);
		}
		/// <summary>
		/// Disconnect from current Device.
		/// </summary>
		public void DisconnectDevice()
		{
			//todo
		}

	}
}
