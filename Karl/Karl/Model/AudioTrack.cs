using System;

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
		public abstract double Duration { get; set; }
		/// <summary>
		/// The cover of the single or album.
		/// </summary>
		public abstract byte[] Cover { get; set; }
		/// <summary>
		/// The Song Title.
		/// </summary>
		public abstract string Title { get; set; }
		/// <summary>
		/// The Songs Artist.
		/// </summary>
		public abstract string Artist { get; set; }
		/// <summary>
		/// The BPM of this Song.
		/// </summary>
		public abstract int BPM { get; set; }

		public abstract String StorageLocation { get; set; }

		public abstract String TextId { get; set; }

	}
}
