using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text.RegularExpressions;

// ReSharper disable once CheckNamespace

namespace Singularity
{
	/// <summary>
	/// A special class to handle worded text.
	/// </summary>
	[DebuggerStepThrough]
	public class WordCollection : IEnumerable<String>, ICloneable<WordCollection>, IStateEmpty
	{
		/// <summary>
		/// Instantiate an empty word delemiter.
		/// </summary>
		[DebuggerHidden]
		public WordCollection()
		{
			_internalList = new List<String>();
			_delimiter = ValueLib.Space.StringValue;
		}

		/// <summary>
		/// Instantiate word handling of a given string Collection where the words are delimited by the given delimiter.
		/// </summary>
		/// <param name="collection">A string collection of words.</param>
		/// <param name="delimiter">The word delimiter which by default is a Space.</param>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed")]
		[DebuggerHidden]
		public WordCollection(ICollection<String> collection, String delimiter = ValueLib.Space.StringValue) 
		{
			_internalList = new List<String>(collection);
			_delimiter = delimiter;
		}

		/// <summary>
		/// Extrapolate words in current string.
		/// </summary>
		/// <param name="value">Primary space delimited string on which to extrapolate words from.</param>
		[DebuggerHidden]
		public WordCollection(String value)
			: this(value, ValueLib.Space.StringValue, 1, -1)
		{
		}

		/// <summary>
		/// Extrapolate words in current string.
		/// </summary>
		/// <param name="value">Primary space delimited string on which to extrapolate words from.</param>
		/// <param name="positionOfWord">Position of word to be returned.  Must be 1 or greater.</param>
		[DebuggerHidden]
		public WordCollection(String value, Int32 positionOfWord)
			: this(value, ValueLib.Space.StringValue, positionOfWord, -1)
		{
		}

		/// <summary>
		/// Extrapolate words in current string.
		/// </summary>
		/// <param name="value">Primary space delimited string on which to extrapolate words from.</param>
		/// <param name="positionOfFirstWord">Position of first word to be returned.  Must be 1 or greater.</param>
		/// <param name="wordCount">Number of words to return.</param>
		[DebuggerHidden]
		public WordCollection(String value, Int32 positionOfFirstWord, Int32 wordCount)
			: this(value, ValueLib.Space.StringValue, positionOfFirstWord, wordCount)
		{
		}

		/// <summary>
		/// Extrapolate words in current string.
		/// </summary>
		/// <param name="value">Primary delimited string on which to extrapolate words from.</param>
		/// <param name="delimiter">A string of characters that separate the words, usually a space.</param>
		/// <returns>Returns a list of a all words delimited by the given delimiter.</returns>
		[DebuggerHidden]
		public WordCollection(String value, String delimiter)
			: this(value, delimiter, 1, -1)
		{
		}

		/// <summary>
		/// Extrapolate words in current string.
		/// </summary>
		/// <param name="value">Primary delimited string on which to extrapolate words from.</param>
		/// <param name="delimiter">A string of characters that separate the words, usually a space.</param>
		/// <param name="positionOfFirstWord">Position of first word to be returned.  Must be 1 or greater.</param>
		/// <param name="wordCount">Number of words to return. If -1, then all words from the position of the first word will be returned.</param>
		/// <modified Date="15 October 2009">Created.</modified>
		[DebuggerHidden]
		public WordCollection(String value, String delimiter, Int32 positionOfFirstWord, Int32 wordCount)
		{
			Contract.Requires(positionOfFirstWord > 0);
			Contract.Requires(wordCount >= -1);

			_internalList = new List<String>(wordCount > 0 ? wordCount : 50);

			if (value.IsEmpty() || wordCount.IsEmpty())
			{
				return;
			}

			if (delimiter.IsEmpty())
			{
				delimiter = ValueLib.Space.StringValue;
			}
			_delimiter = delimiter;

			// todo - fix delimiter when it is a pipe character...
			List<String> lWordCollection = new List<String>(Regex.Split(value, delimiter));
			if (wordCount.Equals(-1))
			{
				wordCount = lWordCollection.Count;
			}
			if (positionOfFirstWord <= lWordCollection.Count)
			{
				positionOfFirstWord--;  // Adjust for zero based array.
				Int32 lLastField = (positionOfFirstWord + wordCount).LimitMax(lWordCollection.Count);
				for (Int32 iIdx = 0; iIdx < lLastField; iIdx++)
				{
					_internalList.Add(lWordCollection[iIdx]);
				}
			}
		}

		/// <summary>
		/// Add another word collection to this one.
		/// </summary>
		/// <param name="anotherWordCollection">The word collection to add.  It can have a different delimiter than this one has.</param>
		public void Add(WordCollection anotherWordCollection)
		{
			_internalList.AddRange(anotherWordCollection);
		}

		/// <summary>
		/// Add to Word Handlers together, returning a new Words with combined set of words.
		/// </summary>
		/// <param name="leftCollection"></param>
		/// <param name="rightCollection"></param>
		/// <returns></returns>
		public static WordCollection operator +(WordCollection leftCollection, WordCollection rightCollection)
		{
			return new WordCollection
			{
				_delimiter = leftCollection._delimiter,
				_internalList = new List<String>(leftCollection._internalList.Union(rightCollection._internalList))
			};
		}

		/// <summary>
		/// Implicitly cast Words to a String.
		/// </summary>
		/// <param name="wordCollection"></param>
		/// <returns></returns>
		public static implicit operator String(WordCollection wordCollection)
		{
			return wordCollection.ToString();
		}

		/// <remarks>This is exception safe if the index is invalid.</remarks>
		[DebuggerHidden]
		public String this[Int32 index]
		{
			get
			{
				String result = String.Empty;
				if (!index.IsOutOfRange(0, _internalList.Count - 1))
				{
					result = _internalList[index];
				}
				return result;
			}
			set
			{
				Contract.Requires(index >= 0);

				if (!index.IsOutOfRange(0, _internalList.Count - 1))
				{
					_internalList[index] = value;
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="word"></param>
		/// <param name="includeEmptyWords"></param>
		/// <returns></returns>
		[DebuggerHidden]
		public Boolean Append(String word, Boolean includeEmptyWords = false)
		{
			if (word.IsEmpty() && !includeEmptyWords)
			{
				return false;
			}
			_internalList.Add(word);
			return true;
		}

		[DebuggerHidden]
		public Boolean Append(WordCollection words, Boolean includeEmptyWords = false)
		{
			if (!words.IsEmpty())
			{
				foreach (String word in words)
				{
					Append(word, includeEmptyWords);
				}
				return true;
			}
			return false;
		}

		[DebuggerHidden]
		public Boolean AppendUnique(String word)
		{
			if (!word.IsEmpty() && !_internalList.Contains(word))
			{
				_internalList.Add(word);
				return true;
			}
			return false;
		}

		[DebuggerHidden]
		public WordCollection Clone()
		{
			return new WordCollection(ToString(), _delimiter);
		}

		[DebuggerHidden]
		public Boolean Contains(String word)
		{
			return _internalList.Contains(word);
		}

		[DebuggerHidden]
		public Int32 Count
		{
			get { return _internalList.Count(); }
		}

		[DebuggerHidden]
		public WordCollection Extract(Int32 index, Int32 count = 1)
		{
			Contract.Requires(index >= 0);
			Contract.Requires(count >= 1);

			if (_internalList.Count.Equals(0))
			{
				return new WordCollection();
			}

			count = count.LimitInRange(1, _internalList.Count - index);

			WordCollection result = new WordCollection(_internalList.GetRange(index, count));
			_internalList.RemoveRange(index, count);
			return result;
		}

		public WordCollection FormatWith(Object model, String beginTag = "{{", String endTag = "}}")
		{
			List<String> result = new List<String>(_internalList.Count);
			for (Int32 idx = 0; idx < _internalList.Count; idx++)
			{
				result.Add(_internalList[idx].FormatWith(model, beginTag, endTag));
			}
			return new WordCollection(result, _delimiter);
		}

		[DebuggerHidden]
		public IEnumerator<String> GetEnumerator()
		{
			return _internalList.GetEnumerator();
		}

		[DebuggerHidden]
		public WordCollection GetWords(Int32 startIndex, Int32? count = null)
		{
			Contract.Requires(startIndex >= 0);

			return new WordCollection(_internalList.GetRange(startIndex, count.GetValueOrDefault(_internalList.Count - startIndex)));
		}

		public WordCollection GetRangeNonEmpty(Int32 startIndex, Int32? count = null)
		{
			Contract.Requires(startIndex >= 0);
			Contract.Requires(count >= 1);

			return new WordCollection(_internalList.GetRangeNonEmpty(startIndex, count.GetValueOrDefault(_internalList.Count - startIndex)));
		}

		[DebuggerHidden]
		public Int32 IndexOf(String item)
		{
			return _internalList.IndexOf(item);
		}

		public Int32 IndexOf(Predicate<String> predicate, Int32 index = 0, Int32 count = 1)
		{
			if (predicate == null || index < 0 || count < 1)
			{
				return -1;
			}

			return _internalList.IndexOf(predicate, index, count);
		}

		[DebuggerHidden]
		public Boolean Insert(Int32 index, String word)
		{
			Contract.Requires(index >= 0);

			Boolean result = false;
			if (index.IsInRange(0, _internalList.Count - 1))
			{
				_internalList.Insert(index, word);
				result = true;
			}
			return result;
		}

		public Boolean InsertRange(Int32 index, WordCollection words)
		{
			Contract.Requires(index >= 0);

			Boolean result = false;
			if (index.IsInRange(0, _internalList.Count - 1))
			{

				_internalList.InsertRange(index, words._internalList);
				result = true;
			}
			return result;
		}

		[DebuggerHidden]
		public String LastWord
		{
			get 
			{
				String result = String.Empty;
				if (_internalList.Count > 0)
				{
					result = _internalList[_internalList.Count - 1];
				}
				return result; 
			}
		}

		public void Remove(Int32 index)
		{
			_internalList.RemoveAt(index);
		}

		[DebuggerHidden]
		public Boolean Remove(WordCollection words)
		{
			return Remove(words._internalList.ToArray());
		}

		[DebuggerHidden]
		public Boolean Remove(params String[] wordStrings)
		{
			Boolean result = false;
			foreach (String wordString in wordStrings)
			{
				WordCollection removeWords = new WordCollection(wordString);
				foreach (String removeWord in removeWords)
				{
					String foundWord = _internalList.FirstOrDefault(w => w.ToUpper() == removeWord);
					if (!foundWord.IsEmpty())
					{
						result |= _internalList.Remove(foundWord);
						if (!result)
						{
							break;
						}
					}
				}
			}
			return result;
		}

		[DebuggerHidden]
		public Boolean RemoveLastWord()
		{
			Boolean result = false;
			if (_internalList.Count > 0)
			{
				_internalList.RemoveAt(_internalList.Count - 1);
				result = true;
			}
			return result;
		}

		[DebuggerHidden]
		public WordCollection RemoveRange(Int32 index, Int32 count)
		{
			Contract.Requires(index >= 0);
			Contract.Requires(count >= 1);

			WordCollection result = null;
			if (index.IsInRange(0, _internalList.Count - 1) && (count + index).IsInRange(1, _internalList.Count))
			{
				result = new WordCollection(_internalList.GetRange(index, count), _delimiter);
				_internalList.RemoveRange(index, count);
			}
			return result ?? new WordCollection() { _delimiter = _delimiter };
		}

		public void Sort()
		{
			_internalList.Sort();
		}

		[DebuggerHidden]
		public WordCollection ToUpper()
		{
			for (Int32 idx = 0; idx < _internalList.Count; idx++)
			{
				_internalList[idx] = _internalList[idx].ToUpper();
			}
			return this;
		}

		[DebuggerHidden]
		public WordCollection ToLower()
		{
			for (Int32 idx = 0; idx < _internalList.Count; idx++)
			{
				_internalList[idx] = _internalList[idx].ToLower();
			}
			return this;
		}

		[DebuggerHidden]
		public override String ToString()
		{
			return String.Join(_delimiter, _internalList.ToArray());
		}

		public void UpdateRange(CodeRegion originalRegion, WordCollection newWords)
		{
			RemoveRange(originalRegion.StartLineIndex + 1, (originalRegion.LineIndexCount - 2).LimitMin(1));
			InsertRange(originalRegion.StartLineIndex + 1, newWords);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return _internalList.GetEnumerator();
		}

		public String Delimiter
		{
			get { return _delimiter; }
			set { _delimiter = value; }
		}

		public Boolean IsEmpty => _internalList.IsEmpty();

		private String _delimiter;
		private List<String> _internalList;
	}
}
