using Karl.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Karl.View
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MainPage : ContentPage
	{
		private MainPageVM _mainPageVM;

		public MainPage(MainPageVM mainPageVM)
		{
			InitializeComponent();
			_mainPageVM = mainPageVM;
			BindingContext = _mainPageVM;
		}

	}
}
