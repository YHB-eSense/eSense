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
		public Stack<string> RemoveCalls = new Stack<string>();
		public Stack<string> ContainsKeyCalls = new Stack<string>();
		public Stack<Tuple<string, object>> AddCalls = new Stack<Tuple<string, object>>();
		public Stack<Tuple<string, object>> TryGetValueCalls = new Stack<Tuple<string, object>>();

		public bool TriggerTryGetValueCalled_lang
		{
			get
			{
				foreach (var call in TryGetValueCalls)
					if (call.Item1.Equals("lang"))
						return true;
				return false;
			}
		}
		public bool TriggerRemoveCalled_lang
		{
			get
			{
				return RemoveCalls.Contains("lang");
			}
		}
		public bool TriggerAddCalled_lang_english
		{
			get
			{
				foreach (var call in AddCalls)
				{
					if (call.Item1.Equals("lang") && call.Item2.ToString().Equals("lang_english")) return true;
				}
				return false;
			}
		}

		private readonly Dictionary<string, object> _obj = new Dictionary<string, object>();

		public object this[string key] { get => _obj[key]; set => _obj[key] = value; }

		public ICollection<string> Keys => _obj.Keys;

		public ICollection<object> Values => _obj.Values;

		public int Count => _obj.Count;

		public bool IsReadOnly => false;

		public void Add(string key, object value)
		{
			_obj.Add(key, value);
			AddCalls.Push(new Tuple<string, object>(key, value));
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
			ContainsKeyCalls.Push(key);
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
			RemoveCalls.Push(key);
			return _obj.Remove(key);
		}

		public bool Remove(KeyValuePair<string, object> item)
		{
			throw new NotImplementedException();
		}

		public bool TryGetValue(string key, out object value)
		{
			var found =_obj.TryGetValue(key, out value);
			TryGetValueCalls.Push(new Tuple<string, object>(key, value));
			return found;
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			throw new NotImplementedException();
		}

	}
}
