using Karl.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Karl.Model
{

	sealed class BasicAudioLib : IAudioLibImpl
	{
		private BasicAudioTrackDatabase _database;

		public ObservableCollection<AudioTrack> AllAudioTracks { get; set; }

		public BasicAudioLib()
		{
			_database = BasicAudioTrackDatabase.SingletonDatabase;
			GetTracks();
			//testing
			//AllAudioTracks = new ObservableCollection<AudioTrack>();
			//AllAudioTracks.Add(new BasicAudioTrack("tnt.mp3", "TNT", 100));
			//AllAudioTracks.Add(new BasicAudioTrack("sw.mp3", "SW", 100));
		}

		public async void AddTrack(String storage)
		{
			await _database.SaveTrackAsync(new BasicAudioTrack(storage));
			GetTracks();
		}

		public async void AddTrack(string storage, string title)
		{
			await _database.SaveTrackAsync(new BasicAudioTrack(storage, title));
			GetTracks();
		}

		public async void AddTrack(string storage, string title, string artist, int bpm)
		{
			await _database.SaveTrackAsync(new BasicAudioTrack(storage, title, artist, bpm));
			GetTracks();
		}

		private async void GetTracks()
		{
			var data = await _database.GetTracksAsync();
			AllAudioTracks = new ObservableCollection<AudioTrack>(data);
		}
	}
	
}
