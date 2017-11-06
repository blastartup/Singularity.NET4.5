using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

// ReSharper disable once CheckNamespace

namespace Singularity
{
	[DebuggerStepThrough]
	public class KeyValuePairs : IEnumerable<KeyValuePair<String, String>>
	{
		public KeyValuePairs() : this(String.Empty, ValueLib.SemiColon.CharValue, ValueLib.EqualsSign.CharValue, false) { }

		public KeyValuePairs(String keyValueString) : this(keyValueString, ValueLib.SemiColon.CharValue, ValueLib.EqualsSign.CharValue, false) { }

		public KeyValuePairs(String keyValueString, Char pairDelimiter, Char keyValueDelimiter, Boolean caseInsensitive = false)
		{
			_caseInsensitive = caseInsensitive;
			if (!caseInsensitive)
			{
				_internalValue = new Dictionary<String, String>(StringComparer.OrdinalIgnoreCase);
			}

			_pairDelimiter = pairDelimiter;
			_keyValueDelimiter = keyValueDelimiter;

			if (!keyValueString.IsEmpty())
			{
				IList<String> lPairs = keyValueString.Split(pairDelimiter);
				_internalValue = new Dictionary<String, String>(lPairs.Count);
				foreach (String lPair in lPairs)
				{
					if (!lPair.IsEmpty())
					{
						IList<String> pairValues = lPair.Split(keyValueDelimiter);
						if (pairValues.Count == 2)
						{
							Add(pairValues[0], pairValues[1]);
						}
					}
				}
			}
			else
			{
				_internalValue = new Dictionary<String, String>(0);
			}
		}

		public KeyValuePairs(IDictionary<String, String> keyValueDictionary)
		{
			_caseInsensitive = false;
			_pairDelimiter = ValueLib.SemiColon.CharValue;
			_keyValueDelimiter = ValueLib.EqualsSign.CharValue;
			_internalValue = new Dictionary<String, String>(keyValueDictionary.Count);

			foreach (KeyValuePair<String, String> keyValuePair in keyValueDictionary)
			{
				Add(keyValuePair.Key, keyValuePair.Value);
			}
		}

		public Boolean ContainsValue(String value)
		{
			return InternalValue.ContainsValue(value);
		}

		#region IDictionary<string,string> Members

		public void Add(String key, String value)
		{
			InternalValue.Add(LowerCaseKeyIfCaseInsensitive(key), value);
		}

		public void Set(String key, String value)
		{
			if (!InternalValue.ContainsKey(key))
			{
				InternalValue.Add(LowerCaseKeyIfCaseInsensitive(key), value);
			}
			else
			{
				InternalValue[LowerCaseKeyIfCaseInsensitive(key)] = value;
			}
		}

		public Boolean ContainsKey(String key)
		{
			return InternalValue.ContainsKey(LowerCaseKeyIfCaseInsensitive(key));
		}

		public IList<String> Keys => new List<String>(InternalValue.Keys);

		public Boolean Remove(String key)
		{
			return InternalValue.Remove(LowerCaseKeyIfCaseInsensitive(key));
		}

		public Boolean TryGetValue(String key, out String value)
		{
			return InternalValue.TryGetValue(LowerCaseKeyIfCaseInsensitive(key), out value);
		}

		public IList<String> Values => new List<String>(InternalValue.Values);

		public String this[String key]
		{
			get { return InternalValue[LowerCaseKeyIfCaseInsensitive(key)]; }
			set { InternalValue[LowerCaseKeyIfCaseInsensitive(key)] = value; }
		}

		#endregion

		#region ICollection<KeyValuePair<string,string>> Members

		public void Add(KeyValuePair<String, String> item)
		{
			InternalValue.Add(ObtainUniqueKey(item.Key), item.Value);
		}

		private String ObtainUniqueKey(String key)
		{
			String lookupKey = LowerCaseKeyIfCaseInsensitive(key);
			Int32 occ = 0;
			while (InternalValue.ContainsKey(lookupKey))
			{
				occ++;
				lookupKey = "{0} [{1}]".FormatX(key, occ);
			}
			return lookupKey;
		}

		public void Clear()
		{
			InternalValue.Clear();
		}

		public Boolean Contains(KeyValuePair<String, String> item)
		{
			return InternalValue.Contains(item);
		}

		public void CopyTo(KeyValuePair<String, String>[] array, Int32 arrayIndex)
		{
			throw new NotImplementedException();
		}

		public Int32 Count => InternalValue.Count;

		public Boolean IsReadOnly => false;

		public Boolean Remove(KeyValuePair<String, String> item)
		{
			return InternalValue.Remove(item.Key);
		}

		#endregion

		#region IEnumerable Members

		public IEnumerator GetEnumerator()
		{
			return InternalValue.GetEnumerator();
		}

		IEnumerator<KeyValuePair<String, String>> IEnumerable<KeyValuePair<String, String>>.GetEnumerator()
		{
			return InternalValue.GetEnumerator();
		}
		#endregion

		public override String ToString()
		{
			if (InternalValue.Count == 0)
			{
				return String.Empty;
			}

			DelimitedStringBuilder builder = new DelimitedStringBuilder(InternalValue.Count);
			foreach (KeyValuePair<String, String> keyValue in InternalValue)
			{
				builder.Add(Pattern.FormatX(keyValue.Key, KeyValueDelimiter, keyValue.Value));
			}
			return builder.ToDelimitedString(PairDelimiter);
		}

		private const String Pattern = "{0}{1}{2}";


		public Char PairDelimiter => !_pairDelimiter.IsEmpty() ? _pairDelimiter : (_pairDelimiter = ValueLib.SemiColon.CharValue);
		private Char _pairDelimiter;

		public Char KeyValueDelimiter => !_keyValueDelimiter.IsEmpty() ? _keyValueDelimiter : (_keyValueDelimiter = ValueLib.EqualsSign.CharValue);
		private Char _keyValueDelimiter;

		public Dictionary<String, String> PairsDictionary => InternalValue;

		private Dictionary<String, String> InternalValue => _internalValue ?? (_internalValue = new Dictionary<String, String>());
		private Dictionary<String, String> _internalValue;

		public Boolean IsEmpty => InternalValue.Count == 0;

		private String LowerCaseKeyIfCaseInsensitive(String key)
		{
			return _caseInsensitive ? key.ToLower() : key;
		}

		private readonly Boolean _caseInsensitive;
	}
}
