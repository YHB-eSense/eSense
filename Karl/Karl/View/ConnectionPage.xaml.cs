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
		private ConnectionPageVM connectionPageVM;

		public ConnectionPage(ConnectionPageVM connectionPageVM)
		{
			InitializeComponent();
			this.connectionPageVM = connectionPageVM;
			this.BindingContext = this.connectionPageVM;
		}

		public ConnectionPageVM ConnectionPageVM
		{
			get => default;
			set
			{
			}
		}

		public void OnRefreshDevices(object sender, EventArgs args)
		{
			connectionPageVM.RefreshDevices();
		}
	}
}
