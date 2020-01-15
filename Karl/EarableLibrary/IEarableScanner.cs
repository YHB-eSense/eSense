using System.Threading.Tasks;

namespace EarableLibrary
{
	public interface IEarableManager
	{
		Task<IEarable> ConnectEarableAsync();
	}
}
