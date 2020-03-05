using System;

namespace Karl.Model.Exceptions
{
	class NotInitializedException : Exception
	{
		public NotInitializedException(string message) : base(message)
		{
		}
	}
}
