using System;
using System.Collections.Generic;
using System.Text;

namespace Karl.Model
{
	public enum Lang
	{
		//todo


	}

	class LangManager : IObservable<Lang>
	{
		public static LangManager SingletonLangManager
		{
			get
			{
				if (SingletonLangManager == null)
				{
					SingletonLangManager = new LangManager();
					return SingletonLangManager;
				}
				else
				{
					return SingletonLangManager;
				}
			}
			private set => SingletonLangManager = value;
		}

		private LangManager()
		{
			//todo
		}


		//todo https://docs.microsoft.com/en-us/dotnet/api/system.iobservable-1?view=netframework-4.8
		public IDisposable Subscribe(IObserver<Lang> observer)
		{
			//todo
			throw new NotImplementedException();
		}

		private class Unsubscriber : IDisposable
		{
			//todo
			public void Dispose()
			{
				//todo
				throw new NotImplementedException();
			}
		}
	}

	interface ILangObserver : IObserver<Lang>
	{
		//todo
	}

	public struct Language 
	{
		public String Name { get; }
		internal String Tag { get; }

	}

}
