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
		public override double Duration => throw new NotImplementedException();//todo
		public override string Title => throw new NotImplementedException();

		public override string Artist => throw new NotImplementedException();
		public override Image Cover => throw new NotImplementedException();//todo
		public override int BPM { get => throw new NotImplementedException(); }
	}
}
