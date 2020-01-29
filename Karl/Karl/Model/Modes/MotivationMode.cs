using System;
using StepDetectionLibrary;
using static Karl.Model.AudioPlayer;
using static Karl.Model.AudioLib;
using static Karl.Model.LangManager;
using static StepDetectionLibrary.OutputManager;
using System.Diagnostics;

namespace Karl.Model
{
	/// <summary>
	/// The Motivation Mode.
	/// </summary>
	public class MotivationMode : Mode, IObserver<Output>
	{
		/// <summary>
		/// If current step rate and current song BPM difer by more than this threshold, a new song will prematurely be chosen.
		/// </summary>
		public double MaxAllowedBPMDiff = 15;

		private uint CurrentBPM;

		private IDisposable StepDetectionDisposable;

		public override string Name
		{
			get => SingletonLangManager.CurrentLang.Get("motivation_mode");
		}

		protected override bool Activate()
		{
			Debug.WriteLine("Activating mode '{0}'", args: Name);
			SingletonAudioPlayer.Clear();
			SingletonAudioPlayer.NextSongEvent += ChooseNextSong;
			StepDetectionDisposable = SingletonOutputManager.Subscribe(this);
			if (SingletonAudioPlayer.Paused)
			{
				ChooseNextSong();
				SingletonAudioPlayer.NextTrack();
			}
			return true;
		}

		protected override bool Deactivate()
		{
			Debug.WriteLine("Deactivating mode '{0}'", args: Name);
			SingletonAudioPlayer.NextSongEvent -= ChooseNextSong;
			if (StepDetectionDisposable != null)
			{
				StepDetectionDisposable.Dispose();
				StepDetectionDisposable = null;
			}
			return true;
		}

		public void OnNext(Output value)
		{
			CurrentBPM = (uint)(value.Frequency * 60);

			if (SingletonAudioPlayer.CurrentTrack == null)
			{
				ChooseNextSong();
			}
			else if (Math.Abs(SingletonAudioPlayer.CurrentTrack.BPM - CurrentBPM) > MaxAllowedBPMDiff)
			{
				ChooseNextSong();
			}
		}

		public void ChooseNextSong()
		{
			Debug.WriteLine("Choosing next song...");
			AudioTrack BestTrack = null;
			double MinDiff = MaxAllowedBPMDiff;
			foreach (AudioTrack Track in SingletonAudioLib.AudioTracks)
			{
				// if (SingletonAudioPlayer.SongsQueue.Contains(Track)) continue;
				// if (SingletonAudioPlayer.SongsBefore.Contains(Track)) continue;

				double ThisDiff = Math.Abs(Track.BPM - CurrentBPM);

				if (ThisDiff <= MinDiff)
				{
					MinDiff = ThisDiff;
					BestTrack = Track;
				}
			}
			if (BestTrack != null)
			{
				Debug.WriteLine("... found one: {0} ({1} BPM-diff)", BestTrack.Title, MinDiff);
				SingletonAudioPlayer.SongsQueue.Enqueue(BestTrack);
				SingletonAudioPlayer.NextTrack();
				if (SingletonAudioPlayer.Paused) SingletonAudioPlayer.TogglePause();
			}
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
