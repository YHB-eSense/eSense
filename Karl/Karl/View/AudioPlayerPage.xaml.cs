using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Karl.ViewModel;
using FormsControls.Base;

namespace Karl.View
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AudioPlayerPage : ContentPage, IAnimationPage
	{
		private AudioPlayerPageVM _audioPlayerPageVM;

		public IPageAnimation PageAnimation { get; } = new SlidePageAnimation { Duration = AnimationDuration.Short, Subtype = AnimationSubtype.FromBottom };

		public AudioPlayerPage(AudioPlayerPageVM audioPlayerPageVM)
		{
			InitializeComponent();
			_audioPlayerPageVM = audioPlayerPageVM;
			BindingContext = _audioPlayerPageVM;
		}

		public void OnAnimationStarted(bool isPopAnimation)
		{
		}

		public void OnAnimationFinished(bool isPopAnimation)
		{
		}
	}
}
