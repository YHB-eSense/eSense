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
		private NavigationHandler _handler;
		private AudioLib _audioLib;
		private AudioPlayer _audioPlayer;
		private ObservableCollection<AudioTrack> _oldSongs;

		/**
		 Properties binded to AudioLibPage of View
		**/
		public ObservableCollection<AudioTrack> Songs
		{
			get { return _audioLib.AudioTracks; }
			set { _audioLib.AudioTracks = value; OnPropertyChanged("Songs");}
		}

		public AudioTrack SelectedSong { get; set; }

		/**
		 Commands binded to AudioLibPage of View
		**/
		public ICommand TitleSortCommand { get; }
		public ICommand ArtistSortCommand { get; }
		public ICommand BPMSortCommand { get; }
		public ICommand PlaySongCommand { get; }
		public ICommand AddSongCommand { get; }
		public ICommand SearchSongCommand { get; }

		/// <summary>
		/// Initializises Commands, NavigationHandler and AudioLib, AudioPlayer of Model
		/// </summary>
		/// <param name="handler">For navigation</param>
		public AudioLibPageVM(NavigationHandler handler)
		{
			_handler = handler;
			_audioLib = AudioLib.SingletonAudioLib;
			_audioPlayer = AudioPlayer.SingletonAudioPlayer;
			_oldSongs = null;
			TitleSortCommand = new Command(TitleSort);
			ArtistSortCommand = new Command(ArtistSort);
			BPMSortCommand = new Command(BPMSort);
			PlaySongCommand = new Command(PlaySong);
			AddSongCommand = new Command(AddSong);
			SearchSongCommand = new Command<string>(SearchSong);
		}

		/// <summary>
		/// Sorts Titles by Name
		/// </summary>
		private void TitleSort()
		{
			Songs = new ObservableCollection<AudioTrack>(Songs.OrderBy(s => s.Title));
		}

		/// <summary>
		/// Sorts Titles by Artist
		/// </summary>
		private void ArtistSort()
		{
			Songs = new ObservableCollection<AudioTrack>(Songs.OrderBy(s => s.Artist));
		}

		/// <summary>
		/// Sorts Titles by BPM
		/// </summary>
		private void BPMSort()
		{
			Songs = new ObservableCollection<AudioTrack>(Songs.OrderBy(s => s.BPM));
		}

		/// <summary>
		/// Jumps to AudioPlayer in Model
		/// </summary>
		private void PlaySong()
		{
			_handler.GotoPage(_handler._pages[0]);
			if (SelectedSong != _audioPlayer.CurrentTrack)
			{
				_audioPlayer.PlayTrack(SelectedSong);
			}	
		}

		/// <summary>
		/// Navigates to AddSongPage
		/// </summary>
		private void AddSong()
		{
			_handler.GotoPage(_handler._pages[5]);
		}

		/// <summary>
		/// Sets Songs to only contain AudioTracks with title in their Title property
		/// </summary>
		/// <param name="title"></param>
		private void SearchSong(string title)
		{
			if (_oldSongs == null)
			{
				_oldSongs = new ObservableCollection<AudioTrack>(Songs);
			}
			if (title == null || title == "")
			{
				Songs = new ObservableCollection<AudioTrack>(_oldSongs);
				_oldSongs = null;
			}
			else
			{
				Songs = new ObservableCollection<AudioTrack>(Songs.Where(song => song.Title.Contains(title)));
			}
		}

		public void RefreshPage()
		{
			OnPropertyChanged("Songs");
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
