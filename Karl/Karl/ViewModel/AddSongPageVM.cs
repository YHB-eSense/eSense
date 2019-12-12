using Karl.Model;
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
		public ICommand AddSongCommand { get; }

		public AddSongPageVM(AppLogic appLogic)
		{
			AppLogic = appLogic;
			AddSongCommand = new Command<string>(AddSong);
		}

		private void AddSong(string title /*,string artist, string bpm*/)
		{
			//AppLogic
			NavigationHandler.GoBack();
		}
	}
}
