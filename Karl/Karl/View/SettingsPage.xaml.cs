
using FormsControls.Base;
using Karl.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Karl.View
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SettingsPage : ContentPage, IAnimationPage
	{

		private SettingsPageVM _settingsPageVM;

		public IPageAnimation PageAnimation { get; } = new SlidePageAnimation { Duration = AnimationDuration.Short, Subtype = AnimationSubtype.FromTop };

		public SettingsPage(SettingsPageVM settingsPageVM)
		{
			InitializeComponent();
			_settingsPageVM = settingsPageVM;
			BindingContext = _settingsPageVM;
		}

		public void OnAnimationStarted(bool isPopAnimation)
		{
		}

		public void OnAnimationFinished(bool isPopAnimation)
		{
		}

	}
}
