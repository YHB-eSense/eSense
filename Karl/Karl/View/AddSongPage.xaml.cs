using Karl.ViewModel;
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
	public partial class AddSongPage : ContentPage
	{
		private AddSongPageVM _addSongPageVM;
		public AddSongPage(AddSongPageVM addSongPageVM)
		{
			InitializeComponent();
			_addSongPageVM = addSongPageVM;
			BindingContext = _addSongPageVM;
		}

	}
}
