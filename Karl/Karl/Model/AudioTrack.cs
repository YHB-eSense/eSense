using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Karl.Model
{
	public partial class AudioTrack
	{
		private IAudioTrack audioTrack;
		public double Duration
		{
			get
			{
				//todo
				return 0;
			}
		}
		public Image Cover
		{
			get
			{
				//todo
				return null;
			}
		}
		public double CurrentPosition
		{
			get
			{
				//todo
				return 0;
			}
		}
		public string Title { get; set; }
		public string Artist { get; set; }
		public int BPM { get; set; }

		public AudioTrack(string title, string artist, int bpm, string fileLocation)
		{
			Title = title;
			Artist = artist;
			BPM = bpm;
			//FileLocation = fileLocation;
			//Rest aus Metadaten auslesen
		}

	}

	internal interface IAudioTrack
	{

		private interface IAudioTrack
		{
			double Duration { get; }
			Image Cover { get; }
			double CurrentPosition { get; }
			//todo
		}
	}
}
