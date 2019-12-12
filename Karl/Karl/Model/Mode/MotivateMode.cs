using System;
using System.Collections.Generic;
using System.Text;

namespace Karl.Model
{
	class MotivateMode : Mode
	{

		public override void Activate()
		{
			throw new NotImplementedException(); //todo
		}

		public override void Deactivate()
		{
			throw new NotImplementedException(); //todo
		}

		protected override String UpdateName(Lang value)
		{
			return value.MOTIVATE_MODE;//todo
		}
	}
}
