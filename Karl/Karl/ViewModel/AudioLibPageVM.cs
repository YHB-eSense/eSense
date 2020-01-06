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
		private List<AudioTrack> _deleteList;

		/**
		 Properties binded to AudioLibPage of View
		**/
		public ObservableCollection<AudioTrack> Songs
		{
			get => _audioLib.AudioTracks;
			set { _audioLib.AudioTracks = value; OnPropertyChanged("Songs");}
		}

		/**
		 Commands binded to AudioLibPage of View
		**/
		public ICommand TitleSortCommand { get; }
		public ICommand ArtistSortCommand { get; }
		public ICommand BPMSortCommand { get; }
		public ICommand PlaySongCommand { get; }
		public ICommand AddSongCommand { get; }
		public ICommand SearchSongCommand { get; }
		public ICommand DeleteSongsCommand { get; }
		public ICommand EditDeleteListCommand { get; }

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
			_deleteList = new List<AudioTrack>();
			TitleSortCommand = new Command(TitleSort);
			ArtistSortCommand = new Command(ArtistSort);
			BPMSortCommand = new Command(BPMSort);
			PlaySongCommand = new Command<AudioTrack>(PlaySong);
			AddSongCommand = new Command(AddSong);
			SearchSongCommand = new Command<string>(SearchSong);
			DeleteSongsCommand = new Command(DeleteSongs);
			EditDeleteListCommand = new Command<AudioTrack>(EditDeleteList);
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
		private void PlaySong(AudioTrack track)
		{
			_handler.GotoPage(_handler._pages[0]);
			if (track != _audioPlayer.CurrentTrack) { _audioPlayer.PlayTrack(track); }	
		}

		/// <summary>
		/// Navigates to AddSongPage
		/// </summary>
		private void AddSong()
		{
			_handler.GotoPage(_handler._pages[5]);
		}

		private void EditDeleteList(AudioTrack song)
		{
			if (_deleteList.Contains(song)) { _deleteList.Remove(song); }
			else { _deleteList.Add(song); }
		}

		/// <summary>
		/// Sets Songs to only contain AudioTracks with title in their Title property
		/// </summary>
		/// <param name="title"></param>
		private void SearchSong(string title)
		{
			if (_oldSongs == null){ _oldSongs = new ObservableCollection<AudioTrack>(Songs); }
			if (title == null || title == "")
			{
				Songs = new ObservableCollection<AudioTrack>(_oldSongs);
				_oldSongs = null;
			}
			else { Songs = new ObservableCollection<AudioTrack>(Songs.Where(song => song.Title.Contains(title))); }
		}

		private async void DeleteSongs()
		{
			bool answer = await Application.Current.MainPage.DisplayAlert("Question?", "Are you sure you want to delete the selected Songs?", "Yes", "No");
			if (answer) {
				foreach(AudioTrack song in _deleteList) { _audioLib.DeleteTrack(song); }
				OnPropertyChanged("Songs");
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
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

	}
}
