using EarableLibrary;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xunit;

namespace EarableLibraryTestApp
{
	public class MainPageVM : INotifyPropertyChanged
	{
		public string StatusText { get; private set; }

		public string ButtonText => TestRunning ? "Running..." : "Run Tests";

		public ICommand StartTestCommand { get; }

		public event PropertyChangedEventHandler PropertyChanged;

		private Task TestTask = null;

		private readonly Test<IEarable>[] _tests = {
			new ConnectionTest(),
			new SensorTest<MotionSensor, MotionSensorSample>(),
			new SensorTest<PushButton, ButtonState>(),
			new SensorTest<VoltageSensor, BatteryState>(),
			new NameChangeTest()
		};

		public bool TestRunning
		{
			get => TestTask != null && !TestTask.IsCompleted;
		}

		private readonly IEarableManager _manager = new EarableLibrary.EarableLibrary();

		public MainPageVM()
		{
			StartTestCommand = new Command(StartTesting);
			StatusUpdate("Ready to run tests.");
		}

		private void StartTesting()
		{
			if (TestTask != null && !TestTask.IsCompleted) return;
			TestTask = new Task(async () => await RunTestsAsync());
			TestTask.Start();
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ButtonText)));
		}

		private async Task RunTestsAsync()
		{
			StatusUpdate("Running Tests async...");
			StatusUpdate("EarableLibrary initialized, connecting Earable...");
			var earable = await _manager.ConnectEarableAsync(); // TODO: show list instead
			Assert.NotNull(earable);
			StatusUpdate("Earable connected!");
			foreach (var test in _tests)
			{
				string result = await test.RunAndCatch(earable);
				StatusUpdate(result);
			}
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ButtonText)));
		}

		private void StatusUpdate(string status)
		{
			Debug.WriteLine(status);
			StatusText = status;
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(StatusText)));
		}
	}
}
