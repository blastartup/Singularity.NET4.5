using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
// ReSharper disable InheritdocConsiderUsage
// ReSharper disable once CheckNamespace

namespace Singularity
{
	/// <summary>
	/// A special class to handle worded text.
	/// </summary>
	[DebuggerStepThrough]
	public class Words : IEnumerable<String>, ICloneable<Words>, IStateEmpty
	{
		/// <summary>
		/// Instantiate an empty word delimiter.
		/// </summary>
		[DebuggerHidden]
		public Words()
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
		public Words(IEnumerable<String> collection, String delimiter = ValueLib.Space.StringValue)
		{
			_internalList = new List<String>(collection);
			_delimiter = delimiter;
		}

		/// <summary>
		/// Instantiate word handling of a given string List where the words are delimited by a space.
		/// </summary>
		/// <param name="collection">A string List of words.</param>
		[DebuggerHidden]
		public Words(List<String> collection)
		{
			_internalList = collection;
			_delimiter = ValueLib.Space.StringValue;
		}

		/// <summary>
		/// Extrapolate words in current string.
		/// </summary>
		/// <param name="value">Primary space delimited string on which to extrapolate words from.</param>
		[DebuggerHidden]
		public Words(String value)
			: this(value, ValueLib.Space.StringValue, 1, -1)
		{
		}

		/// <summary>
		/// Extrapolate words in current string.
		/// </summary>
		/// <param name="value">Primary space delimited string on which to extrapolate words from.</param>
		/// <param name="positionOfWord">Position of word to be returned.  Must be 1 or greater.</param>
		[DebuggerHidden]
		public Words(String value, Int32 positionOfWord)
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
		public Words(String value, Int32 positionOfFirstWord, Int32 wordCount)
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
		public Words(String value, String delimiter)
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
		public Words(String value, String delimiter, Int32 positionOfFirstWord, Int32 wordCount)
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

			// Originally had Regex.Split but it doesn't work with fullstops, commas or pipes.
			List<String> lWordCollection = new List<String>(value.Split(delimiter));
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
		/// Split a given command line type string where commands are separated by spaces, except when wrapped inside a double quoted command.
		/// </summary>
		/// <param name="commandLine"></param>
		/// <returns></returns>
		[DebuggerHidden]
		public static Words FromCommandLine(String commandLine)
		{
			return new Words(Split(commandLine));
		}

		private static List<String> Split(String commandLine)
		{
			List<String> wordParts = new List<String>();
			var partBuilder = new StringBuilder();
			var insideDoubleQuotes = false;
			foreach (Char c in commandLine)
			{
				if (c == ValueLib.DoubleQuotes.CharValue)
				{
					insideDoubleQuotes = !insideDoubleQuotes;
					continue;
				}

				if (c == ValueLib.Space.CharValue && !insideDoubleQuotes)
				{
					wordParts.Add(partBuilder.ToString());
					partBuilder = new StringBuilder();
					continue;
				}

				partBuilder.Append(c);
			}
			wordParts.Add(partBuilder.ToString());
			return wordParts;
		}


		/// <summary>
		/// Add another words collection to this one.
		/// </summary>
		/// <param name="otherWords">The words collection to add.  It can have a different delimiter than this one has.</param>
		[DebuggerHidden]
		public void Add(Words otherWords)
		{
			_internalList.AddRange(otherWords);
		}

		/// <summary>
		/// Add to Word Handlers together, returning a new Words with combined set of words.
		/// </summary>
		/// <param name="leftWords"></param>
		/// <param name="rightWords"></param>
		/// <returns></returns>
		[DebuggerHidden]
		public static Words operator +(Words leftWords, Words rightWords)
		{
			return new Words
			{
				_delimiter = leftWords._delimiter,
				_internalList = new List<String>(leftWords._internalList.Union(rightWords._internalList))
			};
		}

		/// <summary>
		/// Implicitly cast Words to a String.
		/// </summary>
		/// <param name="words">Word collection to implicitly cast to a String.</param>
		/// <returns></returns>
		[DebuggerHidden]
		public static implicit operator String(Words words)
		{
			return words.ToString();
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
		public Boolean Append(Words words, Boolean includeEmptyWords = false)
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
		public Words Clone()
		{
			return new Words(ToString(), _delimiter);
		}

		[DebuggerHidden]
		public Boolean Contains(String word)
		{
			return _internalList.Contains(word);
		}

		[DebuggerHidden]
		public Int32 Count => _internalList.Count();

		[DebuggerHidden]
		public Words Extract(Int32 index, Int32 count = 1)
		{
			Contract.Requires(index >= 0);
			Contract.Requires(count >= 1);

			if (_internalList.Count.Equals(0))
			{
				return new Words();
			}

			count = count.LimitInRange(1, _internalList.Count - index);

			Words result = new Words(_internalList.GetRange(index, count));
			_internalList.RemoveRange(index, count);
			return result;
		}

		[DebuggerHidden]
		public Words FormatWith(Object model, String beginTag = "{{", String endTag = "}}")
		{
			List<String> result = new List<String>(_internalList.Count);
			foreach (String item in _internalList)
			{
				result.Add(item.FormatWith(model, beginTag, endTag));
			}
			return new Words(result, _delimiter);
		}

		[DebuggerHidden]
		public IEnumerator<String> GetEnumerator()
		{
			return _internalList.GetEnumerator();
		}

		[DebuggerHidden]
		public Words GetWords(Int32 startIndex, Int32? count = null)
		{
			Contract.Requires(startIndex >= 0);

			if (startIndex > _internalList.Count - 1)
			{
				return new Words() { Delimiter = _delimiter };
			}

			return new Words(_internalList.GetRange(startIndex, count.GetValueOrDefault(_internalList.Count - startIndex).LimitMax(_internalList.Count - startIndex)), _delimiter);
		}

		[DebuggerHidden]
		public static Words GetWords(String value, String delimiter, Int32 startIndex, Int32? count = null)
		{
			var words = new Words(value, delimiter);
			return words.GetWords(startIndex, count);
		}

		[DebuggerHidden]
		public Words GetRangeNonEmpty(Int32 startIndex, Int32? count = null)
		{
			Contract.Requires(startIndex >= 0);
			Contract.Requires(count >= 1);

			return new Words(_internalList.GetRangeNonEmpty(startIndex, count.GetValueOrDefault(_internalList.Count - startIndex)), _delimiter);
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

		[DebuggerHidden]
		public Boolean InsertRange(Int32 index, Words words)
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

		[DebuggerHidden]
		public void Remove(Int32 index)
		{
			_internalList.RemoveAt(index);
		}

		[DebuggerHidden]
		public Boolean Remove(Words words)
		{
			return Remove(words._internalList.ToArray());
		}

		[DebuggerHidden]
		public Boolean Remove(params String[] wordStrings)
		{
			Boolean result = false;
			foreach (String wordString in wordStrings)
			{
				Words removeWords = new Words(wordString, _delimiter);
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
		public Words RemoveRange(Int32 index, Int32 count)
		{
			Contract.Requires(index >= 0);
			Contract.Requires(count >= 1);

			Words result = null;
			if (index.IsInRange(0, _internalList.Count - 1) && (count + index).IsInRange(1, _internalList.Count))
			{
				result = new Words(_internalList.GetRange(index, count), _delimiter);
				_internalList.RemoveRange(index, count);
			}
			return result ?? new Words() { _delimiter = _delimiter };
		}

		[DebuggerHidden]
		public void Sort()
		{
			_internalList.Sort();
		}

		[DebuggerHidden]
		public Words ToUpper()
		{
			for (Int32 idx = 0; idx < _internalList.Count; idx++)
			{
				_internalList[idx] = _internalList[idx].ToUpper();
			}
			return this;
		}

		[DebuggerHidden]
		public Words ToLower()
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

		[DebuggerHidden]
		public void UpdateRange(CodeRegion originalRegion, Words newWords)
		{
			RemoveRange(originalRegion.StartLineIndex + 1, (originalRegion.LineIndexCount - 2).LimitMin(1));
			InsertRange(originalRegion.StartLineIndex + 1, newWords);
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return _internalList.GetEnumerator();
		}

		[DebuggerHidden]
		public String Delimiter
		{
			get => _delimiter;
			set => _delimiter = value;
		}

		[DebuggerHidden]
		public Boolean IsEmpty => _internalList.IsEmpty();

		private String _delimiter;
		private List<String> _internalList;
	}
}
