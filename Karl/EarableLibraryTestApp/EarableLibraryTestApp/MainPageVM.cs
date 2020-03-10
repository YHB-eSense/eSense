using EarableLibrary;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace EarableLibraryTestApp
{
	public class MainPageVM : INotifyPropertyChanged
	{
		public string StatusText { get; private set; }

		public string ButtonText => TestRunning ? "Running..." : "Run Tests";

		public ICommand StartTestCommand { get; }

		public event PropertyChangedEventHandler PropertyChanged;

		private Task TestTask = null;

		public bool TestRunning
		{
			get => TestTask != null && !TestTask.IsCompleted;
		}

		private IEarableManager manager = new EarableLibrary.EarableLibrary();

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
			var earable = await manager.ConnectEarableAsync();
			StatusUpdate("Earable connected!");
			string result;
			result = await new ConnectionTest().RunAndCatch(earable);
			StatusUpdate(result);
			/*var result = await new NameChangeTest(mgr).Run();
			StatusUpdate(result);
			var result = new SensorTest(mgr, typeof(MotionSensor)).Run();
			StatusUpdate(result);
			var result = new SensorTest(mgr, typeof(PushButton)).Run();
			StatusUpdate(result);
			var result = new SensorTest(mgr, typeof(VoltageSensor)).Run();
			StatusUpdate(result);*/
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
