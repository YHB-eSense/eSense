using System.Collections.Generic;
using System.IO;

namespace Karl.Model
{
	/// <summary>
	/// A class that stores all important Strings for Lang data
	/// </summary>
	public class Lang
	{
		private IDictionary<string, string> _wordPairs;
		private bool _initDone = false;
		private FileInfo _baseFile;

		public string Tag { get; private set; }
		public string Name { get => Get("name"); }

		/// <summary>
		/// Sets Path for given Language
		/// </summary>
		public Lang(FileInfo File)
		{
			_wordPairs = new Dictionary<string, string>();
			_baseFile = File;
			Tag = _baseFile.Name.Split('.')[0];
		}

		/// <summary>
		/// Returns the value of Tag in the chosen Language
		/// </summary>
		/// <param name="tag">key to searched values</param>
		/// <returns></returns>
		public string Get(string tag)
		{
			if (!_initDone) Init();
			if (_wordPairs.ContainsKey(tag)) { return _wordPairs[tag]; }
			return tag;
		}
		/// <summary>
		/// This initializes the Lang data
		/// </summary>
		public void Init()
		{
			if (_initDone) return;
			System.Diagnostics.Debug.WriteLine("Init " + _baseFile.Name);
			StreamReader output = _baseFile.OpenText();
			while (!output.EndOfStream)
			{
				string langWord = output.ReadLine();
				string[] seperated = langWord.Split('=');
				if (seperated.Length > 1)
				{
					_wordPairs.Add(seperated[0], seperated[1]); System.Diagnostics.Debug.WriteLine(seperated[0] + seperated[1]);
				}
			}
			output.Close();
			System.Diagnostics.Debug.WriteLine(_wordPairs.Count);
			System.Diagnostics.Debug.WriteLine(_wordPairs.ContainsKey("title"));
			_initDone = true;
		}
	}
}
