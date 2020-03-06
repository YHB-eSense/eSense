using Karl.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTesting
{
	class TestAudioTrack : AudioTrack
	{
		public override double Duration { get => 100; set => _ = value; }
		public override byte[] Cover { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
		public override string Title { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
		public override string Artist { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
		public override int BPM { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
		public override string StorageLocation { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
		public override string TextId { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
	}
}
