using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Karl.Model
{
	sealed class BasicAudioTrack : AudioTrack
	{
		public override double Duration => throw new NotImplementedException(); //todo

		public override Image Cover => throw new NotImplementedException(); //todo
		public override string Title => throw new NotImplementedException();
		public override string Artist => throw new NotImplementedException();
		public override int BPM { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
	}
	
}
