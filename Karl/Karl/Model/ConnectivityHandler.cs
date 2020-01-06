using System;
using System.Collections.Generic;
using System.Text;
using EarableLibrary;
using System.Collections.ObjectModel;

namespace Karl.Model
{
	/// <summary>
	/// This class handles the conntection to the Earables Library and Step Detection Library
	/// </summary>
	public class ConnectivityHandler
	{

		private static ConnectivityHandler _connectivityHandler;
		public static ConnectivityHandler SingletonConnectivityHandler
		{
			get
			{
				if (_connectivityHandler == null)
				{
					_connectivityHandler = new ConnectivityHandler();
				}
				return _connectivityHandler;
			}
		}

		private IEarable Earable;
		public BluetoothDevice CurrentDevice { get; private set; }
		public bool Connected { get; }
		public ObservableCollection<BluetoothDevice> FoundDevices { get; private set; } //todo

		private ConnectivityHandler()
		{
			//todo
		}

		/// <summary>
		/// Search for Bluetooth Devices.
		/// </summary>
		/// <returns>List of Bluetooth Devices</returns>
		public void SearchDevices()
		{
			//todo
		}

		/// <summary>
		/// Connect to the Device given as a parameter.
		/// </summary>
		/// <param name="device">Device to connect with.</param>
		public void ConnectDevice(BluetoothDevice device)
		{
			//todo
		}

		public void Disconnect()
		{
			//todo
		}

		/// <summary>
		/// Set a new Device name.
		/// </summary>
		/// <param name="name">The new Name.</param>
		public void SetDeviceName(String name)
		{
			//todo
		}

	}
}
