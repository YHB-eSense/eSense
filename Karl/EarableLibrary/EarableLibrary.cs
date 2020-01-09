using Plugin.BLE;
using Plugin.BLE.Abstractions.EventArgs;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace EarableLibrary
{
	public class EarableLibrary : IEarableScanner
	{
		public event EventHandler<EarableEventArgs> EarableDiscovered;

		public event EventHandler ScanningStarted, ScanningStopped;

		public EarableLibrary(int scanTimeout)
		{
			var adapter = CrossBluetoothLE.Current.Adapter;
			//adapter.ScanTimeout = scanTimeout;
			adapter.DeviceDiscovered += DeviceDiscovered;
			// adapter.ScanTimeoutElapsed += ScanningStopped.Invoke;
		}

		private void DeviceDiscovered(object sender, DeviceEventArgs e)
		{
			var device = new ESense(e.Device);
			var args = new EarableEventArgs(device);
			EarableDiscovered?.Invoke(this, args);
		}

		public async Task StartScanningAsync()
		{
			Debug.WriteLine("Starting Scan...");
			await CrossBluetoothLE.Current.Adapter.StartScanningForDevicesAsync(serviceUuids:ESense.RequiredServiceUuids);
			ScanningStarted?.Invoke(this, null);
		}

		public async Task StopScanningAsync()
		{
			Debug.WriteLine("Stopping Scan...");
			await CrossBluetoothLE.Current.Adapter.StopScanningForDevicesAsync();
			ScanningStopped?.Invoke(this, null);
		}
	}
}
