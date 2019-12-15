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
	public partial class ConnectionPage : ContentPage
	{
		private ConnectionPageVM _connectionPageVM;

		public ConnectionPage(ConnectionPageVM connectionPageVM)
		{
			InitializeComponent();
			_connectionPageVM = connectionPageVM;
			BindingContext = _connectionPageVM;
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();
			_connectionPageVM.RefreshDevices();
		}

	}
}
