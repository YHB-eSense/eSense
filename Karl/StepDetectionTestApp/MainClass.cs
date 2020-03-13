using EarableLibrary;
using StepDetectionLibrary;
using System;
using System.Diagnostics;
using System.IO;

namespace StepDetectionTestApp
{
	public class MainClass
	{
		public static void Main()
		{
			string path = "C:\\Users\\Leo\\Documents\\PSE\\Daten\\16schritte50hz.db";
			ActivityLog activitylog = new ActivityLog(path);
			var activityFrames = activitylog.GetData();

			Input input = new Input();
			OutputManager.SingletonOutputManager.Subscribe(new Printer());
			OutputManager.SingletonOutputManager.Log.Reset();

			for (int i = 0; i < activityFrames.Length; i++)
			{
				MotionSensorSample arg = activityFrames[i].ToMotionSensorSample();
				input.ValueChanged(null, arg);
			}
			Debug.WriteLine(Path.Combine(Environment.CurrentDirectory, @"Data\", "6schritte50hz.db"));
		}
	}
}
