using System;
using System.Collections.Generic;
using System.Text;
using EarableLibrary;

namespace Karl.Model
{
	/// <summary>
	/// This class handles the conntection to the Earables Library and Step Detection Library
	/// </summary>
	internal class ConnectionHandler
	{
		private IEarable Earable;
		/// <summary>
		/// Search for Bluetooth Devices.
		/// </summary>
		/// <returns>List of Bluetooth Devices</returns>
		internal List<BluetoothDevice> SearchDevices()
		{
			//todo
			return null;
		}
		/// <summary>
		/// Connect to the Device given as a parameter.
		/// </summary>
		/// <param name="device">Device to connect with.</param>
		internal void ConnectDevice(BluetoothDevice device)
		{
			//todo
		}
		/// <summary>
		/// Set a new Device name.
		/// </summary>
		/// <param name="name">The new Name.</param>
		internal void SetDeviceName(String name)
		{
			//todo
		}


	}
}
