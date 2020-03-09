using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Karl.Model;
using Moq;
using Plugin.SimpleAudioPlayer;
using Xunit;

namespace UnitTesting.ModelTests
{
	public class BasicAudioPlayerTests
	{
		[Fact]
		public void PlayTrackTest()
		{
			//setup
			BasicAudioTrack_NEW track = new BasicAudioTrack_NEW("title", "artist", 0);
			BasicAudioPlayer_NEW player = new BasicAudioPlayer_NEW();
			//test
			player.PlayTrack(track);
			Assert.Equal(track, player.CurrentTrack);
		}

		[Fact]
		public void TogglePauseTest()
		{
			//setup
			BasicAudioTrack_NEW track = new BasicAudioTrack_NEW("title", "artist", 0);
			BasicAudioPlayer_NEW player = new BasicAudioPlayer_NEW();
			player.PlayTrack(track);
			//test
			Assert.False(player.Paused);
			player.TogglePause();
			Assert.True(player.Paused);
		}

		internal class BasicAudioTrack_NEW : BasicAudioTrack
		{
			public BasicAudioTrack_NEW(string title, string artist, int bpm)
			{
				Title = title;
				Artist = artist;
				BPM = bpm;
			}
		}

		internal class BasicAudioPlayer_NEW : BasicAudioPlayer
		{
			protected override void GetSimpleAudioPlayer()
			{
				_simpleAudioPlayer = new SimpleAudioPlayer_NEW();
			}
			protected override Stream GetStream()
			{
				return null;
			}
		}

		internal class SimpleAudioPlayer_NEW : ISimpleAudioPlayer
		{
			public event EventHandler PlaybackEnded;
			public double Duration { get; }
			public double CurrentPosition { get; private set; }
			public double Volume { get; set; }
			public double Balance { get; set; }
			public bool IsPlaying { get; private set; }
			public bool Loop { get; set; }
			public bool CanSeek { get; }
			public void Dispose() { }
			public bool Load(Stream audioStream) { return false; }
			public bool Load(string fileName) { return false; }
			public void Pause() { IsPlaying = !IsPlaying; }
			public void Play() { IsPlaying = true; }
			public void Seek(double position) { CurrentPosition = position; }
			public void Stop() { IsPlaying = false; }
			public SimpleAudioPlayer_NEW() { IsPlaying = false; }
		}

	}
}
