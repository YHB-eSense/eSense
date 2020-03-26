using Karl.Model;
using SQLite;
using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using static System.Environment;

namespace Karl.Data
{
	public class BasicAudioTrackDatabase
	{
		private static bool _testing;
		private SQLiteAsyncConnection _database { get; set; }
		private static BasicAudioTrackDatabase _singletonDatabase;

		public static BasicAudioTrackDatabase SingletonDatabase
		{
			get
			{
				if (_singletonDatabase == null)
				{
					var path = Path.Combine(GetFolderPath(SpecialFolder.LocalApplicationData), "BasicAudioTracks.db3");
					_singletonDatabase = new BasicAudioTrackDatabase(path);
				}
				return _singletonDatabase;
			}
		}

		protected BasicAudioTrackDatabase()
		{
			if (!_testing) throw new MethodAccessException("This Contructor is for creating mocks only.");
		}

		private BasicAudioTrackDatabase(string dbPath)
		{
			_database = new SQLiteAsyncConnection(dbPath);
			//_database.DropTableAsync<BasicAudioTrack>().Wait(); //only for testing
			_database.CreateTableAsync<BasicAudioTrack>().Wait();
		}

		public virtual Task<List<BasicAudioTrack>> GetTracksAsync()
		{
			return _database.Table<BasicAudioTrack>().ToListAsync();
		}

		public virtual Task<int> SaveTrackAsync(BasicAudioTrack track)
		{
			/*
			if (track.Id != null)
			{
				return _database.UpdateAsync(track);
			}
			else
			{
				
			}
			*/
			return _database.InsertAsync(track);
		}

		public virtual Task<int> DeleteTrackAsync(AudioTrack track)
		{
			return _database.DeleteAsync(track);
		}

		[Conditional("TESTING")]
		internal static void Testing(bool testing)
		{
			_testing = testing;
		}
	}
}
