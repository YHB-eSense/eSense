using System.Collections.Generic;
using System.Threading.Tasks;

namespace EarableLibrary
{
	/// <summary>
	/// Interface which provides access to earables.
	/// </summary>
	public interface IEarableManager
	{
		/// <summary>
		/// List earables which are available (can be connected to).
		/// </summary>
		/// <returns>List of all available earables</returns>
		List<IEarable> ListEarables();


		/// <summary>
		/// Try connecting to one of the available earable.
		/// Implementation determines which available earable is chosen.
		/// </summary>
		/// <returns>Earable which has been successfully connected or null</returns>
		Task<IEarable> ConnectEarableAsync();
	}
}
