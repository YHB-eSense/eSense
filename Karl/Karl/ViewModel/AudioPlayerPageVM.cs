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
		private AudioLib _audioLib;
		//private Image _iconPlay;
		//private Image _iconPause;
		private AudioTrack _audioTrack;
		private Boolean _pausePlayBoolean;
		private int _volume;
		private double _currentPosition;
		//private Image _icon;

		/// <summary>
		/// Safes the state of the active Audio Track(True = Playing False=Pausing)
		/// </summary>
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
		/*
		public Image Icon
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
		*/

		/**
		 * Commands were called from Elements in AudioPlayerPage
		 * **/
		public ICommand PausePlayCommand { get; }
		public ICommand PlayPrevCommand { get; }
		public ICommand PlayNextCommand { get; }
		public ICommand ChangeVolumeCommand { get; }
		public ICommand MoveInSongCommand { get; }

		/// <summary>
		/// Initializises App Logic and all available Commands
		/// </summary>
		/// <param name="appLogic"> For needed functions in Model</param>
		public AudioPlayerPageVM()
		{
			_audioPlayer = AudioPlayer.SingletonAudioPlayer;
			_audioLib = AudioLib.SingletonAudioLib;
			PausePlayCommand = new Command(PausePlay);
			PlayPrevCommand = new Command(PlayPrev);
			PlayNextCommand = new Command(PlayNext);
			ChangeVolumeCommand = new Command<int>(ChangeVolume);
			MoveInSongCommand = new Command<double>(MoveInSong);
			//_iconPlay = ...
			//_iconPause = ...
			//Icon = new Image();
			//IconPlay = new Image(); //fileLocation
			//IconPlay.Source = ImageSource.FromFile("");
			//IconPause = new Image(); //fileLocation
		}

		/// <summary>
		/// Continues/Stops Song in App Logic and changes icon
		/// from Play to Pause/ Pause to Play
		/// </summary>
		private void PausePlay()
		{
			if (PausePlayBoolean)
			{
				//AudioLogic
				PausePlayBoolean = !PausePlayBoolean;
				//Icon = IconPlay;
			}
			else
			{
				//AudioLogic
				PausePlayBoolean = !PausePlayBoolean;
				//Icon = IconPause;
			}
		}

		/// <summary>
		/// Plays previous Song in App Logic
		/// </summary>
		private void PlayPrev()
		{
			//AudioLogic
			GetAudioTrack();
			OnPropertyChanged("AudioTrack.Duration");
			OnPropertyChanged("AudioTrack.Cover");
		}

		/// <summary>
		/// Plays next Song in App Logic
		/// </summary>
		private void PlayNext()
		{
			//AudioLogic
			GetAudioTrack();
			OnPropertyChanged("AudioTrack.Duration");
			OnPropertyChanged("AudioTrack.Cover");
		}

		/// <summary>
		/// Changes Volume in App Logic
		/// </summary>
		private void ChangeVolume(int volume)
		{
			//AudioLogic	//TODO
		}

		/// <summary>
		/// Changes the position in Song
		/// </summary>
		/// <param name="time">New Time in Song</param>
		private void MoveInSong(double time)
		{
			//AudioLogic	//TODO
		}

		/// <summary>
		/// App Logic load an Audio Track
		/// </summary>
		public void GetAudioTrack()
		{
			//stattdessen audioTrack von AppLogic holen
		}

		/// <summary>
		/// ?
		/// </summary>
		public void GetPausePlayBoolean()
		{
			Boolean pausePlayBoolean = false;
			//AppLogic
			PausePlayBoolean = pausePlayBoolean;
		}
		/// <summary>
		/// ?
		/// </summary>
		public void GetVolume()
		{
			//AppLogic
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
