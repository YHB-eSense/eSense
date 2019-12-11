using System;
using System.Collections.Generic;
using System.Text;

namespace Karl.Model
{
	public class AudioPlayer
	{
		private AudioLib audioLib;
		private IAudioPlayer audioPlayerImplementation;
		public int volume { get; set; } //todo react to changes from system
		public double currentSecInTrack { get; set; } //todo updating it
		public bool Paused { get; set; }

		internal AudioPlayer(AudioLib audioLib)
		{
			this.audioLib = audioLib;
		}

		internal void UseBasicAudioPlayer()
		{
			audioPlayerImplementation = BasicAudioPlayer.SingletonBasicAudioPlayer;
		}

		public void pauseTrack()
		{
			//todo
		}
		public void playTrack()
		{
			//todo
		}
		public void nextTrack()
		{
			//todo
		}
		public void prevTrack()
		{
			//todo
		}
	}

	internal interface IAudioPlayer
	{

	}
}
