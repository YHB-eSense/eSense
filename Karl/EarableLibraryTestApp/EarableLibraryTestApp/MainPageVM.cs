using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace EarableLibraryTestApp
{
	public class MainPageVM : INotifyPropertyChanged
	{
		private static MainPageVM _instance;
		private readonly TestRunner _testRunner;
		private string _statusText;

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

		// TODO: PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(ButtonText))) in adequate place
		public string ButtonText => _testRunner.Running ? "Running..." : "Run Tests";

		public ICommand StartTestCommand { get; }

		public event PropertyChangedEventHandler PropertyChanged;

		private MainPageVM()
		{
			_testRunner = new TestRunner();
			StartTestCommand = new Command(_testRunner.StartTesting);
		}
	}
}
