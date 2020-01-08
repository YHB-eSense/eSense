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
	public partial class MainPage : ContentPage, IAnimationPage
	{
		private MainPageVM _mainPageVM;

		public IPageAnimation PageAnimation { get; } = new SlidePageAnimation { Duration = AnimationDuration.Short, Subtype = AnimationSubtype.FromRight };

		public MainPage(MainPageVM mainPageVM)
		{
			InitializeComponent();
			_mainPageVM = mainPageVM;
			BindingContext = _mainPageVM;
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();
			_mainPageVM.RefreshPage();
		}

		public void OnAnimationStarted(bool isPopAnimation)
		{
		}

		public void OnAnimationFinished(bool isPopAnimation)
		{
		}
	}
}
