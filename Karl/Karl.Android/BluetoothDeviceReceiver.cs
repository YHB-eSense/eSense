using System;
using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Content;
using Android.Bluetooth;

namespace Karl.Droid
{
	public class BluetoothDeviceReceiver : BroadcastReceiver
	{
		public override void OnReceive(Context context, Intent intent)
		{
			var action = intent.Action;

			if (action != BluetoothDevice.ActionFound)
			{
				return;
			}

			// Get the device
			var device = (BluetoothDevice)intent.GetParcelableExtra(BluetoothDevice.ExtraDevice);

			if (device.BondState != Bond.Bonded)
			{
				Console.WriteLine($"Found device with name: {device.Name} and MAC address: {device.Address}");
			}
		}
	}
}
