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
		private SettingsHandler _settingsHandler;
		private NavigationHandler _handler;
		private AudioLib _audioLib;
		private AudioPlayer _audioPlayer;
		private LangManager _langManager;
		private ObservableCollection<AudioTrack> _oldSongs;
		private List<AudioTrack> _deleteList;

		private Color _chosenColor = Color.SkyBlue;
		private Color _titleSortColor = Color.SkyBlue;
		private Color _titleSortTextColor = Color.White;
		private Color _artistSortColor = Color.Transparent;
		private Color _artistSortTextColor = Color.Black;
		private Color _bpmSortColor = Color.Transparent;
		private Color _bpmSortTextColor = Color.Black;

		/**
		 Properties binded to AudioLibPage of View
		**/
		public CustomColor CurrentColor { get => _settingsHandler.CurrentColor; }
		public string TitleLabel { get => _langManager.CurrentLang.Get("title"); }
		public string ArtistLabel { get => _langManager.CurrentLang.Get("artist"); }
		public string BPMLabel { get => _langManager.CurrentLang.Get("bpm"); }
		public ObservableCollection<AudioTrack> Songs
		{
			get => _audioLib.AudioTracks;
			set { _audioLib.AudioTracks = value; OnPropertyChanged("Songs");}
		}

		public Color TitleSortColor {
			get => _titleSortColor;
			set { _titleSortColor = value; OnPropertyChanged("TitleSortColor"); }
		}

		public Color TitleSortTextColor
		{
			get => _titleSortTextColor;
			set { _titleSortTextColor = value; OnPropertyChanged("TitleSortTextColor"); }
		}

		public Color ArtistSortColor
		{
			get => _artistSortColor;
			set { _artistSortColor = value; OnPropertyChanged("ArtistSortColor"); }
		}

		public Color ArtistSortTextColor
		{
			get => _artistSortTextColor;
			set { _artistSortTextColor = value; OnPropertyChanged("ArtistSortTextColor"); }
		}

		public Color BPMSortColor
		{
			get => _bpmSortColor;
			set { _bpmSortColor = value; OnPropertyChanged("BPMSortColor"); }
		}

		public Color BPMSortTextColor
		{
			get => _bpmSortTextColor;
			set { _bpmSortTextColor = value; OnPropertyChanged("BPMSortTextColor"); }
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
			_settingsHandler = SettingsHandler.SingletonSettingsHandler;
			_audioLib = AudioLib.SingletonAudioLib;
			_audioPlayer = AudioPlayer.SingletonAudioPlayer;
			_langManager = LangManager.SingletonLangManager;
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
			TitleSortColor = _chosenColor;
			ArtistSortColor = Color.Transparent;
			BPMSortColor = Color.Transparent;
			TitleSortTextColor = Color.White;
			ArtistSortTextColor = Color.Black;
			BPMSortTextColor = Color.Black;
		}

		/// <summary>
		/// Sorts Titles by Artist
		/// </summary>
		private void ArtistSort()
		{
			Songs = new ObservableCollection<AudioTrack>(Songs.OrderBy(s => s.Artist));
			TitleSortColor = Color.Transparent;
			ArtistSortColor = _chosenColor;
			BPMSortColor = Color.Transparent;
			TitleSortTextColor = Color.Black;
			ArtistSortTextColor = Color.White;
			BPMSortTextColor = Color.Black;
		}

		/// <summary>
		/// Sorts Titles by BPM
		/// </summary>
		private void BPMSort()
		{
			Songs = new ObservableCollection<AudioTrack>(Songs.OrderBy(s => s.BPM));
			TitleSortColor = Color.Transparent;
			ArtistSortColor = Color.Transparent;
			BPMSortColor = _chosenColor;
			TitleSortTextColor = Color.Black;
			ArtistSortTextColor = Color.Black;
			BPMSortTextColor = Color.White;
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
			bool answer = await Application.Current.MainPage.DisplayAlert(_langManager.CurrentLang.Get("question_title"),
					_langManager.CurrentLang.Get("question_text"), _langManager.CurrentLang.Get("question_no"),
					_langManager.CurrentLang.Get("question_yes"));
			if (answer) {
				foreach(AudioTrack song in _deleteList) { _audioLib.DeleteTrack(song); }
				OnPropertyChanged("Songs");
			}
		}

		public void RefreshPage()
		{
			OnPropertyChanged("Songs");
			OnPropertyChanged("TitleLabel");
			OnPropertyChanged("ArtistLabel");
			OnPropertyChanged("BPMLabel");
			OnPropertyChanged("CurrentColor");
		}

		//Eventhandling

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

	}
}
