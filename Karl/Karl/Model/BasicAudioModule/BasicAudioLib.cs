using Karl.Data;
using SpotifyAPI.Web.Models;
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
		public SimplePlaylist[] AllPlaylists { get => null; set => _ = 0; }
		public SimplePlaylist SelectedPlaylist { get => null; set => _ = 0; }

		public event AudioLibEventHandler AudioLibChanged;

		public BasicAudioLib()
		{
			_database = BasicAudioTrackDatabase.SingletonDatabase;
			AllAudioTracks = new List<AudioTrack>();
			GetTracks();
			//testing
			//AllAudioTracks = new ObservableCollection<AudioTrack>();
			//AllAudioTracks.Add(new BasicAudioTrack("tnt.mp3", "TNT", 100));
			//AllAudioTracks.Add(new BasicAudioTrack("sw.mp3", "SW", 100));
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
