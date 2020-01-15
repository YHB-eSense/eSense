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
		private Boolean initDone = false;
		private FileInfo BaseFile;
		public String Tag { get; private set; }
		public string Name { get => Get("name"); }

		/// <summary>
		/// Sets Path for given Language
		/// </summary>
		public Lang(FileInfo File) {
			_wordPairs = new Dictionary<string, string>();
			BaseFile = File;
			Tag = BaseFile.Name.Split('.')[0];
		}

		/// <summary>
		/// Returns the value of Tag in the chosen Language
		/// </summary>
		/// <param name="tag">key to searched values</param>
		/// <returns></returns>
		public string Get(string tag)
		{
			if (!initDone) Init();
			if (_wordPairs.ContainsKey(tag)) { return _wordPairs[tag]; }
			return WORD_NOT_FOUND_MESSAGE;
		}

		/// <summary>
		/// This initializes the Lang data
		/// </summary>
		public void Init()
		{
			if (initDone) return;
			/*//Reads Words from Language from corresponding File
			string[] langWords = File.ReadAllLines(filePath);

			//Searches in List for matching word
			foreach (string langWord in langWords)
			{*/

			System.Diagnostics.Debug.WriteLine("Init " + BaseFile.Name);

			StreamReader output = BaseFile.OpenText();

			while (!output.EndOfStream)
			{
				string langWord = output.ReadLine();
				string[] seperated = langWord.Split('=');
				if (seperated.Length > 1) { _wordPairs.Add(seperated[0], seperated[1]); System.Diagnostics.Debug.WriteLine(seperated[0] + seperated[1]); }
			}
			output.Close();
			System.Diagnostics.Debug.WriteLine(_wordPairs.Count);
			System.Diagnostics.Debug.WriteLine(_wordPairs.ContainsKey("title"));
			initDone = true;
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
		private Lang _currentLang;
		public List<Lang> AvailableLangs { get; }
		public Dictionary<String, Lang> LangMap { get; private set; }

		/// <summary>
		/// The Language obj. currently selected.
		/// </summary>
		public Lang CurrentLang
		{
			get => _currentLang;
			set
			{
				_currentLang = value;
				value.Init();
				foreach (IObserver<Lang> observer in Observers)
				{
					observer.OnNext(value);
				}
			}
		}

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

					//In Case VM needs LangData before Settings were loaded.
					SettingsHandler.Init();

					return _singletonLangManager;
				}
				return _singletonLangManager;
			}
		}

		/// <summary>
		/// This tries to find a lang with the tag specified.
		/// </summary>
		/// <param name="tag">The language tag</param>
		/// <returns>true if lang was found. False otherwise.</returns>
		public Boolean ChooseLang(String tag)
		{
			if( LangMap.TryGetValue(tag, out Lang lang ))
			{
				CurrentLang = lang;
				return true;
			} else
			{
				return false;
			}
		}

		/// <summary>
		/// Initializes available languages
		/// </summary>
		private LangManager()
		{
			AvailableLangs = new List<Lang>();
			LangMap = new Dictionary<String, Lang>();
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

			//Create Lang Dir in LocalApplicationData
			string LangDirPath = Path.Combine(directory, "lang");
			DirectoryInfo LangDir = new DirectoryInfo(LangDirPath);
			if (!LangDir.Exists) LangDir.Create();
			FileInfo[] LangFiles = LangDir.GetFiles("*.lang");

			//write english to LocalApplicationData if it does not exist already
			if (!File.Exists(Path.Combine(LangDirPath, "lang_english.lang")))
			{
				File.WriteAllText(Path.Combine(LangDirPath, "lang_english.lang"), english);
			}

			//write german to LocalApplicationData if it does not exist already
			if (!File.Exists(Path.Combine(LangDirPath, "lang_german.lang")))
			{
				File.WriteAllText(Path.Combine(LangDirPath, "lang_german.lang"), german);
			}

			//load all langs from LocalApplicationData
			string[] filePaths = Directory.GetFiles(directory);
			foreach (FileInfo file in LangFiles)
			{
				Lang NewLang = new Lang(file);
				AvailableLangs.Add(NewLang);
				LangMap.Add(NewLang.Tag, NewLang);
			}

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
