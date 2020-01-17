using EarableLibrary;
using StepDetectionLibrary;

namespace TestProject
{
    public class MainClass
    {
		public static void Main()
		{
			string path = "C:\\Users\\Tims\\Code\\Workspace\\PSE\\eSense\\Karl\\StepDetectionTestApp\\Data\\schritte50hz.db";
			ActivityLog activitylog = new ActivityLog(path);
			var activityFrames = activitylog.GetData();

			Input input = new Input();

			for (int i = 0; i < activityFrames.Length; i++)
			{
				MotionSensorSample arg = activityFrames[i].ToMotionSensorSample();
				input.ValueChanged(null, arg);
			}
		}
	}
}
