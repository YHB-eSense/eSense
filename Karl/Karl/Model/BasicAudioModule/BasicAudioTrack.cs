using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using TagLib;
using Xamarin.Forms;

namespace Karl.Model
{
	public sealed class BasicAudioTrack : AudioTrack
	{
		[PrimaryKey, AutoIncrement]
		public override int Id { get; set; }
		public override String StorageLocation { get; set; }
		public override double Duration { get; set; }
		public override byte[] Cover { get; set; }
		public override string Title { get; set; }
		public override string Artist { get; set; }
		public override int BPM { get; set; }

		private File _file;

		public BasicAudioTrack(string storageLocation, string title, string artist, int bpm)
		{
			StorageLocation = storageLocation;
			_file = File.Create(StorageLocation);
			Title = title;
			Artist = artist;
			BPM = bpm;
			Duration = GetDuration();
			Cover = GetCover();
		}

		public BasicAudioTrack(string storageLocation, string title)
		{
			StorageLocation = storageLocation;
			_file = File.Create(StorageLocation);
			Title = title;
			Artist = GetArtist();
			BPM = GetBPM();
			Duration = GetDuration();
			Cover = GetCover();
		}

		public BasicAudioTrack(string storageLocation)
		{
			StorageLocation = storageLocation;
			_file = File.Create(StorageLocation);
			Title = GetTitle();
			Artist = GetArtist();
			BPM = GetBPM();
			Duration = GetDuration();
			Cover = GetCover();
		}

		public BasicAudioTrack() {}
		
		private double GetDuration()
		{
			if (_file != null && _file.Properties.Duration != null)
			{
				return _file.Properties.Duration.TotalSeconds;
			}
			return 0;
		}

		private byte[] GetCover()
		{
			if (_file != null && _file.Tag.Pictures.Length >= 1)
			{
				return (byte[]) (_file.Tag.Pictures[0].Data.Data);
			}
			return null;	
		}

		private string GetTitle()
		{
			if (_file != null && _file.Tag.Title != null)
			{
				return _file.Tag.Title;
			}
			return "Unknown Title";
		}

		private string GetArtist()
		{
			if (_file != null && _file.Tag.AlbumArtists.Length >= 1)
			{
				return _file.Tag.AlbumArtists[0];
			}
			return "Unknown Artist";	
		}

		private int GetBPM()
		{
	
			if (_file != null && _file.Tag.BeatsPerMinute != 0)
			{
				return (int) _file.Tag.BeatsPerMinute;
			}
			return 0;
		}

	}
}
