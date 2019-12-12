using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Karl.Model
{
	/// <summary>
	/// This is a general Audiotrack. It could have any Implementation.
	/// </summary>
	public partial class AudioTrack
	{
		//The actual implementation.
		private IAudioTrack audioTrack;
		/// <summary>
		/// The duration of the song.
		/// </summary>
		public double Duration
		{
			get
			{
				//todo
				return audioTrack.Duration;
			}
		}
		/// <summary>
		/// The cover of the single or album.
		/// </summary>
		public Image Cover
		{
			get
			{
				//todo
				return audioTrack.Cover;
			}
		}
		/// <summary>
		/// The current position in Song.
		/// </summary>
		public double CurrentPosition
		{
			get
			{
				//todo
				return audioTrack.CurrentPosition;
			}
		}
		/// <summary>
		/// The Song Title.
		/// </summary>
		public string Title
		{
			get
			{
				return audioTrack.Title; //todo
			}
			//set yes no?
		}
		/// <summary>
		/// The Songs Artist.
		/// </summary>
		public string Artist
		{
			get
			{
				return audioTrack.Artist;
			}
			//set yes no?
		}
		/// <summary>
		/// The BPM of this Song.
		/// </summary>
		public int BPM
		{
			get
			{
				return audioTrack.BPM; //todo
			}
			//set yes no?
		}
		/// <summary>
		/// Create a new Track.
		/// </summary>
		/// <param name="title">Tracks Name</param>
		/// <param name="artist">The Tracks artist</param>
		/// <param name="bpm">The BPM</param>
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
			string Title { get; }
			string Artist { get; }
			int BPM { get; }
			//todo
		}

	}
}
