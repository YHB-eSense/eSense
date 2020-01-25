using StepDetectionLibrary;
using System;
using System.Collections.Generic;
using System.Text;

namespace Karl.Model
{
	/// <summary>
	/// The Autostop Mode.
	/// </summary>
	class AutostopMode : Mode
	{
		private AudioPlayer _audioPlayer = AudioPlayer.SingletonAudioPlayer;
		private LangManager _langManager = LangManager.SingletonLangManager;
		private IDisposable StepDetectionDisposable;
		private bool _autostopped;
		bool Autostopped
		{
			get => _autostopped;
			set
			{
				if (value)
				{
					if (!_audioPlayer.Paused) _audioPlayer.TogglePause();
				}
				else
				{
					if (_audioPlayer.Paused) _audioPlayer.TogglePause();
				}
				_autostopped = value;
			}
		}
		public AutostopMode()
		{
			//todo
		}

		public override void Activate()
		{
			_autostopped = false;
			StepDetectionDisposable = OutputManager.SingletonOutputManager.Subscribe(new StepDetectionObserver(this));
			//throw new NotImplementedException();//todo
		}

		public override void Deactivate()
		{
			StepDetectionDisposable.Dispose();
			_autostopped = false;
			//throw new NotImplementedException();//todo
		}

		protected override String UpdateName(Lang value)
		{
			return "AutoStopMode"; //value.get("mode_autostop");//todo
		}

		public override string Name
		{
			get => _langManager.CurrentLang.Get("autostop_mode");
		}

		private class StepDetectionObserver : IObserver<Output>
		{
			AutostopMode parent;
			public StepDetectionObserver(AutostopMode parent)
			{
				this.parent = parent;
			}
			public void OnCompleted()
			{
				throw new NotImplementedException(); //todo
			}

			public void OnError(Exception error)
			{
				throw new NotImplementedException(); //todo
			}

			public void OnNext(Output value)
			{
				if (value.Frequency == 0)
				{
					if (!parent.Autostopped) parent.Autostopped = true;
				}
				else if (parent.Autostopped) parent.Autostopped = false;
			}
		}
	} 
}
