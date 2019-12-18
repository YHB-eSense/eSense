using System;
using System.Collections.Generic;
using System.Text;

namespace Karl.Model
{
	/// <summary>
	/// A class that stores all important Strings for Lang data
	/// </summary>
	public class Lang
	{

		public String get(String tag)
		{
			//todo
			return tag;
		}
		
		//todo

	}
	/// <summary>
	/// This Singleton stores loads and changes the current Language.
	/// It is based on a push Observer Pattern. And sends a notification if the language is changed.
	/// </summary>
	public class LangManager : IObservable<Lang>
	{
		private static LangManager _singletonLangManager;
		/// <summary>
		/// The singleton object of LangManager.
		/// </summary>
		public static LangManager SingletonLangManager
		{
			get
			{
				if (_singletonLangManager == null)
				{
					_singletonLangManager = new LangManager();
					return _singletonLangManager;
				}
				else
				{
					return _singletonLangManager;
				}
			}
			private set => _singletonLangManager = value;
		}

		private LangManager()
		{
			CurrentLang = new Lang();//todo
		}

		/// <summary>
		/// The Language obj. currently selected.
		/// </summary>
		public Lang CurrentLang { get; private set; }

		//todo https://docs.microsoft.com/en-us/dotnet/api/system.iobservable-1?view=netframework-4.8
		/// <summary>
		/// Usual Subscribe method.
		/// </summary>
		/// <param name="observer">The Observer you want to register.</param>
		/// <returns>The IDisposable for Unsubscribing.</returns>
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
	/// <summary>
	/// A general struct to indentify a language.
	/// </summary>
	public struct Language 
	{
		public String Name { get; }
		internal String Tag { get; }

	}

}
