using SQLite;
using System;
using System.Diagnostics;
using TagLib;

namespace Karl.Model
{
	public class BasicAudioTrack : AudioTrack
	{
		[PrimaryKey, AutoIncrement]
		public int Id { get; set; }
		public override string StorageLocation { get; set; }
		public override double Duration { get; set; }
		public override byte[] Cover { get; set; }
		public override string Title { get; set; }
		public override string Artist { get; set; }
		public override int BPM { get; set; }
		public override string TextId { get => ""; set => _ = value; }

		private File _file = null;

		public BasicAudioTrack(string storageLocation, string title, string artist, int bpm)
		{
			StorageLocation = storageLocation;
			if (!_testing) _file = File.Create(storageLocation);
			Title = title;
			Artist = artist;
			BPM = bpm;
			Duration = GetDuration();
			Cover = GetCover();
		}

		public BasicAudioTrack() { }

		private double GetDuration()
		{
			if (!_testing && _file != null && _file.Properties.Duration != null)
			{
				return _file.Properties.Duration.TotalSeconds;
			}
			return 0;
		}

		private byte[] GetCover()
		{
			if (!_testing && _file != null && _file.Tag.Pictures.Length >= 1)
			{
				return _file.Tag.Pictures[0].Data.Data;
			}
			return null;
		}

		private static bool _testing;
		[Conditional("TESTING")]
		internal static void Testing(bool testing)
		{
			_testing = testing;
		}
	}
}
