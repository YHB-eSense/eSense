using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Karl.Model;
using Foundation;
using UIKit;
using Xamarin.Forms;
using Karl.iOS;

[assembly:Dependency(typeof(IOSNavToSettings))]
namespace Karl.iOS
{
	class IOSNavToSettings : INavToSettings
	{
		public void NavToSettings() {
			UIApplication.SharedApplication.OpenUrl(new NSUrl("App-Prefs:root=Bluetooth"));
		}

	}
}
