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

		public void OnPlayPause(object sender, EventArgs args)
		{

		}

		public void OnSkip(object sender, EventArgs args)
		{

		}
		public void OnPlayPrev(object sender, EventArgs args)
		{

		}

		public void OnMoveInSong(object sender, EventArgs args) {

		}

		public void OnChangedVolume(object sender, EventArgs args) {

		}
	}
}
