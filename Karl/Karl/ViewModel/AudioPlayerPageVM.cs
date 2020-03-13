using Karl.Model;
using System;
using System.ComponentModel;
using System.Timers;
using System.Windows.Input;
using Xamarin.Forms;

namespace Karl.ViewModel
{
	public class AudioPlayerPageVM : INotifyPropertyChanged
	{
		protected SettingsHandler _settingsHandler;
		protected AudioPlayer _audioPlayer;
		private ImageSource _iconPlay;
		private ImageSource _iconPause;
		protected Timer _timer;
		protected double _dragValue;
		protected bool _wasPaused;

		//Eventhandling
		public event PropertyChangedEventHandler PropertyChanged;

		//Properties binded to AudioPlayerPage of View
		public CustomColor CurrentColor { get => _settingsHandler.CurrentColor; }
		public AudioTrack AudioTrack { get => _audioPlayer.CurrentTrack; }
		public double CurrentPosition
		{
			get
			{
				if (AudioTrack == null) { return 0; }
				return _audioPlayer.CurrentSecInTrack / AudioTrack.Duration;
			}
			set => _dragValue = value;
		}
		public ImageSource Icon
		{
			get
			{
				if (_audioPlayer.Paused) { return _iconPlay; }
				return _iconPause;
			}
		}
		public string TimePlayed
		{
			get
			{
				if (AudioTrack == null) { return "-:--"; }
				return string.Format("{0}:{1:00}", 
				(int)TimeSpan.FromSeconds(_audioPlayer.CurrentSecInTrack).TotalMinutes,
				TimeSpan.FromSeconds(_audioPlayer.CurrentSecInTrack).Seconds);
			}
		}
		public string TimeLeft
		{
			get
			{
				if (AudioTrack == null) { return "-:--"; }
				return string.Format("{0}:{1:00}",
				(int)TimeSpan.FromSeconds(AudioTrack.Duration - _audioPlayer.CurrentSecInTrack).TotalMinutes,
				TimeSpan.FromSeconds(AudioTrack.Duration - _audioPlayer.CurrentSecInTrack).Seconds);
			}
		}
		public ImageSource Cover
		{
			get
			{
				if (AudioTrack == null || AudioTrack.Cover == null) { return ImageSource.FromResource("Karl.Resources.Images.art.png"); }
				return ImageSource.FromStream(() => new System.IO.MemoryStream(AudioTrack.Cover));
			}
		}
		public double Volume
		{
			get => AudioPlayer.SingletonAudioPlayer.Volume;
			set => AudioPlayer.SingletonAudioPlayer.Volume = value;
		}
		public virtual bool UsingBasicAudio { get => _settingsHandler.UsingBasicAudio; }

		//Commands binded to AudioPlayerPage of View
		public ICommand PausePlayCommand { get; }
		public ICommand PlayPrevCommand { get; }
		public ICommand PlayNextCommand { get; }
		public ICommand PositionDragStartedCommand { get; }
		public ICommand PositionDragCompletedCommand { get; }

		/// <summary>
		/// Initializises Commands, Images and AudioPlayer of Model
		/// </summary>
		public AudioPlayerPageVM()
		{
			PausePlayCommand = new Command(PausePlay);
			PlayPrevCommand = new Command(PlayPrev);
			PlayNextCommand = new Command(PlayNext);
			PositionDragStartedCommand = new Command(PositionDragStarted);
			PositionDragCompletedCommand = new Command(PositionDragCompleted);
			_iconPlay = ImageSource.FromResource("Karl.Resources.Images.play.png");
			_iconPause = ImageSource.FromResource("Karl.Resources.Images.pause.png");
			_timer = new Timer();
			_timer.Interval = 100;
			_timer.Elapsed += new ElapsedEventHandler(Tick);
			_timer.AutoReset = true;
			_wasPaused = true;
			InitializeSingletons();
			_settingsHandler = SettingsHandler.SingletonSettingsHandler;
			_settingsHandler.ColorChanged += RefreshColor;
			_settingsHandler.AudioModuleChanged += RefreshAudio;
		}

		protected virtual void InitializeSingletons()
		{
			_audioPlayer = AudioPlayer.SingletonAudioPlayer;
			_audioPlayer.AudioChanged += RefreshAudio;
		}

		private void RefreshColor(object sender, EventArgs args)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentColor)));
		}

		public void RefreshAudio(object sender, EventArgs args)
		{
			if (!_audioPlayer.Paused) { _timer.Start(); }
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Icon)));
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AudioTrack)));
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentPosition)));
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TimePlayed)));
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TimeLeft)));
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Cover)));
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(UsingBasicAudio)));
		}

		private void PausePlay()
		{
			if (AudioTrack == null) { return; }
			_audioPlayer.TogglePause();
			if (_audioPlayer.Paused) { _timer.Stop(); }
			else { _timer.Start(); }
			if (!UsingBasicAudio)
			{
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AudioTrack)));
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Cover)));
			}
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Icon)));
		}

		private void PlayPrev()
		{
			if (AudioTrack == null) { return; }
			_audioPlayer.PrevTrack();
		}

		private void PlayNext()
		{
			if (AudioTrack == null) { return; }
			_audioPlayer.NextTrack();
		}

		private void PositionDragStarted()
		{
			if (AudioTrack == null) { return; }
			if (!_audioPlayer.Paused)
			{
				PausePlay();
				_wasPaused = false;
			}
			else { _wasPaused = true; }
		}

		private void PositionDragCompleted()
		{
			if (AudioTrack == null) { return; }
			if (!_wasPaused) { PausePlay(); }
			CurrentPosition = _dragValue * AudioTrack.Duration;
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentPosition)));
		}

		private void Tick(object sender, EventArgs e)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentPosition)));
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TimePlayed)));
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TimeLeft)));
		}
	}
}
