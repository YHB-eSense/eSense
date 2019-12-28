using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Karl.Model
{
	sealed class SpotifyAudioTrack : AudioTrack
	{
		/// <summary>
		/// This is the Spotify Tag of this Song.
		/// </summary>
		private String Tag;
		public override TimeSpan Duration { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
		public override string Title { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
		public override string Artist { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
		public override Image Cover { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
		public override int BPM { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
		public override string StorageLocation { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
	}
}
