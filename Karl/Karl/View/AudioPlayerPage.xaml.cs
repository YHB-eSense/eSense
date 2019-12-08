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
		private AudioPlayerPageVM audioPlayerPageVM;

		public AudioPlayerPage(AudioPlayerPageVM audioPlayerPageVM)
		{
			InitializeComponent();
			this.audioPlayerPageVM = audioPlayerPageVM;
		}

		private void OnPausePlay(object sender, ToggledEventArgs e)
		{
			if(e.Value == true)
			{
				audioPlayerPageVM.Pause();
			}
			else
			{
				audioPlayerPageVM.Play();
			}
		}

		public void OnPlayPrev(object sender, EventArgs args)
		{
			audioPlayerPageVM.PlayPrevious();
		}

		public void OnPlayNext(object sender, EventArgs args)
		{
			audioPlayerPageVM.PlayNext();
		}

		public void OnMoveInSong(object sender, EventArgs args)
		{
			audioPlayerPageVM.MoveInSong(TimeSlider.Value);
		}

		public void OnChangedVolume(object sender, EventArgs args)
		{
			int newStep = (int) Math.Round(VolumeSlider.Value);
			VolumeSlider.Value = newStep;
			audioPlayerPageVM.ChangeVolume(newStep);
		}

	}
}
