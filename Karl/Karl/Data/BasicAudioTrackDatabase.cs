using Karl.Model;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Karl.Data
{
	public class BasicAudioTrackDatabase
	{
		readonly SQLiteAsyncConnection _database;
		private static BasicAudioTrackDatabase _singletonDatabase;

		public static BasicAudioTrackDatabase SingletonDatabase
		{
			get
			{
				if (_singletonDatabase == null) { _singletonDatabase = new BasicAudioTrackDatabase(
					Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
					"BasicAudioTracks.db3")); }
				return _singletonDatabase;
			}
		}

		private BasicAudioTrackDatabase(string dbPath)
		{
			_database = new SQLiteAsyncConnection(dbPath);
			//_database.DropTableAsync<BasicAudioTrack>().Wait(); //only for testing
			_database.CreateTableAsync<BasicAudioTrack>().Wait();
		}

		public Task<List<BasicAudioTrack>> GetTracksAsync()
		{
			return _database.Table<BasicAudioTrack>().ToListAsync();
		}

		public Task<BasicAudioTrack> GetTrackAsync(int id)
		{
			return _database.Table<BasicAudioTrack>().Where(i => i.Id == null).FirstOrDefaultAsync();
		}

		public Task<int> SaveTrackAsync(BasicAudioTrack track)
		{
			if (track.Id != null)
			{
				return _database.UpdateAsync(track);
			}
			else
			{
				return _database.InsertAsync(track);
			}
		}

		public Task<int> DeleteTrackAsync(AudioTrack track)
		{
			return _database.DeleteAsync(track);
		}
		
	}
}
