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
			AllAudioTracks = new ObservableCollection<AudioTrack>();
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
			foreach(AudioTrack track in tracks) { AllAudioTracks.Add(track); }
		}

		public async void AddTrack(string storage, string title, string artist, int bpm)
		{
			BasicAudioTrack newTrack = new BasicAudioTrack(storage, title, artist, bpm);
			await _database.SaveTrackAsync(newTrack);
			AllAudioTracks.Add(newTrack);
		}

		public async void DeleteTrack(AudioTrack track)
		{	
			await _database.DeleteTrackAsync(track);
			AllAudioTracks.Remove(track);
		}

		public void Init()
		{
			//throw new NotImplementedException();
		}
	}
	
}
