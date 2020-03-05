
using FormsControls.Base;
using Karl.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Karl.View
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AudioLibPage : ContentPage, IAnimationPage
	{
		private AudioLibPageVM _audioLibPageVM;

		public IPageAnimation PageAnimation { get; } = new SlidePageAnimation { Duration = AnimationDuration.Short, Subtype = AnimationSubtype.FromRight };

		public AudioLibPage(AudioLibPageVM audioLibPageVM)
		{
			InitializeComponent();
			_audioLibPageVM = audioLibPageVM;
			BindingContext = _audioLibPageVM;
		}

		public void OnAnimationStarted(bool isPopAnimation)
		{
		}

		public void OnAnimationFinished(bool isPopAnimation)
		{
		}
	}
}
