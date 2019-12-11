using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using Karl.Model;
using Xamarin.Forms;

namespace Karl.ViewModel
{
	public class AudioLibPageVM
	{
		private AppLogic AppLogic;
		public ICommand TitleSortCommand;
		public ICommand ArtistSortCommand;
		public ICommand BPMSortCommand;
		private ObservableCollection<string[]> songs;

		public ObservableCollection<string[]> Songs
		{
			get
			{
				return songs;
			}
			set
			{
				songs = value;
				OnPropertyChanged("Songs");
			}
		}

		public AudioLibPageVM(AppLogic appLogic)
		{
			AppLogic = appLogic;
			Songs = new ObservableCollection<string[]>();
			TitleSortCommand = new Command(TitleSort);
			ArtistSortCommand = new Command(ArtistSort);
			BPMSortCommand = new Command(BPMSort);
		}

		private void TitleSort()
		{
			ObservableCollection<string[]> songs = new ObservableCollection<string[]>();
			//sort
			Songs = songs;
		}

		private void ArtistSort()
		{
			ObservableCollection<string[]> songs = new ObservableCollection<string[]>();
			//sort
			Songs = songs;
		}

		private void BPMSort()
		{
			ObservableCollection<string[]> songs = new ObservableCollection<string[]>();
			//sort
			Songs = songs;
		}

		private void GetSongs()
		{
			ObservableCollection<string[]> songs = new ObservableCollection<string[]>();
			//AppLogic
			Songs = songs;
		}

		private void PlaySong()
		{
			//Applogic
		}

		private void AddSong()
		{

		}

		private void SearchSong()
		{

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
