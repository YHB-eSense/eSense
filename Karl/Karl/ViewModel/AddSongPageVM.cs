using Karl.Model;
using Plugin.FilePicker;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
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
		private TagLib.File _file;
		private bool _picked;
		private string _newSongTitle;
		private string _newSongArtist;
		private string _newSongBPM;
		private string _newSongFileLocation;

		//Eventhandling
		public event PropertyChangedEventHandler PropertyChanged;

		//Properties binded to AddSongsPage of View
		public CustomColor CurrentColor { get => _settingsHandler.CurrentColor; }
		public string TitleLabel { get => _langManager.CurrentLang.Get("title"); }
		public string ArtistLabel { get => _langManager.CurrentLang.Get("artist"); }
		public string BPMLabel { get => _langManager.CurrentLang.Get("bpm"); }
		public string PickFileLabel { get => _langManager.CurrentLang.Get("pick_file"); }
		public string AddSongLabel { get => _langManager.CurrentLang.Get("add_song"); }
		public string NewSongTitle
		{
			get => _newSongTitle;
			set
			{
				_newSongTitle = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(NewSongTitle)));
			}
		}
		public string NewSongArtist
		{
			get => _newSongArtist;
			set
			{
				_newSongArtist = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(NewSongArtist)));
			}
		}
		public string NewSongBPM
		{
			get => _newSongBPM;
			set
			{
				_newSongBPM = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(NewSongBPM)));
			}
		}

		//Commands binded to AddSongsPage of View
		public ICommand AddSongCommand { get; }
		public ICommand PickFileCommand { get; }
		public ICommand GetBPMCommand { get; }

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
			_settingsHandler.SettingsChanged += Refresh;
			GetBPMCommand = new Command(DetBPM);
		}

		private async void DetBPM()
		{
			if (Path.GetExtension(_newSongFileLocation).Equals("wav"))
			{
				NewSongBPM = ((int)BPMDectetorWave.DetectBPM(_newSongFileLocation)).ToString();

			}
			else if (Path.GetExtension(_newSongFileLocation).Equals("mp3"))
			{
					
			}
			else {
				await Application.Current.MainPage.DisplayAlert(_langManager.CurrentLang.Get("alert_title"),
				"Not a .wav file", _langManager.CurrentLang.Get("alert_ok"));
			}
		}

		private void Refresh(object sender, EventArgs args)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TitleLabel)));
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ArtistLabel)));
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(BPMLabel)));
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PickFileLabel)));
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AddSongLabel)));
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentColor)));
		}

		private async void AddSong()
		{
			int bpm;
			if (NewSongTitle == null || NewSongTitle == "" || NewSongArtist == null
				|| NewSongArtist == "" || NewSongBPM == null || NewSongBPM == "" || !_picked || !int.TryParse(NewSongBPM, out bpm))
			{
				await Application.Current.MainPage.DisplayAlert(_langManager.CurrentLang.Get("alert_title"),
					_langManager.CurrentLang.Get("alert_text"), _langManager.CurrentLang.Get("alert_ok"));
				return;
			}
			_audioLib.AddTrack(_newSongFileLocation, NewSongTitle, NewSongArtist, bpm);
			_handler.GoBack();
			NewSongTitle = null;
			NewSongArtist = null;
			NewSongBPM = null;
			_newSongFileLocation = null;
			_picked = false;
		}

		private async void PickFile()
		{
			var pick = await CrossFilePicker.Current.PickFile();
			if (pick != null)
			{
				_picked = true;
				_newSongFileLocation = pick.FilePath;
				_file = TagLib.File.Create(_newSongFileLocation);
				NewSongTitle = GetTitle();
				NewSongArtist = GetArtist();
				NewSongBPM = GetBPM();
			}
		}

		private string GetTitle()
		{
			if (_file != null && _file.Tag.Title != null) { return _file.Tag.Title; }
			return _langManager.CurrentLang.Get("unknown");
		}

		private string GetArtist()
		{
			if (_file != null && _file.Tag.Performers.Length >= 1){ return _file.Tag.Performers[0]; }
			return _langManager.CurrentLang.Get("unknown");
		}

		private string GetBPM()
		{
			if (_file != null && _file.Tag.BeatsPerMinute != 0) { return Convert.ToString(_file.Tag.BeatsPerMinute); }
			return _langManager.CurrentLang.Get("unknown");
		}

	}
}
