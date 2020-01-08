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
			try
			{
				Debug.WriteLine("Discovered Device '{0}' ({1})", e.Device.Name, e.Device.Id);
				ESense device = new ESense(e.Device);
				bool success = await device.ConnectAsync();
				if (!success) return;
				await device.Initialize();
				await device.DisconnectAsync();
				if (device.IsValid())
				{
					EarableEventArgs args = new EarableEventArgs(device);
					EarableDiscovered?.Invoke(this, args);
				}
			}
			catch(Exception ex)
			{
				Debug.WriteLine("An error occurred after a device has been discovered: " + ex);
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
