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
		private AudioLibPageVM AudioLibPageVM;

		public AudioLibPage(AudioLibPageVM audioLibPageVM)
		{
			InitializeComponent();
			AudioLibPageVM = audioLibPageVM;
		}
	}
}
