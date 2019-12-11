using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Karl.Model
{
	public class AudioTrack
	{
		private IAudioTrack audioTrack;
		public string Title { get; set; }
		public string Artist { get; set; }
		public int BPM { get; set; }
		public double Duration { get; set; }
		public Image Cover { get; set; }
		public double CurrentPosition { get; set; }
		public string FileLocation { get; set; }

		public AudioTrack(string title, string artist, int bpm, string fileLocation)
		{
			Title = title;
			Artist = artist;
			BPM = bpm;
			FileLocation = fileLocation;
			//Rest aus Metadaten auslesen
		}

	}

	internal interface IAudioTrack
	{

	}
}
