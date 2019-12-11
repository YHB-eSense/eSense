using System;
using System.Collections.Generic;
using System.Text;

namespace Karl.Model
{
	public partial class AudioPlayer
	{
		private AudioLib audioLib;
		private IAudioPlayer audioPlayerImp;
		public int Volume { get; set; } //todo react to changes from system
		public double CurrentSecInTrack { get; set; } //todo updating it
		public bool Paused { get; set; }

		internal AudioPlayer(AudioLib audioLib)
		{
			this.audioLib = audioLib;
		}

		public void PauseTrack()
		{
			audioPlayerImp.PauseTrack();
		}
		public void PlayTrack()
		{
			audioPlayerImp.PlayTrack();
		}
		public void NextTrack()
		{
			audioPlayerImp.NextTrack();
		}
		public void PrevTrack()
		{
			audioPlayerImp.PrevTrack();
		}

		private interface IAudioPlayer
		{
			void PauseTrack();
			void PlayTrack();
			void NextTrack();
			void PrevTrack();
		}
	}

	/*internal interface IAudioPlayer
	{

	}*/
}
