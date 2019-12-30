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

		public int Volume
		{
			get { return _audioPlayer.Volume; }
			set { _audioPlayer.Volume = value; OnPropertyChanged("Volume"); }
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
			get { return "0"; //TimeSpan.FromSeconds(CurrentPosition).ToString();
			}
		}

		public string TimeLeft
		{
			get { return "0"; //TimeSpan.FromSeconds(CurrentPosition - AudioTrack.Duration).ToString();
			}
		}

		/**
		 Commands binded to AudioPlayerPage of View
		**/
		public ICommand PausePlayCommand { get; }
		public ICommand PlayPrevCommand { get; }
		public ICommand PlayNextCommand { get; }
		public ICommand ChangeVolumeCommand { get; }
		public ICommand MoveInSongCommand { get; }

		/// <summary>
		/// Initializises Commands, Images and AudioPlayer of Model
		/// </summary>
		public AudioPlayerPageVM()
		{
			_audioPlayer = AudioPlayer.SingletonAudioPlayer;
			PausePlayCommand = new Command(PausePlay);
			PlayPrevCommand = new Command(PlayPrev);
			PlayNextCommand = new Command(PlayNext);
			ChangeVolumeCommand = new Command<int>(ChangeVolume);
			MoveInSongCommand = new Command<double>(MoveInSong);
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
			OnPropertyChanged("AudioTrack");
			/*
			OnPropertyChanged("TimePlayed");
			OnPropertyChanged("TimeLeft");
			OnPropertyChanged("CurrentPosition");
			OnPropertyChanged("Volume");
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

		/// <summary>
		/// Changes Volume in AudioPlayer of Model
		/// </summary>
		private void ChangeVolume(int volume)
		{
			//_audioPlayer.Volume = volume;
		}

		/// <summary>
		/// Changes the position in song
		/// </summary>
		/// <param name="time">New position in song</param>
		private void MoveInSong(double time)
		{
			//_audioPlayer.CurrentSecInTrack = time;
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
