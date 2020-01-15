using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.EventArgs;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace EarableLibrary
{
	public class EarableLibrary : IEarableScanner
	{
		public event EventHandler<EarableEventArgs> EarableDiscovered; // EarableDisconnected

		public int ScanTimeout { get; set; }

		public bool IsScanning
		{
			get => CrossBluetoothLE.Current.Adapter.IsScanning;
		}

		public EarableLibrary()
		{
			var adapter = CrossBluetoothLE.Current.Adapter;
			adapter.DeviceDiscovered += DeviceDiscovered;
			// adapter.DeviceConnectionLost += DeviceConnectionLost;
			// adapter.ScanTimeoutElapsed += ScanTimeoutElapsed;
		}

		private void DeviceDiscovered(object sender, DeviceEventArgs e)
		{
			var device = new ESense(e.Device);
			var args = new EarableEventArgs(device);
			EarableDiscovered?.Invoke(this, args);
		}

		public async Task StartScanningAsync()
		{
			var adapter = CrossBluetoothLE.Current.Adapter;
			adapter.ScanMode = ScanMode.LowLatency;
			adapter.ScanTimeout = ScanTimeout;
			await adapter.StartScanningForDevicesAsync(serviceUuids:ESense.RequiredServiceUuids);
		}

		public async Task StopScanningAsync()
		{
			var adapter = CrossBluetoothLE.Current.Adapter;
			await adapter.StopScanningForDevicesAsync();
		}
	}
}
