using System;

namespace EarableLibrary
{

	public interface ISensor
	{
		event EventHandler ValueChanged;
	}
}
