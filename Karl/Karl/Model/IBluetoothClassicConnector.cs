using System;
using System.Collections.Generic;
using System.Text;

namespace Karl.Model
{
	public interface IBluetoothClassicConnector
	{
		List<String> FindAvailableDevices();
		void ConnectToDevice(string devicename);
	}
}
