using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Karl.Model
{
	/// <summary>
	/// This Singleton stores loads and changes the current Language.
	/// It is based on a push Observer Pattern. And sends a notification if the language is changed.
	/// </summary>
	public class LangManager : IObservable<Lang>
	{
		private static LangManager _singletonLangManager;
		private readonly List<IObserver<Lang>> _observers;
		private Lang _currentLang;

		public List<Lang> AvailableLangs { get; }
		public Dictionary<string, Lang> LangMap { get; private set; }
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
				foreach (IObserver<Lang> observer in _observers)
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
		public bool ChooseLang(string tag)
		{
			if (LangMap.TryGetValue(tag, out Lang lang))
			{
				CurrentLang = lang;
				return true;
			}
			else { return false; }
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
			var stream = GetType().GetTypeInfo().Assembly.GetManifestResourceStream("Karl.Model.Languages.english.txt");
			var reader = new StreamReader(stream);
			string english = reader.ReadToEnd();
			reader.Close();

			//load embedded resource "german.txt"
			stream = GetType().GetTypeInfo().Assembly.GetManifestResourceStream("Karl.Model.Languages.german.txt");
			reader = new StreamReader(stream);
			string german = reader.ReadToEnd();
			reader.Close();

			//Create Lang Dir in LocalApplicationData
			string LangDirPath = Path.Combine(directory, "lang");
			DirectoryInfo LangDir = new DirectoryInfo(LangDirPath);
			if (!LangDir.Exists) LangDir.Create();

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

			//Fetch all files from LangDir
			FileInfo[] LangFiles = LangDir.GetFiles("*.lang");

			//load all langs from LocalApplicationData
			string[] filePaths = Directory.GetFiles(directory);
			foreach (FileInfo file in LangFiles)
			{
				Lang NewLang = new Lang(file);
				AvailableLangs.Add(NewLang);
				LangMap.Add(NewLang.Tag, NewLang);
			}

			//Init Observable Pattern
			_observers = new List<IObserver<Lang>>();
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
			_observers.Add(observer);
			return new Unsubscriber(this, observer);
		}

		private class Unsubscriber : IDisposable
		{
			private LangManager _parent;
			private IObserver<Lang> _myObserver;
			public Unsubscriber(LangManager parent, IObserver<Lang> observer)
			{
				_parent = parent;
				_myObserver = observer;
			}
			public void Dispose()
			{
				_parent._observers.Remove(_myObserver);
			}
		}
	}

}

