using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Karl.Model
{
	/// <summary>
	/// This is a general Audiotrack. It could have any Implementation.
	/// </summary>
	public abstract class AudioTrack
	{
		/// <summary>
		/// The duration of the song.
		/// </summary>
		public abstract double Duration { get; }
		
		/// <summary>
		/// The cover of the single or album.
		/// </summary>
		public abstract Image Cover { get; }
		/// <summary>
		/// The Song Title.
		/// </summary>
		public abstract string Title { get; }
		/// <summary>
		/// The Songs Artist.
		/// </summary>
		public abstract string Artist { get; }
		/// <summary>
		/// The BPM of this Song.
		/// </summary>
		public abstract int BPM { get; }

	}
}
