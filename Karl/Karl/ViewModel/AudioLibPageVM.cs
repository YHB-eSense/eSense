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
	public class AudioLibPageVM
	{
		private NavigationHandler _handler;
		private AudioLib _audioLib;
		private AudioPlayer _audioPlayer;

		/**
		 Properties binded to AudioLibPage of View
		**/
		public ObservableCollection<AudioTrack> Songs { get; set; }

		/**
		 Commands binded to AudioLibPage of View
		**/
		public ICommand TitleSortCommand { get; }
		public ICommand ArtistSortCommand { get; }
		public ICommand BPMSortCommand { get; }
		public ICommand PlaySongCommand { get; }
		public ICommand AddSongCommand { get; }
		public ICommand SearchSongCommand { get; }

		/// <summary>
		/// Initializises Commands, NavigationHandler and AudioLib, AudioPlayer of Model
		/// </summary>
		/// <param name="handler"> For navigation</param>
		public AudioLibPageVM(NavigationHandler handler)
		{
			_handler = handler;
			_audioLib = AudioLib.SingletonAudioLib;
			_audioPlayer = AudioPlayer.SingletonAudioPlayer;
			Songs = new ObservableCollection<AudioTrack>();
			TitleSortCommand = new Command(TitleSort);
			ArtistSortCommand = new Command(ArtistSort);
			BPMSortCommand = new Command(BPMSort);
			PlaySongCommand = new Command<AudioTrack>(PlaySong);
			AddSongCommand = new Command(AddSong);
			SearchSongCommand = new Command<string>(SearchSong);
		}

		/// <summary>
		/// Sorts Titles by Name
		/// </summary>
		private void TitleSort()
		{
			ObservableCollection<AudioTrack> songs = new ObservableCollection<AudioTrack>();
			//sort
			Songs = songs;
		}

		/// <summary>
		/// Sorts Titles by Artist
		/// </summary>
		private void ArtistSort()
		{
			ObservableCollection<AudioTrack> songs = new ObservableCollection<AudioTrack>();
			//sort
			Songs = songs;
		}

		/// <summary>
		/// Sorts Titles by BPM
		/// </summary>
		private void BPMSort()
		{
			ObservableCollection<AudioTrack> songs = new ObservableCollection<AudioTrack>();
			//sort
			Songs = songs;
		}

		/// <summary>
		/// Jumps to AudioPlayer in Model
		/// </summary>
		/// <param name="audioTrack">Name of started song</param>
		private void PlaySong(AudioTrack audioTrack)
		{
			_audioPlayer.CurrentTrack = audioTrack;
			_handler.GotoPage(_handler._pages[0]);
			_audioPlayer.PlayTrack();
		}

		/// <summary>
		/// Navigates to AddSongPage
		/// </summary>
		private void AddSong()
		{
			_handler.GotoPage(_handler._pages[5]);
		}

		/// <summary>
		/// Sets Songs to only contain AudioTracks with title in their Title property
		/// </summary>
		/// <param name="title"></param>
		private void SearchSong(string title)
		{
			Songs = (ObservableCollection<AudioTrack>) Songs.Where(song => song.Title.Contains(title));
			//reset
		}

		/// <summary>
		/// Retrieves Songs from AudioLib in Model
		/// </summary>
		public void GetSongs()
		{
			Songs = (ObservableCollection<AudioTrack>) _audioLib.AudioTracks;
		}

	}
}
