using System;
using System.Collections.Generic;
using System.Text;

namespace Karl.Model
{

	sealed class BasicAudioLib : IAudioLibImpl
	{

		public IList<AudioTrack> AllAudioTracks => throw new NotImplementedException();

		public void AddTrack()
		{
			throw new NotImplementedException();
		}
	}
	
}
