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
		private IDictionary <string, string> _wordPairs;

		public string Name { get => Get("name"); }
		/// <summary>
		/// Sets Path for given Language
		/// </summary>
		public Lang(string filePath) {
			_wordPairs = new Dictionary<string, string>();

			//Reads Words from Language from corresponding File
			string[] langWords = File.ReadAllLines(filePath);

			//Searches in List for matching word
			foreach (string langWord in langWords)
			{
				string[] seperated = langWord.Split('=');
				if(seperated.Length > 1) { _wordPairs.Add(seperated[0], seperated[1]); System.Diagnostics.Debug.WriteLine(seperated[0] + seperated[1]); }
			}
			System.Diagnostics.Debug.WriteLine(_wordPairs.Count);
			System.Diagnostics.Debug.WriteLine(_wordPairs.ContainsKey("title"));

		}

		/// <summary>
		/// Returns the value of Tag in the chosen Language
		/// </summary>
		/// <param name="tag">key to searched values</param>
		/// <returns></returns>
		public string Get(string tag)
		{
			if (_wordPairs.ContainsKey(tag)) { return _wordPairs[tag]; }
			return WORD_NOT_FOUND_MESSAGE;
		}
	}
	/// <summary>
	/// This Singleton stores loads and changes the current Language.
	/// It is based on a push Observer Pattern. And sends a notification if the language is changed.
	/// </summary>
	public class LangManager : IObservable<Lang>
	{
		private ConfigFile _configFile;
		private static LangManager _singletonLangManager;
		private readonly List<IObserver<Lang>> Observers;
		public List<Lang> AvailableLangs { get; }

		/// <summary>
		/// The Language obj. currently selected.
		/// </summary>
		public Lang CurrentLang { get; set; }

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
				return _singletonLangManager;
			}
		}

		/// <summary>
		/// Initializes available languages
		/// </summary>
		private LangManager()
		{
			AvailableLangs = new List<Lang>();
			//LocalApplicationData
			string directory = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
			//load embedded resource "english.txt"
			var stream = GetType().GetTypeInfo().Assembly.GetManifestResourceStream("Karl.english.txt");
			var reader = new StreamReader(stream);
			string english = reader.ReadToEnd();

			//load embedded resource "german.txt"
			stream = GetType().GetTypeInfo().Assembly.GetManifestResourceStream("Karl.german.txt");
			reader = new StreamReader(stream);
			string german = reader.ReadToEnd();

			//write english to LocalApplicationData if it does not exist already
			if (!File.Exists(Path.Combine(directory, "lang_english.txt")))
			{
				File.WriteAllText(Path.Combine(directory, "lang_english.txt"), english);
			}

			//write german to LocalApplicationData if it does not exist already
			if (!File.Exists(Path.Combine(directory, "lang_german.txt")))
			{
				File.WriteAllText(Path.Combine(directory, "lang_german.txt"), german);
			}

			//load all langs from LocalApplicationData
			string[] filePaths = Directory.GetFiles(directory);
			foreach (string filePath in filePaths)
			{
				if (filePath.Contains("lang")) { AvailableLangs.Add(new Lang(filePath)); }
			}

			//for testing
			CurrentLang = AvailableLangs.First();

			//Init Observable Pattern
			Observers = new List<IObserver<Lang>>();
		}
	
		//todo https://docs.microsoft.com/en-us/dotnet/api/system.iobservable-1?view=netframework-4.8
		/// <summary>
		/// Usual Subscribe method.
		/// </summary>
		/// <param name="observer">The Observer you want to register.</param>
		/// <returns>The IDisposable for Unsubscribing.</returns>
		public IDisposable Subscribe(IObserver<Lang> observer)
		{
			//todo
			Observers.Add(observer);
			return new Unsubscriber(this, observer);
		}

		private class Unsubscriber : IDisposable
		{
			LangManager parent;
			IObserver<Lang> myObserver;
			public void Dispose()
			{
				parent.Observers.Remove(myObserver);
			}
			public Unsubscriber(LangManager parent, IObserver<Lang> observer)
			{
				this.parent = parent;
				this.myObserver = observer;
			}
		}
	}

	 /*
	/// <summary>
	/// A general struct to indentify a language.
	/// </summary>
	public struct Language 
	{
		public String Name { get; }
		public String Tag { get; }
	}
	*/

	}
