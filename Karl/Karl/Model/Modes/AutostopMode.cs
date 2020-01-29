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
	public class AutostopMode : IMode, IObserver<Output>
	{
		private IDisposable StepDetectionDisposable;
		private bool _autostopped;
		private bool _activated;
		public bool Autostopped
		{
			get => _autostopped;
			set
			{
				if (value == _autostopped) return;
				_autostopped = value;
				Debug.WriteLine("Performing auto-{0}", args: _autostopped ? "stop" : "resume");
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

		public bool Activated
		{
			get => _activated;
			set
			{
				if (_activated) Deactivate();
				else Activate();
			}
		}

		public void Activate()
		{
			Debug.WriteLine("Activating mode '{0}'", args: Name);
			_autostopped = false;
			StepDetectionDisposable = SingletonOutputManager.Subscribe(this);
			_activated = true;
		}

		public void Deactivate()
		{
			Debug.WriteLine("Deactivating mode '{0}'", args: Name);
			_autostopped = false;
			StepDetectionDisposable.Dispose();
			_activated = false;
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
