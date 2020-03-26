using System;

namespace Karl.Model
{
	public class SpotifyAudioTrack : AudioTrack
	{
		/// <summary>
		/// This is the Spotify Tag of this Song.
		/// </summary>
		private String Tag;
		public override double Duration { get; set; }
		public override string Title { get; set; }
		public override string Artist { get; set; }
		public override byte[] Cover { get; set; }
		public override int BPM { get; set; }
		public override string StorageLocation { get; set; }
		public override string TextId { get; set; }

		public SpotifyAudioTrack(double duration, string title, string artist,
			int bpm, string id, byte[] cover)
		{
			Duration = duration;
			Title = title;
			Artist = artist;
			BPM = bpm;
			TextId = id;
			Cover = cover;
		}

	}
}
