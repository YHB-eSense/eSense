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
	public partial class ConnectionPage : ContentPage, IAnimationPage
	{
		private ConnectionPageVM _connectionPageVM;

		public IPageAnimation PageAnimation { get; } = new FadePageAnimation { Duration = AnimationDuration.Short};

		public ConnectionPage(ConnectionPageVM connectionPageVM)
		{
			InitializeComponent();
			_connectionPageVM = connectionPageVM;
			BindingContext = _connectionPageVM;
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();
			_connectionPageVM.RefreshPage();
		}

		public void OnAnimationStarted(bool isPopAnimation)
		{
		}

		public void OnAnimationFinished(bool isPopAnimation)
		{
		}
	}
}
