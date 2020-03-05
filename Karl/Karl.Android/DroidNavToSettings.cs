using Karl.Model;
using Xamarin.Forms;

[assembly: Dependency(typeof(Karl.Droid.DroidNavToSettings))]
namespace Karl.Droid
{
	class DroidNavToSettings : INavToSettings
	{
		public void NavToSettings()
		{
			Forms.Context.StartActivity(new Android.Content.Intent
			(Android.Provider.Settings.ActionBluetoothSettings));
		}

	}
}
