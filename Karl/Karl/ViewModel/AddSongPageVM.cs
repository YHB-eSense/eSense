using Karl.Model;
using Plugin.FilePicker;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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
		private NavigationHandler _navHandler;
		private AudioLib _audioLib;
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
		public string TitleLabel { get => _settingsHandler.CurrentLang.Get("title"); }
		public string ArtistLabel { get => _settingsHandler.CurrentLang.Get("artist"); }
		public string BPMLabel { get => _settingsHandler.CurrentLang.Get("bpm"); }
		public string PickFileLabel { get => _settingsHandler.CurrentLang.Get("pick_file"); }
		public string AddSongLabel { get => _settingsHandler.CurrentLang.Get("add_song"); }
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
		/// <param name="navHandler"> For navigation</param>
		public AddSongPageVM(NavigationHandler navHandler)
		{
			_navHandler = navHandler;
			_settingsHandler = SettingsHandler.SingletonSettingsHandler;
			_audioLib = AudioLib.SingletonAudioLib;
			AddSongCommand = new Command(AddSong);
			PickFileCommand = new Command(PickFile);
			GetBPMCommand = new Command(CalculateBPM);
			_settingsHandler.LangChanged += RefreshLang;
			_settingsHandler.ColorChanged += RefreshColor;
		}

		private void RefreshLang(object sender, EventArgs args)
		{	
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TitleLabel)));
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ArtistLabel)));
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(BPMLabel)));
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PickFileLabel)));
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AddSongLabel)));
		}

		private void RefreshColor(object sender, EventArgs args)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentColor)));
		}

		private async void AddSong()
		{
			int bpm;
			if (NewSongTitle == null || NewSongTitle == "" || NewSongArtist == null
				|| NewSongArtist == "" || NewSongBPM == null || NewSongBPM == "" || !_picked || !int.TryParse(NewSongBPM, out bpm))
			{
				await Application.Current.MainPage.DisplayAlert(_settingsHandler.CurrentLang.Get("alert_title"),
					_settingsHandler.CurrentLang.Get("alert_text"), _settingsHandler.CurrentLang.Get("alert_ok"));
				return;
			}
			_audioLib.AddTrack(_newSongFileLocation, NewSongTitle, NewSongArtist, bpm);
			_navHandler.GoBack();
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
				_newSongFileLocation = pick.FilePath;
				Debug.WriteLine("Audio file picked: {0}", args: _newSongFileLocation);
				_file = TagLib.File.Create(_newSongFileLocation);
				_picked = true;
				NewSongTitle = GetTitle();
				NewSongArtist = GetArtist();
				NewSongBPM = GetBPM();
			}
		}

		private async void CalculateBPM()
		{
			if (!_picked)
			{
				await Application.Current.MainPage.DisplayAlert(_settingsHandler.CurrentLang.Get("alert_title"),
					_settingsHandler.CurrentLang.Get("alert_text_2"), _settingsHandler.CurrentLang.Get("alert_ok"));
				return;
			}
			if (Path.GetExtension(_newSongFileLocation).Equals(".wav"))
			{
				BPMCalculator calculator = new BPMCalculator(_newSongFileLocation);
				NewSongBPM = calculator.Calculate().ToString();
			}
			else
			{
				await Application.Current.MainPage.DisplayAlert(_settingsHandler.CurrentLang.Get("alert_title"),
				"alert_text_3", _settingsHandler.CurrentLang.Get("alert_ok"));
			}
		}

		private string GetTitle()
		{
			if (_file != null && _file.Tag.Title != null) { return _file.Tag.Title; }
			return _settingsHandler.CurrentLang.Get("unknown");
		}

		private string GetArtist()
		{
			if (_file != null && _file.Tag.Performers.Length >= 1){ return _file.Tag.Performers[0]; }
			return _settingsHandler.CurrentLang.Get("unknown");
		}

		private string GetBPM()
		{
			if (_file != null && _file.Tag.BeatsPerMinute != 0) { return Convert.ToString(_file.Tag.BeatsPerMinute); }
			return _settingsHandler.CurrentLang.Get("unknown");
		}

	}
}
