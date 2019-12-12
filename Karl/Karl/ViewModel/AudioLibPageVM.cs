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
		private ObservableCollection<AudioTrack> songs;

		public ObservableCollection<AudioTrack> Songs
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
			Songs = new ObservableCollection<AudioTrack>();
			TitleSortCommand = new Command(TitleSort);
			ArtistSortCommand = new Command(ArtistSort);
			BPMSortCommand = new Command(BPMSort);
			PlaySongCommand = new Command<AudioTrack>(PlaySong);
			AddSongCommand = new Command(AddSong);
			SearchCommand = new Command<string>(SearchSong);
		}

		private void TitleSort()
		{
			ObservableCollection<AudioTrack> songs = new ObservableCollection<AudioTrack>();
			//sort
			Songs = songs;
		}

		private void ArtistSort()
		{
			ObservableCollection<AudioTrack> songs = new ObservableCollection<AudioTrack>();
			//sort
			Songs = songs;
		}

		private void BPMSort()
		{
			ObservableCollection<AudioTrack> songs = new ObservableCollection<AudioTrack>();
			//sort
			Songs = songs;
		}

		private void PlaySong(AudioTrack audioTrack)
		{
			//Applogic
			NavigationHandler.GoBack();
		}

		private void AddSong()
		{
			NavigationHandler.GotoAddSongPage();
		}

		private void SearchSong(string text)
		{
			Songs = (ObservableCollection<AudioTrack>) Songs.Where(song => song.Title.Contains(text));
			//reset
		}

		public void GetSongs()
		{
			ObservableCollection<AudioTrack> songs = new ObservableCollection<AudioTrack>();
			//AppLogic
			//songs.Add(new AudioTrack("TNT", "ACDC", 130, "location"));
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
