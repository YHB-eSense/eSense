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

		private void DeviceDiscovered(object sender, DeviceEventArgs e)
		{
			var device = new ESense(e.Device);
			var args = new EarableEventArgs(device);
			EarableDiscovered?.Invoke(this, args);
		}

		public void StartScanning()
		{
			Debug.WriteLine("Starting Scan...");
			CrossBluetoothLE.Current.Adapter.StartScanningForDevicesAsync(serviceUuids:ESense.RequiredServiceUuids);
			}

		public void StopScanning()
		{
			Debug.WriteLine("Stopping Scan...");
			CrossBluetoothLE.Current.Adapter.StopScanningForDevicesAsync();
		}
	}
}
