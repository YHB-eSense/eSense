using System;

namespace EarableLibrary
{

	public interface ISensor<T>
	{
		event EventHandler<T> ValueChanged;
	}
}
