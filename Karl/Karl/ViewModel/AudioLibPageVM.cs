using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Karl.Model;
using Xamarin.Forms;

namespace Karl.ViewModel
{
	public class AudioLibPageVM
	{
		private AppLogic AppLogic;
		private ObservableCollection<string> songs;

		public ObservableCollection<string> Songs
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

		public ICommand TitleSortCommand { get; }
		public ICommand ArtistSortCommand { get; }
		public ICommand BPMSortCommand { get; }
		public ICommand PlaySongCommand { get; }
		public ICommand AddSongCommand { get; }
		public ICommand SearchCommand { get; }

		public AudioLibPageVM(AppLogic appLogic)
		{
			AppLogic = appLogic;
			Songs = new ObservableCollection<string>();
			TitleSortCommand = new Command(TitleSort);
			ArtistSortCommand = new Command(ArtistSort);
			BPMSortCommand = new Command(BPMSort);
			PlaySongCommand = new Command<string>(PlaySong);
			AddSongCommand = new Command(AddSong);
			SearchCommand = new Command<string>(SearchSong);
		}

		private void TitleSort()
		{
			ObservableCollection<string> songs = new ObservableCollection<string>();
			//sort
			Songs = songs;
		}

		private void ArtistSort()
		{
			ObservableCollection<string> songs = new ObservableCollection<string>();
			//sort
			Songs = songs;
		}

		private void BPMSort()
		{
			ObservableCollection<string> songs = new ObservableCollection<string>();
			//sort
			Songs = songs;
		}

		private void PlaySong(string text)
		{
			//Applogic
		}

		private void AddSong()
		{
			NavigationHandler.GotoAddSongPage();
		}

		private void SearchSong(string text)
		{
			Songs = (ObservableCollection<string>) Songs.Where(song => song.Contains(text));
			//reset
		}

		public void GetSongs()
		{
			ObservableCollection<string> songs = new ObservableCollection<string>();
			//AppLogic
			songs.Add("TNT");
			Songs = songs;
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
