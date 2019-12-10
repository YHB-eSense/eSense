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
		private ConnectionPageVM ConnectionPageVM { get; }

		public ConnectionPage(ConnectionPageVM connectionPageVM)
		{
			InitializeComponent();
			ConnectionPageVM = connectionPageVM;
			BindingContext = ConnectionPageVM;
		}

	}
}
