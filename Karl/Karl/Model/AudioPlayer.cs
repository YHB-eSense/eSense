using System;
using System.Collections.Generic;
using System.Text;

namespace Karl.Model
{
	public class AudioPlayer
	{
		private AudioLib audioLib;
		private IAudioPlayer audioPlayerImplementation;

		internal AudioPlayer(AudioLib audioLib)
		{
			this.audioLib = audioLib;
		}

		internal void UseBasicAudioPlayer()
		{
			audioPlayerImplementation = BasicAudioPlayer.SingletonBasicAudioPlayer;
		}

		public void play() { }
		public void skip() { }
		public void prev() { }
		public void changeVolume() { }
		public void moveToSecInSong() { }
	}

	internal interface IAudioPlayer
	{

	}
}
