using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;
using Karl.Model;

namespace Karl.ViewModel
{
	public class AudioPlayerPageVM : INotifyPropertyChanged
	{
		private AppLogic AppLogic;
		public AudioTrack AudioTrack;
		public ICommand PausePlayCommand;
		public ICommand PlayPrevCommand;
		public ICommand PlayNextCommand;
		public ICommand ChangeVolumeCommand;
		public ICommand MoveInSongCommand;
		private Boolean pausePlayBoolean;


		public Boolean PausePlayBoolean
		{
			get
			{
				return pausePlayBoolean;
			}
			set
			{
				if(value =!pausePlayBoolean)
				{
					pausePlayBoolean = value;
					OnPropertyChanged("PausePlayBoolean");
				}
			}
		}




		public AudioPlayerPageVM(AppLogic appLogic)
		{
			AppLogic = appLogic;
			PausePlayCommand = new Command(PausePlay);
			PlayPrevCommand = new Command(PlayPrev);
			PlayNextCommand = new Command(PlayNext);
			ChangeVolumeCommand = new Command<int>(ChangeVolume);
			MoveInSongCommand = new Command<double>(MoveInSong);
		}

		public void PausePlay()
		{
			if (PausePlayBoolean)
			{
				//AudioLogic
				PausePlayBoolean = !PausePlayBoolean;
			}
			else
			{
				//AudioLogic
				PausePlayBoolean = !PausePlayBoolean;
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
			AudioTrack audioTrack = new AudioTrack();
			//stattdessen audioTrack von AppLogic holen
			AudioTrack = audioTrack;
		}

		public void GetPausePlayBoolean()
		{
			Boolean pausePlayBoolean = false;
			//AppLogic
			PausePlayBoolean = pausePlayBoolean;
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
