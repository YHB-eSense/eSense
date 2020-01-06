using System;
using System.Collections.Generic;
using System.Text;
using EarableLibrary;

namespace Karl.Model
{
	/// <summary>
	/// This class provides access to EarableLibrary::IEarable from within the Model but hides it from everywhere else.
	/// </summary>
	public class EarableHandle
	{
		internal readonly IEarable handle;
		internal EarableHandle(IEarable earable)
		{
			this.handle = earable;
		}

		/// <summary>
		/// Provides read-only access to the device name
		/// </summary>
		public string Name => handle.Name;

	}
}
