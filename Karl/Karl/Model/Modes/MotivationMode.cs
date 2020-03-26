using StepDetectionLibrary;
using System;
using System.Diagnostics;
using static Karl.Model.AudioLib;
using static Karl.Model.AudioPlayer;
using static Karl.Model.LangManager;
using static StepDetectionLibrary.OutputManager;

namespace Karl.Model
{
	/// <summary>
	/// The Motivation Mode.
	/// </summary>
	public class MotivationMode : Mode, IObserver<Output>
	{

		private uint _currentBPM;

		private IDisposable _stepDetectionDisposable;

		/// <summary>
		/// If current step rate and current song BPM difer by more than this threshold, a new song will prematurely be chosen.
		/// </summary>
		public double MaxAllowedBPMDiff = 15;

		public override string Name
		{
			get => SingletonLangManager.CurrentLang.Get("motivation_mode");
		}

		protected override bool Activate()
		{
			//Debug.WriteLine("Activating mode '{0}'", args: Name);
			SingletonAudioPlayer.Clear();
			SingletonAudioPlayer.NextSongEvent += ChooseNextSong;
			_stepDetectionDisposable = SingletonOutputManager.Subscribe(this);
			if (SingletonAudioPlayer.Paused)
			{
				ChooseNextSong();
				SingletonAudioPlayer.NextTrack();
			}
			return true;
		}

		protected override bool Deactivate()
		{
			SingletonAudioPlayer.NextSongEvent -= ChooseNextSong;
			if (_stepDetectionDisposable != null)
			{
				_stepDetectionDisposable.Dispose();
				_stepDetectionDisposable = null;
			}
			return true;
		}

		public void OnNext(Output value)
		{
			_currentBPM = (uint)(value.Log.AverageStepFrequency(duration: TimeSpan.FromSeconds(10)) * 60);

			if (SingletonAudioPlayer.CurrentTrack == null)
			{
				ChooseNextSong();
			}
			else if (Math.Abs(SingletonAudioPlayer.CurrentTrack.BPM - _currentBPM) > MaxAllowedBPMDiff)
			{
				ChooseNextSong();
			}
		}

		public void ChooseNextSong()
		{
			//Debug.WriteLine("Choosing next song...");
			AudioTrack BestTrack = null;
			double MinDiff = MaxAllowedBPMDiff;
			foreach (AudioTrack Track in SingletonAudioLib.AudioTracks)
			{
				// if (SingletonAudioPlayer.SongsQueue.Contains(Track)) continue;
				// if (SingletonAudioPlayer.SongsBefore.Contains(Track)) continue;

				double ThisDiff = Math.Abs(Track.BPM - _currentBPM);

				if (ThisDiff <= MinDiff)
				{
					MinDiff = ThisDiff;
					BestTrack = Track;
				}
			}
			if (BestTrack != null)
			{
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
