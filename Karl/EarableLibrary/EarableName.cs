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

		private string _name;
		private readonly BLEConnection _conn;

		/// <summary>
		/// Construct a new EarableName.
		/// </summary>
		/// <param name="read">Characteristic which allows read-access to the name</param>
		/// <param name="write">Characteristic which allows write-access to the name</param>
		public EarableName(BLEConnection conn)
		{
			_conn = conn;
			_conn.Read(CHAR_NAME_R, ValueAvailable);
		}

		private void ValueAvailable(byte[] data)
		{
			_name = Encoding.ASCII.GetString(data);
		}

		/// <summary>
		/// Update the remote device name and it's local copy.
		/// </summary>
		/// <param name="value">New device name</param>
		public void Set(string value)
		{
			var msg = new RawMessage()
			{
				Data = Encoding.ASCII.GetBytes(value)
			};
			_conn.Write(CHAR_NAME_W, msg);
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
