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
	public partial class AudioLibPage : ContentPage
	{
		private AudioLibPageVM _audioLibPageVM;

		public AudioLibPage(AudioLibPageVM audioLibPageVM)
		{
			InitializeComponent();
			_audioLibPageVM = audioLibPageVM;
			BindingContext = _audioLibPageVM;
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();
			_audioLibPageVM.GetSongs();
		}

	}
}
