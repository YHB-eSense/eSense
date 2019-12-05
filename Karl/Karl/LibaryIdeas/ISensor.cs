using System;

namespace Karl.LibraryIdeas
{

	public interface ISensor
	{
		event EventHandler ValueChanged;
	}
}
