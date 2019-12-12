using System;
using System.Collections.Generic;
using System.Text;
using EarableLibrary;

namespace Karl.Model
{
	/// <summary>
	/// This is a general Bluetooth Device. You can get it's name.
	/// </summary>
	public class BluetoothDevice
	{
		private IEarable _earable;
		internal BluetoothDevice(IEarable earable)
		{
			
		}
		//todo
		public string Name { get; private set; }

		/// <summary>
		/// For Testing only!!! Will disappear late.
		/// </summary>
		/// <param name="name">The name</param>
		public BluetoothDevice(string name)
		{
			Name = name;
		}
		/// <summary>
		/// Connect with this device.
		/// </summary>
		public void Connect()
		{
			//todo
		}
		/// <summary>
		/// Disconnect with this device.
		/// </summary>
		public void Disconnect()
		{
			//todo
		}


	}
}
