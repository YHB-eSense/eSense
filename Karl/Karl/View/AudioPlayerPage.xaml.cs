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

			int newStep = (int) Math.Round(VolumeSlider.Value);
			VolumeSlider.Value = newStep;
			AudioPlayerPageVM.ChangeVolume(newStep);

		private void OnPausePlay(object sender, EventArgs e)
		{
			if (AudioPlayerPageVM.PausePlayBoolean)
			{
				//update icon of PausePlayButton to Pause
			}
			else
			{
				//update icon of PausePlayButton to Play
			}
		}

		private void OnMoveInSong(object sender, ValueChangedEventArgs e)
		{
			
		}
	}
}
