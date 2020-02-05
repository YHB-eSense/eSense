using System;
using System.Threading.Tasks;

namespace EarableLibrary
{
	/// <summary>
	/// Represents an earable device, which supports connection over BLE.
	/// </summary>
	public interface IEarable
	{
		/// <summary>
		/// Get the device name.
		/// </summary>
		string Name { get; }

		/// <summary>
		/// Asynchronously update the device name.
		/// </summary>
		/// <param name="name">New name</param>
		/// <returns>Whether the name could be changed successfully or not</returns>
		Task<bool> SetNameAsync(string name);

		/// <summary>
		/// The unique device id.
		/// </summary>
		Guid Id { get; }

		/// <summary>
		/// Retrieve an object for one of the builtin sensors.
		/// </summary>
		/// <typeparam name="T">Type of the sensor</typeparam>
		/// <returns>Sensor object if earable has this kind of sensor built in, null otherwise</returns>
		T GetSensor<T>() where T : ISensor;

		/// <summary>
		/// Try connecting to the earable.
		/// </summary>
		/// <returns>true on success, false on fail or if already connected</returns>
		Task<bool> ConnectAsync();

		/// <summary>
		/// Disconnect from the earable.
		/// </summary>
		/// <returns>true on success, false if already disconnected (or never connected)</returns>
		Task<bool> DisconnectAsync();


		/// <summary>
		/// Retrieve connection state.
		/// </summary>
		/// <returns>Whether the earable is connected or not</returns>
		bool IsConnected();
	}
}
