using Plugin.BLE;
using Plugin.BLE.Abstractions.EventArgs;
using System;
using System.Diagnostics;

namespace EarableLibrary
{
	public class EarableLibrary : IEarableScanner
	{
		public event EventHandler<EarableEventArgs> EarableDiscovered;

		public EarableLibrary()
		{
			CrossBluetoothLE.Current.Adapter.DeviceDiscovered += DeviceDiscovered;
		}

		private async void DeviceDiscovered(object sender, DeviceEventArgs e)
		{
			Debug.WriteLine("Discovered Device");
			ESense device = new ESense(e.Device);
			await device.Initialize();
			if (device.CanConnect())
			{
				EarableEventArgs args = new EarableEventArgs(device);
				EarableDiscovered?.Invoke(this, args);
			}
		}

		public void StartScanning()
		{
			Debug.WriteLine("Starting Scan...");
			CrossBluetoothLE.Current.Adapter.StartScanningForDevicesAsync();
		}

		public void StopScanning()
		{
			Debug.WriteLine("Stopping Scan...");
			CrossBluetoothLE.Current.Adapter.StopScanningForDevicesAsync();
		}
	}
}
