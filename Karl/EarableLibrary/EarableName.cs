using Plugin.BLE.Abstractions.EventArgs;
using Plugin.BLE.Abstractions.Extensions;
using System;
using System.Text;
using System.Threading.Tasks;

namespace EarableLibrary
{
	/// <summary>
	/// Wrapper class which bundles read- and write-functionality for the device name.
	/// Stores a local copy of the remote value, which allows for synchronous read operations.
	/// </summary>
	internal class EarableName
	{
		private static readonly Guid CHAR_NAME_R = GuidExtension.UuidFromPartial(0x2A00);
		private static readonly Guid CHAR_NAME_W = GuidExtension.UuidFromPartial(0xFF0C);

		private readonly BLEConnection _conn;
		private string _name;

		/// <summary>
		/// Construct a new EarableName.
		/// </summary>
		/// <param name="read">Characteristic which allows read-access to the name</param>
		/// <param name="write">Characteristic which allows write-access to the name</param>
		public EarableName(BLEConnection conn)
		{
			_conn = conn;
		}

		/// <summary>
		/// Update the remote device name and it's local copy.
		/// </summary>
		/// <param name="value">New device name</param>
		public async Task<bool> SetAsync(string value)
		{
			var msg = new RawMessage()
			{
				Data = Encoding.ASCII.GetBytes(value)
			};
			bool success = await _conn.WriteAsync(CHAR_NAME_W, msg);
			_name = value;
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

		public async Task Initialize()
		{
			var data = await _conn.ReadAsync(CHAR_NAME_R);
			_name = Encoding.ASCII.GetString(data);
		}
	}
}
