using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SpotifyAPI.Web.Auth;
using Karl.Model;
using Xamarin.Auth;

namespace Karl.Droid
{
	[Activity(Label = "ActivityCustomUrlSchemeInterceptor", NoHistory = true, LaunchMode = LaunchMode.SingleTop)]
	[IntentFilter(
		new[] { Intent.ActionView },
		Categories = new[] { Intent.CategoryDefault, Intent.CategoryBrowsable },
		DataSchemes = new[] { @"karl1.companyname.com" },
		DataPath = "/oauth2redirect")]
	public class CustomURLSchemeInterceptorActivity : Activity
	{
		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			// Convert Android.Net.Url to Uri
			var uri = new Uri(Intent.Data.ToString());

			System.Diagnostics.Debug.WriteLine(uri.OriginalString);

			// Load redirectUrl page
			Karl.Model.eSenseSpotifyWebAPI.WebApiSingleton.AuthenticationState.OnPageLoading(uri);

			Finish();
		}
	}
}
