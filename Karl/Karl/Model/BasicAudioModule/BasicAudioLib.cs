using Karl.Data;
using SpotifyAPI.Web.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using static Karl.Model.AudioLib;

namespace Karl.Model
{

	sealed class BasicAudioLib : IAudioLibImpl
	{
		private BasicAudioTrackDatabase _database;

		public List<AudioTrack> AllAudioTracks { get; set; }

		/// <summary>
		/// Non functional. 
		/// </summary>
		public SimplePlaylist[] AllPlaylists
		{
			get => throw new NotImplementedException("Multiple Playlists not supported on a Basic Lib");
			set => throw new NotImplementedException("Multiple Playlists not supported on a Basic Lib");
		}
		/// <summary>
		/// Non functional. 
		/// </summary>
		public SimplePlaylist SelectedPlaylist
		{
			get => throw new NotImplementedException("Multiple Playlists not supported on a Basic Lib");
			set => throw new NotImplementedException("Multiple Playlists not supported on a Basic Lib");
		}

		public event AudioLibEventHandler AudioLibChanged;

		public BasicAudioLib()
		{
			_database = BasicAudioTrackDatabase.SingletonDatabase;
			AllAudioTracks = new List<AudioTrack>();
			GetTracks();
		}

		private async void GetTracks()
		{
			var data = await _database.GetTracksAsync();
			ObservableCollection<AudioTrack> tracks = new ObservableCollection<AudioTrack>(data);
			foreach (AudioTrack track in tracks) { AllAudioTracks.Add(track); }
			AudioLibChanged?.Invoke(this, null);
		}

		public async Task AddTrack(string storage, string title, string artist, int bpm)
		{
			BasicAudioTrack newTrack = new BasicAudioTrack(storage, title, artist, bpm);
			await _database.SaveTrackAsync(newTrack);
			AllAudioTracks.Add(newTrack);
			AudioLibChanged?.Invoke(this, null);
		}

		public async void DeleteTrack(AudioTrack track)
		{
			await _database.DeleteTrackAsync(track);
			AllAudioTracks.Remove(track);
			AudioLibChanged?.Invoke(this, null);
		}

		public void Init()
		{
			//throw new NotImplementedException();
		}
	}

}
