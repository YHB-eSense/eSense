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
		private AppLogic AppLogic { get; }
		public AudioTrack AudioTrack { get; set; }
		public double Time { get; set; }
		public double Duration { get; set; }
		public ICommand PausePlayCommand { get; }
		public ICommand PlayPrevCommand { get; }
		public ICommand PlayNextCommand { get; }
		public ICommand ChangeVolumeCommand { get; }
		public ICommand MoveInSongCommand { get; }
		public Boolean PausePlayBoolean { get; set; }

		public AudioPlayerPageVM(AppLogic appLogic)
		{
			AppLogic = appLogic;
			PausePlayCommand = new Command(PausePlay);
			PlayPrevCommand = new Command(PlayPrev);
			PlayNextCommand = new Command(PlayNext);
			ChangeVolumeCommand = new Command<int>(ChangeVolume);
			MoveInSongCommand = new Command<double>(MoveInSong);
			Duration = AudioTrack.Duration;
			PausePlayBoolean = false;
		}

		private AudioTrack GetTrack()
		{
			AudioTrack audioTrack = new AudioTrack();
			//stattdessen audioTrack von AppLogic holen
			return audioTrack;
		}

		private double GetTime()
		{
			double time = 0;
			//stattdessen time von AppLogic holen
			return time;
		}

		public void PausePlay()
		{
			if (PausePlayBoolean)
			{
				//AudioLogic
				PausePlayBoolean = false;
				OnPropertyChanged("PausePlayBoolean");
			}
			else
			{
				//AudioLogic
				PausePlayBoolean = true;
				OnPropertyChanged("PausePlayBoolean");
			}
		}

		public void PlayPrev()
		{
			//AudioLogic
			AudioTrack = GetTrack();
			OnPropertyChanged("Duration");
		}

		public void PlayNext()
		{
			//AudioLogic
			AudioTrack = GetTrack();
			OnPropertyChanged("Duration");
		}

		public void ChangeVolume(int volume)
		{
			//AudioLogic	//TODO
		}

		public void MoveInSong(double time)
		{
			//AudioLogic	//TODO
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
