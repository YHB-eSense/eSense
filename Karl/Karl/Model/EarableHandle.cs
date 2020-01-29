using EarableLibrary;

namespace Karl.Model
{
	/// <summary>
	/// This class provides access to EarableLibrary::IEarable from within the Model but hides it from everywhere else.
	/// </summary>
	public class EarableHandle
	{
		internal readonly IEarable _handle;

		/// <summary>
		/// Creates controller for earable
		/// </summary>
		/// <param name="earable"></param>
		internal EarableHandle(IEarable earable)
		{
			_handle = earable;
		}

		/// <summary>
		/// Provides read-only access to the device name
		/// </summary>
		public string Name => _handle.Name;

	}
}
