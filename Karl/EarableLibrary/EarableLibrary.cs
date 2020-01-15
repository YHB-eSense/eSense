using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;
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

		public int ScanTimeout { get; set; }

		public EarableLibrary(int scanTimeout)
		{
			ScanTimeout = scanTimeout;
			var adapter = CrossBluetoothLE.Current.Adapter;
			adapter.DeviceDiscovered += DeviceDiscovered;
			adapter.ScanTimeoutElapsed += (s, e) => ScanningStopped.Invoke(this, null);
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
			var adapter = CrossBluetoothLE.Current.Adapter;
			adapter.ScanMode = ScanMode.LowLatency;
			adapter.ScanTimeout = ScanTimeout;
			await adapter.StartScanningForDevicesAsync(serviceUuids:ESense.RequiredServiceUuids);
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
