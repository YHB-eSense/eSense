using System;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;
using Karl.Model;

namespace Karl.ViewModel
{
	public class AudioPlayerPageVM : INotifyPropertyChanged
	{
		private AudioPlayer _audioPlayer;
		private string _iconPlay;
		private string _iconPause;
		private string _icon;

		/**
		 Properties binded to AudioPlayerPage of View
		**/
		public AudioTrack AudioTrack
		{
			get { return _audioPlayer.CurrentTrack; }
		}

		public double Duration
		{
			get { return _audioPlayer.Duration; }
		}

		public double Volume
		{
			get { return _audioPlayer.Volume * 100; }
			set { _audioPlayer.Volume = value / 100; OnPropertyChanged("Volume"); }
		}

		public double CurrentPosition
		{
			get { return _audioPlayer.CurrentSecInTrack; }
			set { _audioPlayer.CurrentSecInTrack = value; OnPropertyChanged("CurrentPosition"); }
		}
		
		public string Icon
		{
			get { return _icon; }
			set { _icon = value; OnPropertyChanged("Icon"); }
		}

		public string TimePlayed
		{
			get { return string.Format("{0}:{1:00}", (int)TimeSpan.FromSeconds(CurrentPosition).TotalMinutes, TimeSpan.FromSeconds(CurrentPosition).Seconds); }
		}

		public string TimeLeft
		{
			get {
				return string.Format("{0}:{1:00}", (int)TimeSpan.FromSeconds(Duration - CurrentPosition).TotalMinutes, TimeSpan.FromSeconds(Duration - CurrentPosition).Seconds);
			}
		}

		/**
		 Commands binded to AudioPlayerPage of View
		**/
		public ICommand PausePlayCommand { get; }
		public ICommand PlayPrevCommand { get; }
		public ICommand PlayNextCommand { get; }

		/// <summary>
		/// Initializises Commands, Images and AudioPlayer of Model
		/// </summary>
		public AudioPlayerPageVM()
		{
			_audioPlayer = AudioPlayer.SingletonAudioPlayer;
			PausePlayCommand = new Command(PausePlay);
			PlayPrevCommand = new Command(PlayPrev);
			PlayNextCommand = new Command(PlayNext);
			_iconPlay = "play.png";
			_iconPause = "pause.png";
			Icon = _iconPause;
		}

		public void RefreshPage()
		{
			if (_audioPlayer.Paused)
			{
				Icon = _iconPlay;
			}
			else
			{
				Icon = _iconPause;
			}
			OnPropertyChanged("CurrentPosition");
			OnPropertyChanged("Duration");
			OnPropertyChanged("Volume");
			OnPropertyChanged("TimePlayed");
			OnPropertyChanged("TimeLeft");
			OnPropertyChanged("AudioTrack");
			/*
			
			
			
			
			*/
		}

		/// <summary>
		/// Pauses/Plays song in AudioPlayer of Model
		/// </summary>
		private void PausePlay()
		{
			_audioPlayer.TogglePause();
			if (_audioPlayer.Paused)
			{
				Icon = _iconPlay;
			}
			else
			{
				Icon = _iconPause;
			}
		}

		/// <summary>
		/// Plays previous song in AudioPlayer of Model
		/// </summary>
		private void PlayPrev()
		{
			//_audioPlayer.PrevTrack();
			OnPropertyChanged("AudioTrack");
		}

		/// <summary>
		/// Plays next song in AudioPlayer of Model
		/// </summary>
		private void PlayNext()
		{
			//_audioPlayer.NextTrack();
			OnPropertyChanged("AudioTrack");
		}

		//Eventhandling

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChanged(string propertyName)
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

	}
}
