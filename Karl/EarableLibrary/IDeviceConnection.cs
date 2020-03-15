using System;
using System.Threading.Tasks;

namespace EarableLibrary
{
	/// <summary>
	/// Delegate for accepting data that comes from the BLE device.
	/// </summary>
	/// <param name="data">Binary data</param>
	public delegate void DataReceiver(byte[] data);

	public interface IDeviceConnection
	{
		ConnectionState State { get; }

		Guid Id { get; }

		Task<bool> Open();

		Task<bool> Close();

		Task<bool> WriteAsync(Guid charId, byte[] val);

		Task<byte[]> ReadAsync(Guid charId);

		Task SubscribeAsync(Guid charId, DataReceiver handler);

		Task UnsubscribeAsync(Guid charId, DataReceiver handler);

		Task<bool> Validate(Guid[] serviceIds);

	}
}
