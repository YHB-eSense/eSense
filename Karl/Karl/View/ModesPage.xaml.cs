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
	public partial class ModesPage : ContentPage, IAnimationPage
	{
		private ModesPageVM _modesPageVM;

		public IPageAnimation PageAnimation { get; } = new SlidePageAnimation { Duration = AnimationDuration.Short, Subtype = AnimationSubtype.FromLeft };

		public ModesPage(ModesPageVM modesPageVM)
		{
			InitializeComponent();
			_modesPageVM = modesPageVM;
			BindingContext = _modesPageVM;
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();
			_modesPageVM.RefreshPage();
		}

		public void OnAnimationStarted(bool isPopAnimation)
		{
		}

		public void OnAnimationFinished(bool isPopAnimation)
		{
		}
	}
}
