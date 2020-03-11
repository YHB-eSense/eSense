using EarableLibrary;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xunit;

namespace EarableLibraryTestApp
{
	public class MainPageVM : INotifyPropertyChanged
	{
		private static MainPageVM _instance;
		private readonly IEarableManager _manager = new EarableLibrary.EarableLibrary();
		private Task _testTask = null;
		private string _statusText;

		private readonly Test<IEarable>[] _tests = {
			new ConnectionTest(),
			new SensorTest<MotionSensor, MotionSensorSample>(),
			new SensorTest<PushButton, ButtonState>(),
			new SensorTest<VoltageSensor, BatteryState>(),
			new NameChangeTest()
		};

		public static MainPageVM Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new MainPageVM();
				}
				return _instance;
			}
		}

		public string StatusText
		{
			get => _statusText;
			set
			{
				_statusText = value;
				PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(StatusText)));
			}
		}

		public string ButtonText => TestRunning ? "Running..." : "Run Tests";

		public ICommand StartTestCommand { get; }

		public event PropertyChangedEventHandler PropertyChanged;

		public bool TestRunning => _testTask != null && !_testTask.IsCompleted;

		private MainPageVM()
		{
			StartTestCommand = new Command(StartTesting);
		}

		private void StartTesting()
		{
			if (_testTask != null && !_testTask.IsCompleted) return;
			_testTask = new Task(async () => await RunTestsAsync());
			_testTask.Start();
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ButtonText)));
		}

		private async Task RunTestsAsync()
		{
			Status.StatusUpdate("Running Tests async...");
			Status.StatusUpdate("EarableLibrary initialized, connecting Earable...");
			var earable = await _manager.ConnectEarableAsync(); // TODO: show list instead
			Assert.NotNull(earable);
			Status.StatusUpdate("Earable connected!");
			foreach (var test in _tests)
			{
				var result = await test.RunAndCatch(earable);
				Status.StatusUpdate(result);
			}
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ButtonText)));
		}
	}
}
