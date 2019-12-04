using System;

namespace Karl.Model
{

	public interface ISensor
	{
		event EventHandler ValueChanged;
	}
}
