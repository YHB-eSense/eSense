using Plugin.BLE;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace EarableLibrary
{
	public class EarableLibrary : IEarableScanner
	{
		public event EventHandler<EarableEventArgs> EarableDiscovered;

		public EarableLibrary()
		{
			CrossBluetoothLE.Current.Adapter.DeviceDiscovered += DeviceDiscovered;
		}

		private async void DeviceDiscovered(object sender, Plugin.BLE.Abstractions.EventArgs.DeviceEventArgs e)
		{
			ESense device = new ESense(e.Device);
			bool valid = await device.ValidateServicesAsync();
			if (valid)
			{
				EarableEventArgs args = new EarableEventArgs(device);
				EarableDiscovered.Invoke(this, args);
			}
		}

		public void StartScanning()
		{
			CrossBluetoothLE.Current.Adapter.StartScanningForDevicesAsync();
		}

		public void StopScanning()
		{
			CrossBluetoothLE.Current.Adapter.StopScanningForDevicesAsync();
		}
	}
}
