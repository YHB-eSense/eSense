using Karl.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using Xamarin.Forms;
using System.Collections;

namespace UnitTesting.Mocks
{
	internal class TestDictionary : IDictionary<string, object>
	{
		public bool LangTest;
		public bool RemoveCalled;
		public bool AddCalled;
		private readonly Dictionary<string, object> _obj = new Dictionary<string, object>();

		public object this[string key] { get => _obj[key]; set => _obj[key] = value; }

		public ICollection<string> Keys => _obj.Keys;

		public ICollection<object> Values => _obj.Values;

		public int Count => _obj.Count;

		public bool IsReadOnly => false;

		public void Add(string key, object value)
		{
			_obj.Add(key, value);
			AddCalled = (key.Equals("lang") && value.ToString().Equals("lang_english")) | AddCalled;
		}

		public void Add(KeyValuePair<string, object> item)
		{
			throw new NotImplementedException();
		}

		public void Clear()
		{
			_obj.Clear();
		}

		public bool Contains(KeyValuePair<string, object> item)
		{
			throw new NotImplementedException();
		}

		public bool ContainsKey(string key)
		{
			return _obj.ContainsKey(key);
		}

		public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
		{
			throw new NotImplementedException();
		}

		public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
		{
			throw new NotImplementedException();
		}

		public bool Remove(string key)
		{
			_obj.Remove(key);
			RemoveCalled = (key.Equals("lang")) | RemoveCalled;
			return RemoveCalled;
		}

		public bool Remove(KeyValuePair<string, object> item)
		{
			throw new NotImplementedException();
		}

		public bool TryGetValue(string key, out object value)
		{
			var found =_obj.TryGetValue(key, out value);
			LangTest = (key.Equals("lang")) | LangTest;
			return found;
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			throw new NotImplementedException();
		}
	}
}
