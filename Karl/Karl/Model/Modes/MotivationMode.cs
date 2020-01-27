using System;
using StepDetectionLibrary;
using static Karl.Model.AudioPlayer;
using static Karl.Model.AudioLib;
using static Karl.Model.LangManager;

namespace Karl.Model
{
	/// <summary>
	/// The Motivation Mode.
	/// </summary>
	public class MotivationMode : IMode, IObserver<Output>
	{
		/// <summary>
		/// If current step rate and current song BPM difer by more than this threshold, a new song will prematurely be chosen.
		/// </summary>
		public double MaxAllowedBPMDiff;

		private Output? LastOutput;

		public string Name
		{
			get => SingletonLangManager.CurrentLang.Get("motivation_mode");
		}

		public void Activate()
		{
			SingletonAudioPlayer.Clear();
			SingletonAudioPlayer.NextSongEvent += ChooseNextSong;
			if (SingletonAudioPlayer.Paused)
			{
				ChooseNextSong();
				SingletonAudioPlayer.NextTrack();
			}
		}

		public void Deactivate()
		{
			SingletonAudioPlayer.NextSongEvent -= ChooseNextSong;
		}

		public void OnNext(Output value)
		{
			LastOutput = value;
			if (SingletonAudioPlayer.SongsQueue.Count == 0)
			{
				ChooseNextSong();
			}
			else if (Math.Abs(SingletonAudioPlayer.SongsQueue.Peek().BPM - value.Frequency) > MaxAllowedBPMDiff)
			{
				ChooseNextSong();
			}
		}

		public void ChooseNextSong()
		{
			if (SingletonAudioLib.AudioTracks.Count == 0) return;
			double StepsPerMinute;
			lock (this)
			{
				if (LastOutput == null) return;
				StepsPerMinute = LastOutput.Value.Frequency * 60;
			}
			AudioTrack BestTrack = null;
			double MinDiff = double.PositiveInfinity;
			foreach (AudioTrack Track in SingletonAudioLib.AudioTracks)
			{
				if (SingletonAudioPlayer.SongsQueue.Contains(Track)) continue;
				if (SingletonAudioPlayer.SongsBefore.Contains(Track)) continue;

				double ThisDiff = Math.Abs(Track.BPM - StepsPerMinute);
				if (ThisDiff < MinDiff)
				{
					MinDiff = ThisDiff;
					BestTrack = Track;
				}
			}
			if (BestTrack != null) SingletonAudioPlayer.SongsQueue.Enqueue(BestTrack);
		}

		public void OnCompleted()
		{
			throw new NotImplementedException();
		}

		public void OnError(Exception error)
		{
			throw new NotImplementedException();
		}
	}
}
