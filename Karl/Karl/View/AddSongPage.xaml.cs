using FormsControls.Base;
using Karl.ViewModel;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Karl.View
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AddSongPage : ContentPage, IAnimationPage
	{
		private AddSongPageVM _addSongPageVM;
		public IPageAnimation PageAnimation { get; } = new FadePageAnimation { Duration = AnimationDuration.Short };

		public AddSongPage(AddSongPageVM addSongPageVM)
		{
			InitializeComponent();
			_addSongPageVM = addSongPageVM;
			BindingContext = _addSongPageVM;
		}

		public void OnAnimationStarted(bool isPopAnimation)
		{
		}

		public void OnAnimationFinished(bool isPopAnimation)
		{
		}
	}
}
