using System;
using System.Collections.Generic;
using System.Text;

namespace Karl.Model
{
	public abstract class Mode
	{
		public String Name { get; protected set; }
		protected Mode()
		{
			LangManager.SingletonLangManager.Subscribe(new LangObserver(this));//todo
		}
		public abstract void Activate();
		public abstract void Deactivate();
		protected abstract String UpdateName(Lang value);
		//todo

		private class LangObserver : ILangObserver
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
				parent.Name = parent.UpdateName(value); //todo
			}
		}
	}
}
