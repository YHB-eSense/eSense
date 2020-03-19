using EarableLibrary;
using StepDetectionLibrary;
using System;
using System.IO;

namespace StepDetectionTestApp
{
	public class MainClass
	{
		public static void Main()
		{
			var inputFiles = Directory.EnumerateFiles("Data", "*.db");

			Input input = new Input();
			OutputManager output = OutputManager.SingletonOutputManager;

			foreach (var file in inputFiles)
			{
				ProcessFile(file, input, output);
			}
		}

		private static void ProcessFile(string file, Input input, OutputManager output)
		{
			output.Log.Reset();
			ActivityLog activitylog = new ActivityLog(file);
			var activityFrames = activitylog.GetData();

			for (int i = 0; i < activityFrames.Length; i++)
			{
				MotionSensorSample arg = activityFrames[i].ToMotionSensorSample();
				input.ValueChanged(null, arg);
			}

			var stepCount = output.Log.CountSteps();

			Console.WriteLine("File '{0}': {1} total steps", file, stepCount);
		}
	}
}
