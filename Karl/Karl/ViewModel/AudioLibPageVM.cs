using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using Karl.Model;
using Karl.View;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using SpotifyAPI.Web.Models;
using Xamarin.Forms;

namespace Karl.ViewModel
{
	public class AudioLibPageVM : INotifyPropertyChanged
	{
		private SettingsHandler _settingsHandler;
		private NavigationHandler _navHandler;
		private AudioLib _audioLib;
		private AudioPlayer _audioPlayer;
		private ObservableCollection<AudioTrack> _oldSongs;
		private List<AudioTrack> _deleteList;
		private Color _titleSortColor;
		private Color _titleSortTextColor;
		private Color _artistSortColor;
		private Color _artistSortTextColor;
		private Color _bpmSortColor;
		private Color _bpmSortTextColor;
		private enum _sortType { TITLESORT, ARTISTSORT, BPMSORT}
		private _sortType type;

		//Eventhandling
		public event PropertyChangedEventHandler PropertyChanged;

		//Properties binded to AudioLibPage of View
		public CustomColor CurrentColor { get => _settingsHandler.CurrentColor; }
		public string TitleLabel { get => _settingsHandler.CurrentLang.Get("title"); }
		public string ArtistLabel { get => _settingsHandler.CurrentLang.Get("artist"); }
		public string BPMLabel { get => _settingsHandler.CurrentLang.Get("bpm"); }
		public string PlaylistsLabel { get => _settingsHandler.CurrentLang.Get("playlists"); }
		public SimplePlaylist[] Playlists { get => _audioLib.Playlists; }
		public SimplePlaylist SelectedPlaylist { get => _audioLib.SelectedPlaylist; set => _audioLib.SelectedPlaylist = value; }
		public ObservableCollection<AudioTrack> Songs
		{
			get => _audioLib.AudioTracks;
			set
			{
				_audioLib.AudioTracks = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Songs)));
			}
		}
		public Color TitleSortColor {
			get => _titleSortColor;
			set
			{
				_titleSortColor = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TitleSortColor)));
			}
		}
		public Color TitleSortTextColor
		{
			get => _titleSortTextColor;
			set
			{
				_titleSortTextColor = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TitleSortTextColor)));
			}
		}
		public Color ArtistSortColor
		{
			get => _artistSortColor;
			set
			{
				_artistSortColor = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ArtistSortColor)));
			}
		}
		public Color ArtistSortTextColor
		{
			get => _artistSortTextColor;
			set
			{
				_artistSortTextColor = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ArtistSortTextColor)));
			}
		}
		public Color BPMSortColor
		{
			get => _bpmSortColor;
			set
			{
				_bpmSortColor = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(BPMSortColor)));
			}
		}
		public Color BPMSortTextColor
		{
			get => _bpmSortTextColor;
			set
			{
				_bpmSortTextColor = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(BPMSortTextColor)));
			}
		}
		public bool UsingBasicAudio { get => UsingBasicAudio; }
		public bool UsingSpotifyAudio { get => UsingSpotifyAudio; }

		// Commands binded to AudioLibPage of View
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
			_navHandler = handler;
			_settingsHandler = SettingsHandler.SingletonSettingsHandler;
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
			_settingsHandler.LangChanged += RefreshLang;
			_settingsHandler.ColorChanged += RefreshColor;
			TitleSort();
		
		}

		private void RefreshLang(object sender, EventArgs args)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TitleLabel)));
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ArtistLabel)));
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(BPMLabel)));
		}

		private void RefreshColor(object sender, EventArgs args)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentColor)));
			switch (type)
			{
				case _sortType.TITLESORT: TitleSort(); break;
				case _sortType.ARTISTSORT: ArtistSort(); break;
				case _sortType.BPMSORT: BPMSort(); break;
			}
		}

		private void TitleSort()
		{
			if (Songs != null) { Songs = new ObservableCollection<AudioTrack>(Songs.OrderBy(s => s.Title)); }
			TitleSortColor = CurrentColor.Color;
			ArtistSortColor = Color.Transparent;
			BPMSortColor = Color.Transparent;
			TitleSortTextColor = Color.White;
			ArtistSortTextColor = Color.Black;
			BPMSortTextColor = Color.Black;
			type = _sortType.TITLESORT;
		}

		private void ArtistSort()
		{
			if (Songs != null) { Songs = new ObservableCollection<AudioTrack>(Songs.OrderBy(s => s.Artist)); }
			TitleSortColor = Color.Transparent;
			ArtistSortColor = CurrentColor.Color;
			BPMSortColor = Color.Transparent;
			TitleSortTextColor = Color.Black;
			ArtistSortTextColor = Color.White;
			BPMSortTextColor = Color.Black;
			type = _sortType.ARTISTSORT;
		}

		private void BPMSort()
		{
			if (Songs != null) { Songs = new ObservableCollection<AudioTrack>(Songs.OrderBy(s => s.BPM)); }
			TitleSortColor = Color.Transparent;
			ArtistSortColor = Color.Transparent;
			BPMSortColor = CurrentColor.Color;
			TitleSortTextColor = Color.Black;
			ArtistSortTextColor = Color.Black;
			BPMSortTextColor = Color.White;
			type = _sortType.BPMSORT;
		}

		private void PlaySong(AudioTrack track)
		{
			_navHandler.GotoPage<AudioPlayerPage>();
			if (track != _audioPlayer.CurrentTrack) { _audioPlayer.PlayTrack(track); }	
		}

		private void AddSong()
		{
			_navHandler.GotoPage<AddSongPage>();
		}

		private void EditDeleteList(AudioTrack song)
		{
			if (_deleteList.Contains(song)) { _deleteList.Remove(song); }
			else { _deleteList.Add(song); }
		}

		private void SearchSong(string value)
		{
			if (_oldSongs == null){ _oldSongs = new ObservableCollection<AudioTrack>(Songs); }
			if (value == null || value == "")
			{
				Songs = new ObservableCollection<AudioTrack>(_oldSongs);
				_oldSongs = null;
			}
			else { Songs = new ObservableCollection<AudioTrack>(_oldSongs.Where(song =>
				song.Title.ToLower().Contains(value.ToLower()) ||
				song.Artist.ToLower().Contains(value.ToLower()))); }
		}

		private async void DeleteSongs()
		{
			bool answer = await Application.Current.MainPage.DisplayAlert(_settingsHandler.CurrentLang.Get("question_title"),
					_settingsHandler.CurrentLang.Get("question_text"), _settingsHandler.CurrentLang.Get("question_yes"),
					_settingsHandler.CurrentLang.Get("question_no"));
			if (answer) { foreach(AudioTrack song in _deleteList) { _audioLib.DeleteTrack(song); }}
		}

	}
}
