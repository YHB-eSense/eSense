using EarableLibrary;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace EarableLibraryTestApp
{
	public class TestRunner
	{
		private readonly List<TestResult<IEarable>> _results = new List<TestResult<IEarable>>();
		private readonly IEarableManager _manager = new EarableLibrary.EarableLibrary();
		private Task _testTask = null;

		private readonly Test<IEarable>[] _tests = {
			new ConnectionTest(),
			new SensorTest<MotionSensor, MotionSensorSample>(),
			new SensorTest<PushButton, ButtonState>(),
			new SensorTest<VoltageSensor, BatteryState>(),
			new NameChangeTest()
		};

		public bool Running => _testTask != null && !_testTask.IsCompleted;

		public void StartTesting()
		{
			if (_testTask != null && !_testTask.IsCompleted) return;
			_testTask = new Task(async () => await RunTestsAsync());
			_testTask.Start();
		}

		private async Task RunTestsAsync()
		{
			_results.Clear();
			Status.StatusUpdate("Connecting to earable...");
			var earable = await _manager.ConnectEarableAsync(); // TODO: show list instead

			Assert.NotNull(earable);
			Status.StatusUpdate("Earable connected!");
			foreach (var test in _tests)
			{
				var result = await test.RunAndCatch(earable);
				_results.Add(result);
				Status.StatusUpdate(result);
			}
			var status = "";
			foreach (var res in _results)
			{
				status += res.ToString() + "\n";
			}
			Status.StatusUpdate(status);
		}
	}
}
