using System;
using System.Collections.Generic;
using System.Text;

namespace Karl.Model.Exceptions
{
	class NotInitializedException : Exception
	{
		public NotInitializedException(string message) : base(message)
		{
		}
	}
}
