using System;
using System.Collections.Generic;
using System.Text;

namespace Karl.Model
{
	/// <summary>
	/// The Autostop Mode.
	/// </summary>
	class AutostopMode : Mode
	{
		public AutostopMode()
		{
			//todo
		}

		public override void Activate()
		{
			throw new NotImplementedException();//todo
		}

		public override void Deactivate()
		{
			throw new NotImplementedException();//todo
		}

		protected override String UpdateName(Lang value)
		{
			return value.get("mode_autostop");//todo
		}
	} 
}
