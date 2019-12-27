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
		private AudioTrack _audioTrack;
		private Boolean _pausePlayBoolean;
		private int _volume;
		private double _currentPosition;
		private string _icon;

		/**
		 Properties binded to AudioPlayerPage of View
		**/
		public Boolean PausePlayBoolean
		{
			get
			{
				return _pausePlayBoolean;
			}
			set
			{
				if(value != _pausePlayBoolean)
				{
					_pausePlayBoolean = value;
					OnPropertyChanged("PausePlayBoolean");
				}
			}
		}

		public AudioTrack AudioTrack
		{
			get
			{
				return _audioTrack;
			}
			set
			{
				_audioTrack = value;
				OnPropertyChanged("AudioTrack");
			}
		}

		public int Volume
		{
			get
			{
				return _volume;
			}
			set
			{
				_volume = value;
				OnPropertyChanged("Volume");
			}
		}

		public double CurrentPosition
		{
			get
			{
				return _currentPosition;
			}
			set
			{
				_currentPosition = value;
				OnPropertyChanged("CurrentPosition");
			}
		}
		
		public string Icon
		{
			get
			{
				return _icon;
			}
			set
			{
				if (_icon != value)
				{
					_icon = value;
					OnPropertyChanged("Icon");
				}
			}
		}

		public string TimePlayed
		{
			get
			{
				return "00:00";
				//return Convert.ToString(_audioPlayer.CurrentSecInTrack);
			}
		}

		public string TimeLeft
		{
			get
			{
				return "-03:00";
				//return Convert.ToString(_audioTrack.Duration - _audioPlayer.CurrentSecInTrack); 
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
			Icon = _iconPlay;
		}

		/// <summary>
		/// Pauses/Plays song in AudioPlayer of Model
		/// </summary>
		private void PausePlay()
		{
			if (PausePlayBoolean)
			{
				//_audioPlayer.TogglePlay();
				GetPausePlayBoolean();
			}
			else
			{
				_audioPlayer.TogglePause();
				GetPausePlayBoolean();
			}
		}

		/// <summary>
		/// Plays previous song in AudioPlayer of Model
		/// </summary>
		private void PlayPrev()
		{
			_audioPlayer.PrevTrack();
			GetAudioTrack();
		}

		/// <summary>
		/// Plays next song in AudioPlayer of Model
		/// </summary>
		private void PlayNext()
		{
			_audioPlayer.NextTrack();
			GetAudioTrack();
		}

		/// <summary>
		/// Changes Volume in AudioPlayer of Model
		/// </summary>
		private void ChangeVolume(int volume)
		{
			_audioPlayer.Volume = volume;
		}

		/// <summary>
		/// Changes the position in song
		/// </summary>
		/// <param name="time">New position in song</param>
		private void MoveInSong(double time)
		{
			_audioPlayer.CurrentSecInTrack = time;
		}

		/// <summary>
		/// Retrieves active AudioTrack from AudioPlayer in Model
		/// </summary>
		public void GetAudioTrack()
		{
			AudioTrack = _audioPlayer.CurrentTrack;
		}

		/// <summary>
		/// ?
		/// </summary>
		public void GetPausePlayBoolean()
		{
			PausePlayBoolean = _audioPlayer.Paused;
			if (PausePlayBoolean)
			{
				Icon = _iconPlay;
			}
			else
			{
				Icon = _iconPause;
			}
		}

		/// <summary>
		/// ?
		/// </summary>
		public void GetVolume()
		{
			Volume = _audioPlayer.Volume;
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
