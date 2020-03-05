using Foundation;
using Karl.iOS;
using Karl.Model;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(IOSNavToSettings))]
namespace Karl.iOS
{
	class IOSNavToSettings : INavToSettings
	{
		public void NavToSettings()
		{
			UIApplication.SharedApplication.OpenUrl(new NSUrl("App-Prefs:root=Bluetooth"));
		}

	}
}
