using Plugin.BLE;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EarableLibrary
{
	/// <summary>
	/// This class serves as the libraries 'entry-point'.
	/// Once you have created an instance of it, IEarabe objects can be retrieved to work with.
	/// </summary>
	public class EarableLibrary : IEarableManager
	{
		/// <summary>
		/// Try establishing a BLE connection to an earable:
		/// Connections to all available earables (<see cref="ListEarables"/>) will be opened successively until one succeeds.
		/// </summary>
		/// <returns>The first successfully connected earable or null if all failed</returns>
		public async Task<IEarable> ConnectEarableAsync()
		{
			var earables = ListEarables();
			if (earables.Count == 0) return null;
			foreach (var earable in earables) {
				if (await earable.ConnectAsync()) return earable;
			}
			return null;
		}

		/// <summary>
		/// List earables which are or can be connected.
		/// Earables must have been paired in system settings to show up here.
		/// A filter is applied, trying to sort out incompatible devices (non-earables).
		/// Whatsoever, no guarantee is given that the returned devices are really compatible. 
		/// </summary>
		/// <returns>List of available earables</returns>
		public List<IEarable> ListEarables()
		{
			var adapter = CrossBluetoothLE.Current.Adapter;
			var devices = adapter.GetSystemConnectedOrPairedDevices(services: ESense.ServiceUuids);
			var earables = new List<IEarable>(devices.Count);
			foreach (var dev in devices) {
				earables.Add(new ESense(dev));
			}
			return earables;
		}
	}
}
