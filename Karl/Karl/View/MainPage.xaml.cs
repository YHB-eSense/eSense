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
	public partial class MainPage : ContentPage
	{
		private MainPageVM _mainPageVM;

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

	}
}
