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
	public partial class MainPage : ContentPage
	{
		private MainPageVM MainPageVM;

		public MainPage(MainPageVM mainPageVM)
		{
			InitializeComponent();
			MainPageVM = mainPageVM;
			BindingContext = MainPageVM;
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();
			MainPageVM.GetDeviceName();
			MainPageVM.GetStepsAmount();
		}
		
	}
}
