using SpotifyAPI.Web;
using SpotifyAPI.Web.Models;
using System;
using System.Net.Http;
using Xamarin.Auth;
using Xamarin.Auth.Presenters;
using Xamarin.Forms;

namespace Karl.Model
{

	/// <summary>
	/// Class to get Connection to Spotify Web API
	/// </summary>
	public class eSenseSpotifyWebAPI
	{
		private static eSenseSpotifyWebAPI _instance;
		public static eSenseSpotifyWebAPI WebApiSingleton
		{
			get
			{
				if (_instance == null)
				{
					_instance = new eSenseSpotifyWebAPI();
				}
				return _instance;
			}
		}

		private eSenseSpotifyWebAPI() { }

		/// <summary>
		/// Spotify realated variables
		/// </summary>
		private const string CLIENT_ID = "cf74e3a8655c4a03b405d2d52c9193cf";
		private const string CLIENT_SECRET = "a9b3b53610484638a35a91da896ccae0";
		private const string SCOPE = "user-read-playback-state user-modify-playback-state";
		private const string AUTHORIZE_URI = @"https://accounts.spotify.com/authorize";
		private const string REDIRECT_URI = @"karl2.companyname.com:/oauth2redirect";
		private const string ACCESSTOKEN_URI = @"https://accounts.spotify.com/api/token";

		/// <summary>
		/// Variables for OAuth2 Authentification
		/// </summary>
		private Account _acc;
		private HttpClient _client;
		private OAuth2Authenticator auth;

		public SpotifyWebAPI api { get; set; }
		public PrivateProfile UsersProfile { get; set; }
		public event EventHandler authentificationFinished;
		public OAuth2Authenticator AuthenticationState { get; private set; }


		/// <summary>
		/// Starts Spotify Authentification Process
		/// </summary>
		public void Auth()
		{
			Uri AuthURI = new Uri(AUTHORIZE_URI);
			Uri RedirectURI = new Uri(REDIRECT_URI);
			Uri AccessTokenUri = new Uri(ACCESSTOKEN_URI);
			auth = new OAuth2Authenticator(
				clientId: CLIENT_ID,
				clientSecret: CLIENT_SECRET,
				scope: SCOPE,
				authorizeUrl: AuthURI,
				redirectUrl: RedirectURI,
				accessTokenUrl: AccessTokenUri);
			auth.Completed += OnAuthAsync;
			OAuthLoginPresenter presenter = new OAuthLoginPresenter();
			presenter.Login(auth);

		}


		/// <summary>
		/// Gets called when Auth completed
		/// </summary>
		/// <param name="sender">Events sender</param>
		/// <param name="args">Events arguments</param>
		private async void OnAuthAsync(object sender, AuthenticatorCompletedEventArgs args)
		{
			//If authentification failed
			if (!args.IsAuthenticated)
			{
				Application.Current.MainPage.DisplayAlert("", "Spotify auth failed", "OK");
				return;
			}

			//Use AccessToken to authentify for connecting to Spotify
			_acc = args.Account;
			_client = new HttpClient();
			string[] serial = _acc.Serialize().Split('&');
			string accessToken = serial[1].Split('=')[1];
			_client.DefaultRequestHeaders.Authorization = new
				System.Net.Http.Headers.AuthenticationHeaderValue(accessToken);

			//Init Spotify Objects
			api = new SpotifyWebAPI
			{
				AccessToken = accessToken,
				TokenType = "Bearer"
			};
			PrivateProfile profile = await api.GetPrivateProfileAsync();
			AvailabeDevices devices = await api.GetDevicesAsync();

			//If Authentification worked properly
			if (!profile.HasError())
			{
				UsersProfile = profile;
				authentificationFinished.Invoke(sender, args);
			}

		}


	}
}
