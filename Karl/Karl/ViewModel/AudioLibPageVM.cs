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
	public class AudioLibPageVM : INotifyPropertyChanged
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

		/**
		 Commands were called from Elements in AudioLibPage
		**/
		public ICommand TitleSortCommand { get; }
		public ICommand ArtistSortCommand { get; }
		public ICommand BPMSortCommand { get; }
		public ICommand PlaySongCommand { get; }
		public ICommand AddSongCommand { get; }
		public ICommand SearchCommand { get; }

		/// <summary>
		/// Initializises App Logic and all available Commands
		/// </summary>
		/// <param name="appLogic"> For needed functions in Model</param>
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

		/// <summary>
		/// Sorts Titles by Name
		/// </summary>
		private void TitleSort()
		{
			ObservableCollection<AudioTrack> songs = new ObservableCollection<AudioTrack>();
			//sort
			Songs = songs;
		}

		/// <summary>
		/// Sorts Titles by Artist
		/// </summary>
		private void ArtistSort()
		{
			ObservableCollection<AudioTrack> songs = new ObservableCollection<AudioTrack>();
			//sort
			Songs = songs;
		}

		/// <summary>
		/// Sorts Titles by BPM
		/// </summary>
		private void BPMSort()
		{
			ObservableCollection<AudioTrack> songs = new ObservableCollection<AudioTrack>();
			//sort
			Songs = songs;
		}

		/// <summary>
		/// Starts Playing song "audioTrack" in App Logic 
		/// </summary>
		/// <param name="audioTrack">Name of started song</param>
		private void PlaySong(AudioTrack audioTrack)
		{
			//Applogic
			NavigationHandler.GotoAudioPlayerPage();
		}

		/// <summary>
		/// Navigates to AddSongPage
		/// </summary>
		private void AddSong()
		{
			NavigationHandler.GotoAddSongPage();
		}

		/// <summary>
		/// Refreshs Listview so it only shows song which
		/// contain "title" in their song title
		/// </summary>
		/// <param name="title"></param>
		private void SearchSong(string title)
		{
			Songs = (ObservableCollection<AudioTrack>) Songs.Where(song => song.Title.Contains(title));
			//reset
		}

		/// <summary>
		/// ?
		/// </summary>
		public void GetSongs()
		{
			ObservableCollection<AudioTrack> songs = new ObservableCollection<AudioTrack>();
			//AppLogic
			//songs.Add(new AudioTrack("TNT", "ACDC", 130));
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
