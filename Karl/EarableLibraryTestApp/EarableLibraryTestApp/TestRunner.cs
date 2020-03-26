using EarableLibrary;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace EarableLibraryTestApp
{
	public class TestRunner
	{
		private readonly List<TestResult<IEarable>> _results = new List<TestResult<IEarable>>();
		private readonly IEarableManager _manager;
		private readonly IStatus _status;

		private readonly Test<IEarable>[] _tests;

		public bool Running { get; private set; }

		public TestRunner(IEarableManager manager, IStatus status)
		{
			_manager = manager;
			_status = status;
			_tests = new Test<IEarable>[] {
				new ConnectionTest(_status),
				new SensorTest<MotionSensor, MotionSensorSample>(_status),
				new SensorTest<PushButton, ButtonState>(_status),
				new SensorTest<VoltageSensor, BatteryState>(_status),
				new NameChangeTest(_status)
			};
		}

		public void StartTesting()
		{
			if (Running) return;
			Running = true;
			RunTestsAsync().ConfigureAwait(false);
		}

		public async Task RunTestsAsync()
		{
			_results.Clear();
			_status.StatusUpdate("Connecting to earable...");
			var earable = await _manager.ConnectEarableAsync(); // TODO: show list instead

			Assert.NotNull(earable);
			_status.StatusUpdate("Earable connected!");
			foreach (var test in _tests)
			{
				var result = await test.RunAndCatch(earable);
				_results.Add(result);
				_status.StatusUpdate(result.ToString());
			}
			var status = "";
			foreach (var res in _results)
			{
				status += res.ToString() + "\n\n";
			}
			_status.StatusUpdate(status);
			Running = false;
		}
	}
}
