using Karl.Model;
using Plugin.FilePicker;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using TagLib;
using Xamarin.Forms;

namespace Karl.ViewModel
{
	public class AddSongPageVM : INotifyPropertyChanged
	{
		private SettingsHandler _settingsHandler;
		private NavigationHandler _handler;
		private AudioLib _audioLib;
		private LangManager _langManager;
		private File _file;
		private bool _picked;

		/**
		 Properties binded to AddSongsPage of View
		**/
		public CustomColor CurrentColor { get => _settingsHandler.CurrentColor; }
		public string TitleLabel { get => _langManager.CurrentLang.Get("title"); }
		public string ArtistLabel { get => _langManager.CurrentLang.Get("artist"); }
		public string BPMLabel { get => _langManager.CurrentLang.Get("bpm"); }
		public string PickFileLabel { get => _langManager.CurrentLang.Get("pick_file"); }
		public string AddSongLabel { get => _langManager.CurrentLang.Get("add_song"); }
		public string NewSongTitle { get; set; }
		public string NewSongArtist { get; set; }
		public string NewSongBPM { get; set; }
		public string NewSongFileLocation { get; set; }

		/**
		 Commands binded to AddSongsPage of View
		**/
		public ICommand AddSongCommand { get; }
		public ICommand PickFileCommand { get; }


		/// <summary>
		/// Initializises Commands, NavigationHandler and AudioLib of Model
		/// </summary>
		/// <param name="handler"> For navigation</param>
		public AddSongPageVM(NavigationHandler handler)
		{
			_handler = handler;
			_settingsHandler = SettingsHandler.SingletonSettingsHandler;
			_audioLib = AudioLib.SingletonAudioLib;
			_langManager = LangManager.SingletonLangManager;
			AddSongCommand = new Command(AddSong);
			PickFileCommand = new Command(PickFile);
		}

		public void RefreshPage()
		{
			OnPropertyChanged("TitleLabel");
			OnPropertyChanged("ArtistLabel");
			OnPropertyChanged("BPMLabel");
			OnPropertyChanged("PickFileLabel");
			OnPropertyChanged("AddSongLabel");
			OnPropertyChanged("CurrentColor");
		}

		/// <summary>
		/// Adds song to AudioLib of Model
		/// </summary>
		private async void AddSong()
		{
			if (NewSongTitle == null || NewSongTitle == "" || NewSongArtist == null
				|| NewSongArtist == "" || NewSongBPM == null || NewSongBPM == "" || !_picked)
			{
				await Application.Current.MainPage.DisplayAlert(_langManager.CurrentLang.Get("alert_title"),
					_langManager.CurrentLang.Get("alert_text"), _langManager.CurrentLang.Get("alert_ok"));
				return;
			}
			_audioLib.AddTrack(NewSongFileLocation, NewSongTitle, NewSongArtist, Convert.ToInt32(NewSongBPM));
			_handler.GoBack();
			_picked = false;
			NewSongTitle = null;
			NewSongArtist = null;
			NewSongBPM = null;
			NewSongFileLocation = null;
			OnPropertyChanged("NewSongTitle");
			OnPropertyChanged("NewSongArtist");
			OnPropertyChanged("NewSongBPM");
		}

		/// <summary>
		/// Opens a file picker
		/// </summary>
		private async void PickFile()
		{
			var pick = await CrossFilePicker.Current.PickFile();
			if (pick != null)
			{
				_picked = true;
				NewSongFileLocation = pick.FilePath;
				_file = File.Create(NewSongFileLocation);
				NewSongTitle = GetTitle();
				NewSongArtist = GetArtist();
				NewSongBPM = Convert.ToString(GetBPM());
				OnPropertyChanged("NewSongTitle");
				OnPropertyChanged("NewSongArtist");
				OnPropertyChanged("NewSongBPM");
			}
		}

		private string GetTitle()
		{
			if (_file != null && _file.Tag.Title != null) { return _file.Tag.Title; }
			return "Unknown";
		}

		private string GetArtist()
		{
			if (_file != null && _file.Tag.AlbumArtists.Length >= 1){return _file.Tag.AlbumArtists[0]; }
			return "Unknown";
		}

		private int GetBPM()
		{
			if (_file != null && _file.Tag.BeatsPerMinute != 0) { return (int)_file.Tag.BeatsPerMinute; }
			return 0;
		}

		//Eventhandling

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

	}
}
