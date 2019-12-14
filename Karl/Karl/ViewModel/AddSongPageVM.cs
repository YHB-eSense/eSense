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
		private AppLogic AppLogic;
		public string NewSongTitle { get; set; }
		public string NewSongArtist { get; set; }
		public string NewSongBPM { get; set; }
		public string NewSongFileLocation { get; set; }

		/**
		 Commands were called from Elements in AudioLibPage
		**/
		public ICommand AddSongCommand { get; }
		public ICommand PickFileCommand { get; }


		/// <summary>
		/// Initializises App Logic and all available Commands
		/// </summary>
		/// <param name="appLogic"> For needed functions in Model</param>
		public AddSongPageVM(AppLogic appLogic)
		{
			AppLogic = appLogic;
			AddSongCommand = new Command(AddSong);
			PickFileCommand = new Command(PickFile);
		}

		/// <summary>
		/// Adds song to AudioLib through App Logic
		/// </summary>
		private void AddSong()
		{
			//AudioTrack newSong = new AudioTrack(NewSongTitle, NewSongArtist, Convert.ToInt32(NewSongBPM), NewSongFileLocation);
			//AppLogic
			NavigationHandler.GoBack();
		}

		/// <summary>
		/// Opens File Choosing Dialog
		/// </summary>
		private async void PickFile()
		{
			var file = await CrossFilePicker.Current.PickFile();
			if (file != null)
			{
				NewSongFileLocation = file.FileName;
			}
		}
	}
}
