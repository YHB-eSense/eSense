using Plugin.BLE.Abstractions.Contracts;
using System;
using System.Text;
using System.Threading.Tasks;

namespace EarableLibrary
{
	/// <summary>
	/// Wrapper class which bundles read- and write-functionality for the device name.
	/// Stores a local copy of the remote value, which allows for synchronous read operations.
	/// </summary>
	public class EarableName
	{
		private string _name;
		private readonly ICharacteristic _read, _write;

		/// <summary>
		/// Construct a new EarableName.
		/// </summary>
		/// <param name="read">Characteristic which allows read-access to the name</param>
		/// <param name="write">Characteristic which allows write-access to the name</param>
		public EarableName(ICharacteristic read, ICharacteristic write)
		{
			_read = read;
			_write = write;
		}

		/// <summary>
		/// Initialize the names local copy by retrieving its current remote value.
		/// </summary>
		public async Task Initialize()
		{
			var bytes = await _read.ReadAsync();
			_name = Encoding.ASCII.GetString(bytes);
		}

		/// <summary>
		/// Asynchronously update the remote device name.
		/// If successful, the local copy is updated too.
		/// </summary>
		/// <param name="value">New device name</param>
		/// <returns>true if write succeeded, false otherwise</returns>
		public async Task<bool> SetAsync(string value)
		{
			var success = await _write.WriteAsync(Encoding.ASCII.GetBytes(value));
			if (success) _name = value;
			return success;
		}

		/// <summary>
		/// Retrieve the device names local copy.
		/// </summary>
		/// <returns>Device name or null if uninitialized</returns>
		public string Get()
		{
			return _name;
		}
	}
}
