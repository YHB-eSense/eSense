using EarableLibrary;
using StepDetectionLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject
{
    public class MainClass
    {
		public static void Main()
		{
			ActivityLog activitylog = new ActivityLog();
			var activityFrames = Blub().GetAwaiter().GetResult();

			Input input = new Input();

			for (int i = 0; i < activityFrames.Length; i++)
			{
				MotionSensorSample arg = activityFrames[i].ToMotionSensorSample();
				input.ValueChanged(null, arg);
			}
		}

		private static async Task<ActivityFrame[]> Blub()
		{
			ActivityLog activitylog = new ActivityLog();
			return await activitylog.GetData();
		}

	}
}
