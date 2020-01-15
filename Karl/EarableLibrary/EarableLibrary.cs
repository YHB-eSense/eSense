using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;
using System.Threading.Tasks;

namespace EarableLibrary
{
	public class EarableLibrary : IEarableManager
	{
		public async Task<IEarable> ConnectEarableAsync()
		{
			var adapter = CrossBluetoothLE.Current.Adapter;
			var devices = adapter.GetSystemConnectedOrPairedDevices(services: ESense.RequiredServiceUuids);
			if (devices.Count == 0) return null;
			foreach (var dev in devices)
			{
				ESense earable = new ESense(dev);
				if (await earable.ConnectAsync()) return earable;
			}
			return null;
		}
	}
}
