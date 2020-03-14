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

		private readonly Test<IEarable>[] _tests = {
			//new ConnectionTest(),
			new SensorTest<MotionSensor, MotionSensorSample>(),
			new SensorTest<PushButton, ButtonState>(),
			new SensorTest<VoltageSensor, BatteryState>(),
			new NameChangeTest()
		};

		private readonly Test<IEarable>[] _failingTestCombination =
		{
			new DelayTest(),
			new DelayTest.Reconnect(),
			new DelayTest(),
			new DelayTest(),
			new DelayTest(),
			new DelayTest()
		};

		public bool Running { get; private set; }

		public void StartTesting()
		{
			if (Running) return;
			Running = true;
			RunTestsAsync().ConfigureAwait(false);
		}

		private async Task RunTestsAsync()
		{
			_results.Clear();
			Status.StatusUpdate("Connecting to earable...");
			var earable = await _manager.ConnectEarableAsync(); // TODO: show list instead

			Assert.NotNull(earable);
			Status.StatusUpdate("Earable connected!");
			foreach (var test in _failingTestCombination)
			{
				var result = await test.RunAndCatch(earable);
				_results.Add(result);
				Status.StatusUpdate(result);
			}
			var status = "";
			foreach (var res in _results)
			{
				status += res.ToString() + "\n\n";
			}
			Status.StatusUpdate(status);
			Running = false;
		}
	}
}
