using EarableLibrary;
using StepDetectionLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject
{
    class MainClass
    {
		public static async void Main(string[] args)
		{
			ActivityLog activitylog = new ActivityLog();
			ActivityFrame[] activityFrames = await activitylog.GetData();
			Input input = new Input();

			for (int i = 0; i < activityFrames.Length; i++)
			{
				MotionArgs arg = activityFrames[i].ToMotionArgs();
				input.ValueChanged(null, arg);
			}


		}

	}
}
