using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using StepDetectionLibrary;

namespace Karl.Model
{
	/// <summary>
	/// The Motivation Mode.
	/// </summary>
	class MotivationMode : Mode
	{
		private AudioPlayer _audioPlayer = AudioPlayer.SingletonAudioPlayer;
		private LangManager _langManager = LangManager.SingletonLangManager;
		Nullable<Output> Output;
		ObservableCollection<AudioTrack> Library = AudioLib.SingletonAudioLib.AudioTracks;

		public override string Name
		{
			get => _langManager.CurrentLang.Get("motivation_mode");
		}

		public void ChooseNextSong()
		{
			if (AudioLib.SingletonAudioLib.AudioTracks.Count == 0) return; //todo
			double StepsPerMinute;
			lock (this)
			{
				if (Output == null) return;
				if (Output.Value.Frequency == 0) return; //todo
				StepsPerMinute = Output.Value.Frequency * 60;
			}
			AudioTrack BestTrack = null;
			double MinDiff = Double.PositiveInfinity;
			foreach (AudioTrack Track in Library)
			{
				if (_audioPlayer.SongsQueue.Contains(Track)) continue;

				double ThisDiff = Math.Abs(Track.BPM - StepsPerMinute);
				if (ThisDiff < MinDiff)
				{
					MinDiff = ThisDiff;
					BestTrack = Track;
				}
			}
			if (BestTrack != null) _audioPlayer.SongsQueue.Enqueue(BestTrack);
		}

		public override void Activate()
		{
			//if (!AudioPlayer.Paused) AudioPlayer.TogglePause();
			_audioPlayer.Clear();
			_audioPlayer.NextSongEvent += ChooseNextSong;
			//throw new NotImplementedException(); //todo
		}

		public override void Deactivate()
		{
			_audioPlayer.NextSongEvent -= ChooseNextSong;
			//throw new NotImplementedException(); //todo
		}

		protected override String UpdateName(Lang value)
		{
			return "MotivateMode"; //value.get("mode_motivate");//todo
		}

		private class StepDetectionObserver : IObserver<Output>
		{
			MotivationMode parent;
			public StepDetectionObserver(MotivationMode parent)
			{
				this.parent = parent;
			}
			public void OnCompleted()
			{
				throw new NotImplementedException(); //todo
			}

			public void OnError(Exception error)
			{
				throw new NotImplementedException(); //todo
			}

			public void OnNext(Output value)
			{
				lock (parent)
				{
					parent.Output = value;//todo
				}
			}
		}
	}
}
