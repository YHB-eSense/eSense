using System;
using System.Collections.Generic;
using System.Text;

namespace Karl.Model
{

	sealed class BasicAudioLib : IAudioLibImpl
	{

		public IList<AudioTrack> AllAudioTracks { get; }

		public BasicAudioLib()
		{
			//testing
			AllAudioTracks = new List<AudioTrack>();
			AllAudioTracks.Add(new BasicAudioTrack("tnt.mp3", "TNT"));
		}

		public void AddTrack(String Indentifier, String Name, String Artist, int BPM)
		{
			throw new NotImplementedException();
		}
	}
	
}
