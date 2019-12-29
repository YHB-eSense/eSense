using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Karl.Model
{

	sealed class BasicAudioLib : IAudioLibImpl
	{

		public ObservableCollection<AudioTrack> AllAudioTracks { get; set; }

		public BasicAudioLib()
		{
			//testing
			AllAudioTracks = new ObservableCollection<AudioTrack>();
			AllAudioTracks.Add(new BasicAudioTrack("tnt.mp3", "TNT"));
			AllAudioTracks.Add(new BasicAudioTrack("sw.mp3", "SW"));
		}

		public void AddTrack(String Indentifier, String Name, String Artist, int BPM)
		{
			throw new NotImplementedException();
		}
	}
	
}
