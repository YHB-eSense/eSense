using UIKit;

namespace Karl.iOS
{
	public class Application
	{
		// This is the main entry point of the application.
		static void Main(string[] args)
		{
			global::Xamarin.Auth.Presenters.XamarinIOS.AuthenticationConfiguration.Init();
			// if you want to use a different Application Delegate class from "AppDelegate"
			// you can specify it here.
			UIApplication.Main(args, null, "AppDelegate");
		}
	}
}
