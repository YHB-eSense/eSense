using StepDetectionLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace UnitTesting.StepDetectionLibraryTests
{
	/// <summary>
	/// Test Class for Activitylog.cs
	/// </summary>
	public class ActivityLogTest
	{
		/// <summary>
		/// tests adding and reseting steps
		/// </summary>
		[Fact]
		public void AddandResetTest()
		{
			var path = @"test.db";
			ActivityLog TestLog = new ActivityLog(path);
			TestLog.Reset();
			Step TestStep = new Step
			{
				Taken = DateTime.UtcNow,
				Intensity = 6742,
				Duration = new TimeSpan(0,0,1)
			};
			TestLog.Add(TestStep);
			Assert.Equal(1, TestLog.CountSteps());
			TestLog.Reset();
			Assert.Equal(0, TestLog.CountSteps());
		}

		/// <summary>
		/// tests method laststeps
		/// </summary>
		[Fact]
		public void LastStepsTest()
		{
			var path = @"test.db";
			ActivityLog TestLog = new ActivityLog(path);
			TestLog.Reset();
			Step TestStep = new Step
			{
				Taken = DateTime.UtcNow,
				Intensity = 6742,
				Duration = new TimeSpan(0, 0, 1)
			};
			TestLog.Add(TestStep);
			Step ReturnValue = TestLog.LastSteps(1).First();
			Assert.Equal(TestStep.Taken, ReturnValue.Taken);
			Assert.Equal(TestStep.Intensity, ReturnValue.Intensity);
			Assert.Equal(TestStep.Duration, ReturnValue.Duration);
		}

		/// <summary>
		/// tests countsteps with timespan and averagefrequency
		/// </summary>
		[Fact]
		public void CountandFrequencyTest()
		{
			var path = @"test.db";
			ActivityLog TestLog = new ActivityLog(path);
			TestLog.Reset();
			for (int i = 0; i <= 11; i++) {
				TestLog.Add(new Step
				{
					Taken = DateTime.UtcNow,
					Intensity = 6742 + i,
					Duration = new TimeSpan(0, 0, 1)
				});
			}
			Assert.Equal(12,TestLog.CountSteps(new TimeSpan(0,0,4)));
			Assert.Equal(3, TestLog.AverageStepFrequency(new TimeSpan(0, 0, 4)));
		}
	}
}
