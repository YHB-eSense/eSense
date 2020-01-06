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
		private NavigationHandler _handler;
		private AudioLib _audioLib;
		private File _file;

		/**
		 Properties binded to AddSongsPage of View
		**/
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
			_audioLib = AudioLib.SingletonAudioLib;
			AddSongCommand = new Command(AddSong);
			PickFileCommand = new Command(PickFile);
		}

		/// <summary>
		/// Adds song to AudioLib of Model
		/// </summary>
		private async void AddSong()
		{
			if (NewSongTitle == null || NewSongTitle == "" || NewSongArtist == null
				|| NewSongArtist == "" || NewSongBPM == null || NewSongBPM == "")
			{
				await Application.Current.MainPage.DisplayAlert("Alert!", "You need to assign a value to every entry!", "OK");
				return;
			}
			_audioLib.AddTrack(NewSongFileLocation, NewSongTitle, NewSongArtist, Convert.ToInt32(NewSongBPM));
			_handler.GoBack();
		}

		/// <summary>
		/// Opens a file picker
		/// </summary>
		private async void PickFile()
		{
			var file = await CrossFilePicker.Current.PickFile();
			if (file != null)
			{
				NewSongFileLocation = file.FilePath;
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
			if (_file != null && _file.Tag.Title != null)
			{
				return _file.Tag.Title;
			}
			return "Unknown Title";
		}

		private string GetArtist()
		{
			if (_file != null && _file.Tag.AlbumArtists.Length >= 1)
			{
				return _file.Tag.AlbumArtists[0];
			}
			return "Unknown Artist";
		}

		private int GetBPM()
		{

			if (_file != null && _file.Tag.BeatsPerMinute != 0)
			{
				return (int)_file.Tag.BeatsPerMinute;
			}
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
