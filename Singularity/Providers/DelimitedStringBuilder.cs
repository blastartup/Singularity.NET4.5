using System;
using System.Collections.Generic;
using System.Text;

// ReSharper disable once CheckNamespace
namespace Singularity
{
	/// <summary>
	/// StringFieldBuilder, based on System.StringBuilder, allows building a segregated or essentially a delimited String, one field at a time.
	/// </summary>
	/// <remarks> Large blobs of Strings, delimiting and field surrounding are only allocated when ToString() is called.</remarks>
	/// <Modification Name="Created" Date="28-Mar-09">Enhance the StringBuilder to support Delimited String Building.</Modification>
	public sealed class DelimitedStringBuilder : IStateEmpty
	{
		#region Constructors

		public DelimitedStringBuilder() { }

		public DelimitedStringBuilder(Int32 fieldCapacity)
		{
			_fieldCapacity = fieldCapacity;
		}

		public DelimitedStringBuilder(Int32 fieldCapacity, Int32 maximumLength)
		{
			_fieldCapacity = fieldCapacity;
			MaximumLength = maximumLength;
		}

		public DelimitedStringBuilder(String value)
		{
			Add(value);
		}

		public DelimitedStringBuilder(String value, Int32 fieldCapacity)
		{
			_fieldCapacity = fieldCapacity;
			Add(value);
		}

		public DelimitedStringBuilder(IEnumerable<String> stringList)
		{
			if (stringList != null)
			{
				StringList.AddRange(stringList);
				StringList.ForEach(s => Length += s.Length);
			}
		}

		public DelimitedStringBuilder(IList<String> stringList)
		{
			if (stringList != null)
			{
				StringList.AddRange(stringList);
				StringList.ForEach(s => Length += s.Length);
			}
		}

		#endregion

		public void Add(DelimitedStringBuilder stringBuilder)
		{
			if (stringBuilder != null)
			{
				StringList.AddRange(stringBuilder.StringList);
			}
		}

		public void Add(IEnumerable<String> stringList)
		{
			if (stringList != null)
			{
				StringList.AddRange(stringList);
				StringList.ForEach(s => Length += s.Length);
			}
		}

		#region Add

		public void Add()
		{
			StringList.Add(String.Empty);
		}

		public void Add(String value)
		{
			Length += value.Length;
			StringList.Add(value);
		}

		public void Add(Boolean value)
		{
			Add(value.ToString());
		}

		public void Add(Char value)
		{
		   Add(value.ToString());
		}

		public void Add(String format, params Object[] values)
		{
			Add(format.FormatX(values));
		}

		public void Add(IFormatProvider formatProvider, String format, params Object[] values)
		{
			Add(format.FormatX(formatProvider, values));
		}

		#endregion

		public void AddIfNotEmpty(String value)
		{
			if (!value.IsEmpty())
			{
				Add(value);
			}
		}

		public void AddIfNotEmpty(String formatMask, params Object[] values)
		{
			if (!values.IsEmpty())
			{
				Add(formatMask.FormatX(values));
			}
		}

		public void Insert(String value)
		{
			Insert(0, value);
		}

		public void Insert(Int32 fieldIndex, String value)
		{
			Length += value.Length;
			StringList.Insert(fieldIndex, value);
		}

		public void Delete(Int32 fieldIndex)
		{
			StringList.RemoveAt(fieldIndex);
		}

		public void Delete(Int32 fieldIndex, Int32 count)
		{
			for (Int32 lIdx = fieldIndex; lIdx < count; lIdx++)
			{
				StringList.RemoveAt(lIdx);
			}
		}

		public void Delete(String value)
		{
			StringList.Remove(value);
		}

		public void Update(Int32 fieldIndex, String value)
		{
			StringList.RemoveAt(fieldIndex);
			StringList.Insert(fieldIndex, value);
		}

		public String Select(Int32 fieldIndex)
		{
			return StringList[fieldIndex];
		}

		public override String ToString()
		{
			return ToStringCore(String.Empty, 0, StringList.Count);
		}

		public String ToString(Int32 startFieldIndex, Int32 count)
		{
			return ToStringCore(String.Empty, startFieldIndex, count);
		}

		public Words ToDelimitedWords(String delimiter = " ")
		{
			return new Words(_stringList, delimiter);
		}

		public String ToNewLineDelimitedString()
		{
			return ToStringCore(Environment.NewLine, 0, StringList.Count);
		}

		public String ToNewLineDelimitedString(Int32 startFieldIndex, Int32 count)
		{
			return ToStringCore(Environment.NewLine, startFieldIndex, count);
		}

		/// <summary>
		/// Space Delimited String
		/// </summary>
		/// <returns>Currently appended string values are returned a one space delimted string.</returns>
		public String ToDelimitedString()
		{
			return ToStringCore(ValueLib.Space.StringValue, 0, StringList.Count);
		}

		public String ToDelimitedString(Char delimiter)
		{
			return ToStringCore(delimiter.ToString(), 0, StringList.Count);
		}

		public String ToDelimitedString(Char delimiter, Int32 startFieldIndex, Int32 count)
		{
			return ToStringCore(delimiter.ToString(), startFieldIndex, count);
		}

		public String ToDelimitedString(String delimiter)
		{
			return ToStringCore(delimiter, 0, StringList.Count);
		}

		public String CopyDelimitedTo(String delimiter, Int32 startFieldIndex, Int32 count)
		{
			return ToStringCore(delimiter, startFieldIndex, count);
		}

		public static String DelimitedString(params String[] values)
		{
			return new DelimitedStringBuilder(values).ToDelimitedString();
		}

		private String ToStringCore(String delimiter, Int32 startFieldIndex, Int32 count)
		{
			String result = String.Empty;
			if (StringList.Count > 0)
			{
				StringBuilder lBuilder = ToStringBuilderCore(delimiter, startFieldIndex, count);
				result = lBuilder.ToString();
			}
			return result;
		}

		private StringBuilder ToStringBuilderCore(String delimiter, Int32 startFieldIndex, Int32 count)
		{
			StringBuilder lBuilder = delimiter.IsEmpty() ? NewStringBuilder(Length) : NewStringBuilder(Length + (StringList.Count * delimiter.Length));
			Boolean lHasRunOnceAtLeast = false;
			for (Int32 iIdx = startFieldIndex; iIdx < count; iIdx++)
			{
				if (!delimiter.IsEmpty())
				{
					if (!lHasRunOnceAtLeast)
					{
						lHasRunOnceAtLeast = true;
					}
					else
					{
						lBuilder.Append(delimiter);
					}
				}
				if (IsFieldSurroundMarksValid)
				{
					lBuilder.Append(BeginFieldMark + StringList[iIdx] + EndFieldMark);
				}
				else
				{
					lBuilder.Append(StringList[iIdx]);
				}
			}
			return lBuilder;
		}

		public Boolean IsEmpty => StringList.IsEmpty();

		public Int32 Length { get; private set; }

		private StringBuilder NewStringBuilder(Int32 length)
		{
			if (MaximumLength > 0)
			{
				return new StringBuilder(length, MaximumLength);
			}
			return new StringBuilder(length);
		}

		public Int32 MaximumLength { get; set; }

		private List<String> StringList => _stringList ?? (_stringList = new List<String>(_fieldCapacity));

		private List<String> _stringList;

		private Boolean IsFieldSurroundMarksValid => !BeginFieldMark.IsEmpty() && !EndFieldMark.IsEmpty();

		public String BeginFieldMark { get; set; }
		public String EndFieldMark { get; set; }

		private Int32 _fieldCapacity;
	}
}

