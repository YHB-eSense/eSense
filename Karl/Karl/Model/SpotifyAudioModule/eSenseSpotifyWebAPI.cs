using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using SpotifyAPI.Web.Auth;
using SpotifyAPI.Web;
using SpotifyAPI.Web.Enums;
using SpotifyAPI.Web.Models;

namespace Karl.Model
{
	class eSenseSpotifyWebAPI
	{
		private const string CLIENT_ID = "cf74e3a8655c4a03b405d2d52c9193cf";
		private const string CLIENT_SECRET = "a9b3b53610484638a35a91da896ccae0";
		private string TokenType;
		private string AccessToken;
		private SpotifyWebAPI _api;
		private PrivateProfile profile;
		public eSenseSpotifyWebAPI()
		{
			Authorize();
		}
		private async void Authorize()
		{
			AuthorizationCodeAuth auth = new AuthorizationCodeAuth(
				CLIENT_ID, CLIENT_SECRET, @"http://google.com", @"http://google.com",
				Scope.PlaylistReadPrivate | Scope.PlaylistReadCollaborative | Scope.AppRemoteControl);

			auth.AuthReceived += async (sender, payload) =>
			{
				auth.Stop();
				Token token = await auth.ExchangeCode(payload.Code);
				_api = new SpotifyWebAPI()
				{
					TokenType = token.TokenType,
					AccessToken = token.AccessToken
				};
				// Do requests with API client
			};
			auth.Start(); // Starts an internal HTTP Server
			auth.OpenBrowser();
		}

		private async void FetchProfile()
		{
			profile = await _api.GetPrivateProfileAsync();
		}

		public Paging<SimplePlaylist> FetchPlaylists()
		{
			return _api.GetUserPlaylists(profile.Id);
		}

		public Paging<PlaylistTrack> FetchPlaylistTracks(SimplePlaylist playlist)
		{
			return _api.GetPlaylistTracks(playlist.Id);
		}

		public Paging<PlaylistTrack> FetchPlaylistTracks(string playlistID)
		{
			return _api.GetPlaylistTracks(playlistID);
		}
	}
}
