using FormsControls.Base;
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
	public partial class AddSongPage : ContentPage, IAnimationPage
	{
		private AddSongPageVM _addSongPageVM;
		public IPageAnimation PageAnimation { get; } = new FadePageAnimation { Duration = AnimationDuration.Short };

		public AddSongPage(AddSongPageVM addSongPageVM)
		{
			InitializeComponent();
			_addSongPageVM = addSongPageVM;
			BindingContext = _addSongPageVM;
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();
			_addSongPageVM.RefreshPage();
		}

		public void OnAnimationStarted(bool isPopAnimation)
		{
		}

		public void OnAnimationFinished(bool isPopAnimation)
		{
		}
	}
}
