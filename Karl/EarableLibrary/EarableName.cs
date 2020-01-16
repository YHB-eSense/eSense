using Plugin.BLE.Abstractions.Contracts;
using System.Text;
using System.Threading.Tasks;

namespace EarableLibrary
{
	public class EarableName
	{
		private string _value;
		private readonly ICharacteristic _read, _write;

		public EarableName(ICharacteristic read, ICharacteristic write)
		{
			_read = read;
			_write = write;
		}

		public async Task Initialize()
		{
			_value = await ReadValueAsync();
		}

		public string Value
		{
			get
			{
				return _value;
			}
			set
			{
				_value = value;
				WriteValueAsync(value).Start();
			}
		}

		private async Task<bool> WriteValueAsync(string value)
		{
			return await _write.WriteAsync(Encoding.ASCII.GetBytes(value));
		}

		private async Task<string> ReadValueAsync()
		{
			return Encoding.ASCII.GetString(await _read.ReadAsync());
		}
	}
}
