using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Karl.Model
{
	/// <summary>
	/// A class that stores all important Strings for Lang data
	/// </summary>
	public class Lang
	{
		private const string WORD_NOT_FOUND_MESSAGE = "Word not found";
		private string _filePath;
		private IDictionary <String, String> _words;

		/// <summary>
		/// Sets Path for given Language
		/// </summary>
		/// <param name="id">Given Languages id</param>
		public Lang(string id) {
			_filePath = Path.Combine(Environment.GetFolderPath
				(Environment.SpecialFolder.LocalApplicationData), id + ".txt");
			//Reads Words from Language from corresponding File
			string langdata = File.ReadAllText(_filePath);

			string[] langWords = langdata.Split(';');
			string key;
			string value;

			//Searches in List for matching word
			for (int i = 0; i < langWords.Length; i++)
			{
				string[] word = langWords[i].Split('=');
				key = word[0];
				value = word[1];
				_words.Add(key, value);
			}
			
		}

		/// <summary>
		/// Returns the value of Tag in the chosen Language
		/// </summary>
		/// <param name="tag">key to searched values</param>
		/// <returns></returns>
		public String get(string tag)
		{
			return _words[tag];
		}
	}
	/// <summary>
	/// This Singleton stores loads and changes the current Language.
	/// It is based on a push Observer Pattern. And sends a notification if the language is changed.
	/// </summary>
	public class LangManager : IObservable<Lang>
	{
		private static LangManager _singletonLangManager;
		private Lang[] _availableLangs;
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

		/// <summary>
		/// Initializes available languages
		/// </summary>
		public LangManager()
		{
			
			//String[] list = curDir.List();
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
		public String Tag { get; }
	}

}
