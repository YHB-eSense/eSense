using System;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;
using Karl.Model;

namespace Karl.ViewModel
{
	public class AudioPlayerPageVM : INotifyPropertyChanged
	{
		private AppLogic AppLogic;
		//private Image IconPlay;
		//private Image IconPause;
		private AudioTrack AudioTrack;
		private Boolean pausePlayBoolean;
		//private Image icon;

		/** True = Playing False=Pausing **/
		public Boolean PausePlayBoolean
		{
			get
			{
				return pausePlayBoolean;
			}
			set
			{
				if(value =! pausePlayBoolean)
				{
					pausePlayBoolean = value;
					OnPropertyChanged("PausePlayBoolean");
				}
			}
		}
		/*
		public Image Icon
		{
			get
			{
				return icon;
			}
			set
			{
				if (icon != value)
				{
					icon = value;
					OnPropertyChanged("Icon");
				}
			}
		}
		*/
		public ICommand PausePlayCommand { get; }
		public ICommand PlayPrevCommand { get; }
		public ICommand PlayNextCommand { get; }
		public ICommand ChangeVolumeCommand { get; }
		public ICommand MoveInSongCommand { get; }

		public AudioPlayerPageVM(AppLogic appLogic)
		{
			AppLogic = appLogic;
			PausePlayCommand = new Command(PausePlay);
			PlayPrevCommand = new Command(PlayPrev);
			PlayNextCommand = new Command(PlayNext);
			ChangeVolumeCommand = new Command<int>(ChangeVolume);
			MoveInSongCommand = new Command<double>(MoveInSong);
			//Icon = new Image();
			//IconPlay = new Image(); //fileLocation
			//IconPlay.Source = ImageSource.FromFile("");
			//IconPause = new Image(); //fileLocation
			PausePlayBoolean = false;
		}

		public void PausePlay()
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

		public void PlayPrev()
		{
			//AudioLogic
			GetAudioTrack();
			OnPropertyChanged("AudioTrack.Duration");
			OnPropertyChanged("AudioTrack.Cover");
		}

		public void PlayNext()
		{
			//AudioLogic
			GetAudioTrack();
			OnPropertyChanged("AudioTrack.Duration");
			OnPropertyChanged("AudioTrack.Cover");
		}

		public void ChangeVolume(int volume)
		{
			//AudioLogic	//TODO
		}

		public void MoveInSong(double time)
		{
			//AudioLogic	//TODO
		}

		public void GetAudioTrack()
		{
			//stattdessen audioTrack von AppLogic holen
		}

		public void GetPausePlayBoolean()
		{
			Boolean pausePlayBoolean = false;
			//AppLogic
			PausePlayBoolean = pausePlayBoolean;
		}

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
