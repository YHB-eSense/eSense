
using FormsControls.Base;
using Karl.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Karl.View
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ModesPage : ContentPage, IAnimationPage
	{
		private ModesPageVM _modesPageVM;

		public IPageAnimation PageAnimation { get; } = new SlidePageAnimation { Duration = AnimationDuration.Short, Subtype = AnimationSubtype.FromLeft };

		public ModesPage(ModesPageVM modesPageVM)
		{
			InitializeComponent();
			_modesPageVM = modesPageVM;
			BindingContext = _modesPageVM;
		}

		public void OnAnimationStarted(bool isPopAnimation)
		{
		}

		public void OnAnimationFinished(bool isPopAnimation)
		{
		}
	}
}
