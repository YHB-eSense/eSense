using System;
using System.Collections.Generic;
using System.Text;

namespace Karl.Model
{
	/// <summary>
	/// A general Mode.
	/// </summary>
	public abstract class Mode
	{
		/// <summary>
		/// The name of the mode.
		/// </summary>
		public abstract string Name { get; }
		/// <summary>
		/// The constructor registers this for name changes.
		/// </summary>
		protected Mode()
		{
			LangManager.SingletonLangManager.Subscribe(new LangObserver(this));//todo
			//Name = UpdateName(LangManager.SingletonLangManager.CurrentLang);
		}
		/// <summary>
		/// Activate this mode.
		/// </summary>
		public abstract void Activate();
		/// <summary>
		/// Deactivate this mode.
		/// </summary>
		public abstract void Deactivate();
		/// <summary>
		/// Returns the Name that you can take from lang.
		/// </summary>
		/// <param name="value">The current Language data</param>
		/// <returns>The name of the Mode.</returns>
		protected abstract string UpdateName(Lang value);
		//todo

		private class LangObserver : IObserver<Lang>
		{
			Mode parent;
			public LangObserver(Mode parent)
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

			public void OnNext(Lang value)
			{
				//parent.Name = parent.UpdateName(value); //todo
			}
		}
	}
}
