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
		public string Title
		{
			get
			{
				return ""; //todo
			}
			//set yes no?
		}
		public string Artist
		{
			get
			{
				return "";
			}
			//set yes no?
		}
		public int BPM
		{
			get
			{
				return 0; //todo
			}
			//set yes no?
		}

		public AudioTrack(string title, string artist, int bpm)
		{
			//Title = title;
			//Artist = artist;
			//BPM = bpm;
			//FileLocation = fileLocation;
			//Rest aus Metadaten auslesen
		}
		private interface IAudioTrack
		{
			double Duration { get; }
			Image Cover { get; }
			double CurrentPosition { get; }
			//todo
		}

	}
}
