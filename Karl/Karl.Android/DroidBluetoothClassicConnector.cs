using System.Collections.Generic;
using Android.Bluetooth;
using Android.Content;
using Karl.Model;
using Xamarin.Forms;

[assembly: Dependency(typeof(IBluetoothClassicConnector))]
namespace Karl.Droid
{

	public class DroidBluetoothClassicConnector : IBluetoothClassicConnector
	{
		public void ConnectToDevice(string devicename)
		{
			
		}

		public List<string> FindAvailableDevices()
		{
			BluetoothAdapter adapter = BluetoothAdapter.DefaultAdapter;
			if (adapter == null ) {
				return null;
			}
			if (!adapter.IsEnabled) {
				adapter.Enable();
			}
			BluetoothDeviceReceiver bd = new BluetoothDeviceReceiver();
			bd.OnReceive(Android.App.Application.Context, );
			return null;
		}

		
	}
}
