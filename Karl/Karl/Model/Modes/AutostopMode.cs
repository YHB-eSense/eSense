using StepDetectionLibrary;
using System;
using System.Diagnostics;
using static Karl.Model.AudioPlayer;
using static Karl.Model.LangManager;
using static StepDetectionLibrary.OutputManager;

namespace Karl.Model
{
	/// <summary>
	/// The Autostop Mode.
	/// </summary>
	public class AutostopMode : Mode, IObserver<Output>
	{
		private IDisposable _stepDetectionDisposable;
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

		public override string Name
		{
			get => SingletonLangManager.CurrentLang.Get("autostop_mode");
		}

		protected override bool Activate()
		{
			_autostopped = false;
			_stepDetectionDisposable = SingletonOutputManager.Subscribe(this);
			return true;
		}

		protected override bool Deactivate()
		{
			_autostopped = false;
			if (_stepDetectionDisposable != null)
			{
				_stepDetectionDisposable.Dispose();
				_stepDetectionDisposable = null;
			}
			return true;
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
