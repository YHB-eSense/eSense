using System;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;

namespace Karl.Droid
{
	[Activity(Label = "CustomUrlSchemeInterceptorActivity", NoHistory = true, LaunchMode = LaunchMode.SingleTop)]
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

			System.Diagnostics.Debug.WriteLine("CustomURLSchemeInterceptor read this URI: " + uri.OriginalString);
			Intent launchIntent = PackageManager.GetLaunchIntentForPackage("com.spotify.music");
			if (launchIntent != null)
			{
				StartActivity(launchIntent);//null pointer check in case package name was not found
			} else
			{
				throw new NotSupportedException("Spotify is not installed!");
			}

			// Load redirectUrl page
			Karl.Model.eSenseSpotifyWebAPI.WebApiSingleton.AuthenticationState.OnPageLoading(uri);

			Finish();
		}
	}
}
