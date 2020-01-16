using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Karl.Droid;
using Karl.Model;
using Xamarin.Forms;

[assembly: Dependency(typeof(Karl.Droid.DroidNavToSettings))]
namespace Karl.Droid
{
	class DroidNavToSettings : INavToSettings
	{
		public void NavToSettings() {
			Forms.Context.StartActivity(new Android.Content.Intent
			(Android.Provider.Settings.ActionBluetoothSettings));
		}
		
	}
}
