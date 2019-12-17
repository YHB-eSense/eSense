using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Karl.Model
{
	/// <summary>
	/// This is a general Audiotrack. It could have any Implementation.
	/// </summary>
	public class AudioTrack
	{
		//The actual implementation.
		private IAudioTrackImpl audioTrack;
		/// <summary>
		/// The duration of the song.
		/// </summary>
		public double Duration
		{
			get
			{
				//todo
				return 0;
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
				return null;
			}
		}
		/// <summary>
		/// The Song Title.
		/// </summary>
		public string Title
		{
			get
			{
				return ""; //todo
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
				return "";
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
				return 0; //todo
			}
			//set yes no?
		}
		/// <summary>
		/// Create a new Track.
		/// </summary>
		/// <param name="title">Tracks Name</param>
		/// <param name="artist">The Tracks artist</param>
		/// <param name="bpm">The BPM</param>
		AudioTrack(IAudioTrackImpl audioTrackImpl)
		{
			//Title = title;
			//Artist = artist;
			//BPM = bpm;
			//FileLocation = fileLocation;
			//Rest aus Metadaten auslesen
		}

	}
	interface IAudioTrackImpl
	{
		double Duration { get; }
		Image Cover { get; }
		int BPM { get; set; }
		//todo
	}
}
