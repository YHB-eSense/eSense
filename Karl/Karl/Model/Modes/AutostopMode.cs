using StepDetectionLibrary;
using System;
using static Karl.Model.AudioPlayer;
using static Karl.Model.LangManager;
using static StepDetectionLibrary.OutputManager;

namespace Karl.Model
{
	/// <summary>
	/// The Autostop Mode.
	/// </summary>
	class AutostopMode : IMode, IObserver<Output>
	{
		private IDisposable StepDetectionDisposable;
		private bool _autostopped;
		public bool Autostopped
		{
			get => _autostopped;
			set
			{
				if (value == _autostopped) return;
				_autostopped = value;
				if (_autostopped == true)
				{
					if (!SingletonAudioPlayer.Paused) SingletonAudioPlayer.TogglePause();
				}
				else
				{
					if (SingletonAudioPlayer.Paused) SingletonAudioPlayer.TogglePause();
				}
			}
		}

		public void Activate()
		{
			_autostopped = false;
			StepDetectionDisposable = SingletonOutputManager.Subscribe(this);
		}

		public void Deactivate()
		{
			StepDetectionDisposable.Dispose();
			_autostopped = false;
		}

		public string Name
		{
			get => SingletonLangManager.CurrentLang.Get("autostop_mode");
		}

		public void OnNext(Output value)
		{
			Autostopped = value.Frequency == 0;
		}

		public void OnCompleted()
		{
			throw new NotImplementedException();
		}

		public void OnError(Exception error)
		{
			throw new NotImplementedException();
		}
	}
}
