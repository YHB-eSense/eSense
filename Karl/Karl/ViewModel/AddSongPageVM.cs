using Karl.Model;
using Plugin.FilePicker;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Karl.ViewModel
{
	public class AddSongPageVM
	{
		private NavigationHandler _handler;
		private AudioLib _audioLib;

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
		private void AddSong()
		{
			if (NewSongTitle == null && NewSongArtist == null && NewSongBPM == null)
			{
				_audioLib.AddTrack(NewSongFileLocation);
			}
			else if (NewSongArtist == null && NewSongBPM == null)
			{
				_audioLib.AddTrack(NewSongFileLocation, NewSongTitle);
			}
			else
			{
				_audioLib.AddTrack(NewSongFileLocation, NewSongTitle, NewSongArtist, Convert.ToInt32(NewSongBPM));
			}
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
				System.Diagnostics.Debug.WriteLine(NewSongFileLocation);
			}
		}
	}
}
