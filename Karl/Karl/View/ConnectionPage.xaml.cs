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
	public partial class ConnectionPage : ContentPage
	{
		public ConnectionPage()
		{
			InitializeComponent();
		}

		public void OnRefreshDevices(object sender, EventArgs args)
		{

		}
	}
}
