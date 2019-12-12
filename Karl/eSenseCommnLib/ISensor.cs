using System;

namespace eSenseCommnLib
{

	public interface ISensor
	{
		event EventHandler ValueChanged;
	}
}
