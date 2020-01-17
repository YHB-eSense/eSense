using EarableLibrary;
using StepDetectionLibrary;

namespace StepDetectionTestApp
{
    public class MainClass
    {
		public static void Main()
		{
			string path = "C:\\Users\\Leo\\Documents\\PSE\\Daten\\schritte50hz.db";
			ActivityLog activitylog = new ActivityLog(path);
			var activityFrames = activitylog.GetData();

			Input input = new Input();
			OutputManager.SingletonOutputManager.Subscribe(new Printer());

			for (int i = 0; i < activityFrames.Length; i++)
			{
				MotionSensorSample arg = activityFrames[i].ToMotionSensorSample();
				input.ValueChanged(null, arg);
			}
		}
	}
}
