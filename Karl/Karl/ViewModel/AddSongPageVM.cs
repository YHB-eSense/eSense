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
		private AppLogic _appLogic;
		public string NewSongTitle { get; set; }
		public string NewSongArtist { get; set; }
		public string NewSongBPM { get; set; }
		public string NewSongFileLocation { get; set; }
		public ICommand AddSongCommand { get; }
		public ICommand PickFileCommand { get; }

		public AddSongPageVM(AppLogic appLogic)
		{
			_appLogic = appLogic;
			AddSongCommand = new Command(AddSong);
			PickFileCommand = new Command(PickFile);
		}

		private void AddSong()
		{
			//AudioTrack newSong = new AudioTrack(NewSongTitle, NewSongArtist, Convert.ToInt32(NewSongBPM), NewSongFileLocation);
			//AppLogic
			NavigationHandler.GoBack();
		}

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
