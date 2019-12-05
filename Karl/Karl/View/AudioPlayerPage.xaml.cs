using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Karl.View
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AudioPlayerPage : ContentPage
	{
		public AudioPlayerPage()
		{
			InitializeComponent();
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
