using SpotifyAPI.Web;
using SpotifyAPI.Web.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using Xamarin.Auth;
using Xamarin.Auth.Presenters;

namespace Karl.Model
{
	public class eSenseSpotifyWebAPI
	{
		private static eSenseSpotifyWebAPI _instance;
		public SpotifyWebAPI api { get; set; }
		public PrivateProfile UsersProfile { get; set; }
		public event EventHandler isauthed;

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
		public OAuth2Authenticator AuthenticationState { get; private set; }

		private const string CLIENT_ID = "cf74e3a8655c4a03b405d2d52c9193cf";
		private const string CLIENT_SECRET = "a9b3b53610484638a35a91da896ccae0";
		private const string SCOPE = "user-read-playback-state user-modify-playback-state";
		private const string AUTHORIZE_URI = @"https://accounts.spotify.com/authorize";
		private const string REDIRECT_URI = @"karl1.companyname.com:/oauth2redirect";
		private const string ACCESSTOKEN_URI = @"https://accounts.spotify.com/api/token";
		private Account _acc;
		private HttpClient _client;
		private OAuth2Authenticator auth;

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
			
			_client = new HttpClient();
			
		}

		public async void OnAuthAsync(object sender, AuthenticatorCompletedEventArgs args)
		{
			if (!args.IsAuthenticated) throw new HttpRequestException("Authentication failed!");
			_acc = args.Account;
			string[] serial = _acc.Serialize().Split('&');
			string accessToken = serial[1].Split('=')[1];
			_client.DefaultRequestHeaders.Authorization = new
				System.Net.Http.Headers.AuthenticationHeaderValue(accessToken);
			api = new SpotifyWebAPI
			{
				AccessToken = accessToken,
				TokenType = "Bearer"
			};
			PrivateProfile profile = await api.GetPrivateProfileAsync();
			if (!profile.HasError())
			{
				UsersProfile = profile;
				isauthed.Invoke(sender, args);
			}
		
		}


	}
}
