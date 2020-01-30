using System;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;

namespace Karl.Droid
{
	[Activity(Label = "ActivityCustomUrlSchemeInterceptor", NoHistory = true, LaunchMode = LaunchMode.SingleTop)]
	[IntentFilter(
		new[] { Intent.ActionView },
		Categories = new[] { Intent.CategoryDefault, Intent.CategoryBrowsable },
		DataSchemes = new[] { @"karl2.companyname.com" },
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
