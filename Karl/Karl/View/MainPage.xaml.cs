using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Karl.ViewModel;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;

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

		protected async override void OnAppearing()
		{
			base.OnAppearing();
			_mainPageVM.RefreshPage();
			/*
			var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Location);
			if (status != PermissionStatus.Granted)
			{
				if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Location))
				{
					await DisplayAlert("Need location", "Gunna need that location", "OK");
				}

				var results = await CrossPermissions.Current.RequestPermissionsAsync(new[] { Permission.Location });
				status = results[Permission.Location];
			*/

		}

	}
}
