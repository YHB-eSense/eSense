using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Karl.Model
{
	sealed class BasicAudioTrack : AudioTrack
	{
		public override String StorageLocation { get; set; }
		public override TimeSpan Duration { get; set; }
		public override Image Cover { get; set; }
		public override string Title { get; set; }
		public override string Artist { get; set; }
		public override int BPM { get; set; }

		//private File _file;

		public BasicAudioTrack(string storageLocation, string title)
		{
			StorageLocation = storageLocation;
			Title = title;
		}

		public BasicAudioTrack(string storageLocation)
		{
			StorageLocation = storageLocation;
			//_file = File.Create(StorageLocation);
			//Duration = GetDuration();
			//Cover = GetCover();
			//Title = GetTitle();
			//Artist = GetArtist();
			//BPM = GetBPM();
		}
		/*
		private TimeSpan GetDuration()
		{
			if (_file.Properties.Duration != null)
			{
				return _file.Properties.Duration;
			}
			return new TimeSpan(0, 0, 0);
		}

		private Image GetCover()
		{
			if (_file.Tag.Pictures.Length >= 1)
			{
				Image cover = new Image();
				var bin = (byte[])(_file.Tag.Pictures[0].Data.Data);
				cover.Source = ImageSource.FromStream(() => new System.IO.MemoryStream(bin));
				return cover;
			}
			return null;
		}

		private string GetTitle()
		{
			if (_file.Tag.Title != null)
			{
				return _file.Tag.Title;
			}
			return null;
		}

		private string GetArtist()
		{
			if (_file.Tag.AlbumArtists.Length >= 1)
			{
				return _file.Tag.AlbumArtists[0];
			}
			return null;
		}

		private int GetBPM()
		{
			if (_file.Tag.BeatsPerMinute != 0)
			{
				return (int) _file.Tag.BeatsPerMinute;
			}
			return 0;
		}
		*/
	}
}
