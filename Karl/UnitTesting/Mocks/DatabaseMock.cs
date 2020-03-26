using Karl.Data;
using Karl.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTesting.Mocks
{
	class DatabaseMock : BasicAudioTrackDatabase
	{
		public DatabaseMock() : base()
		{
			tracksMoq.Add(new BasicAudioTrack("", "Mexico", "Alestorm", 420));
		}

		List<BasicAudioTrack> tracksMoq = new List<BasicAudioTrack>();
		public override Task<List<BasicAudioTrack>> GetTracksAsync()
		{
			return new Task<List<BasicAudioTrack>>(GetTracksMock);
		}

		private List<BasicAudioTrack> GetTracksMock()
		{
			return new List<BasicAudioTrack>(tracksMoq);
		}

		public override Task<int> SaveTrackAsync(BasicAudioTrack track)
		{
			tracksMoq.Add(track);
			return new Task<int>(() => { return tracksMoq.Count; });
		}

		public override Task<int> DeleteTrackAsync(AudioTrack track)
		{
			var x = tracksMoq.Count;
			if (!(typeof(BasicAudioTrack) == track.GetType())) throw new ArgumentException();
			BasicAudioTrack basicAudioTrack = (BasicAudioTrack)track;
			tracksMoq.Remove(basicAudioTrack);
			var y = tracksMoq.Count;
			return new Task<int>(() =>
			{
				return x - y;
			});
		}
	}
}
