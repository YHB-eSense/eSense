using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Karl.ViewModel;

namespace Karl.View
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AudioPlayerPage : ContentPage
	{
		private AudioPlayerPageVM AudioPlayerPageVM;

		public AudioPlayerPage(AudioPlayerPageVM audioPlayerPageVM)
		{
			InitializeComponent();
			AudioPlayerPageVM = audioPlayerPageVM;
			BindingContext = AudioPlayerPageVM;
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();
			AudioPlayerPageVM.GetAudioTrack();
			AudioPlayerPageVM.GetPausePlayBoolean();
			AudioPlayerPageVM.GetVolume();
		}

	}
}
