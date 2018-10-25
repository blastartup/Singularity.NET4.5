using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security;
using System.Security.Cryptography;
using System.Security.Permissions;
using System.Text;
using System.Text.RegularExpressions;

// ReSharper disable once CheckNamespace
namespace Singularity
{
	/// <summary>
	/// Foundation default extension methods to the standard String object.
	/// </summary>
	[DebuggerStepThrough]
	public static class StringExtension
	{
		/// <summary>
		/// Given a string in CamelCase, return the string by inserting a space between each capitalised word. eg: "Camel Case"
		/// </summary>
		/// <param name="pascelCaseToken">String with expected pascal casing eg: "PascelCased".</param>
		/// <returns>A spaced out string of words.</returns>
		public static String Humanise(this String pascelCaseToken)
		{
			String result;
			if (pascelCaseToken.EndsWith("ID"))
			{
				result = HumaniseCore(pascelCaseToken.Substring(0, pascelCaseToken.Length - 2)) + " ID";
			}
			else
			{
				result = HumaniseCore(pascelCaseToken);
			}
			return result;
		}

		private static String HumaniseCore(String pascelCaseToken)
		{
			return Regex.Replace(pascelCaseToken, "([A-Z])", " $1", RegexOptions.Compiled).Trim();
		}

		/// <summary>
		/// Produces a string where {{ L0[.LN]+ }} is replaced with the value of the corresponding (sub)object. Similar to MVC views.
		/// </summary>
		/// <param name="template">The string template to be processed</param>
		/// <param name="model">A composite object to supply template parameters. Only public non-indexed properties</param>
		/// <param name="beginTag">An optional string for the beginning tag characters. Default is "{{"</param>
		/// <param name="endTag">An optional string for the ending tag characters. Default is "}}"</param>
		/// <returns></returns>
		public static String FormatWith(this String template, Object model, String beginTag = "{{", String endTag = "}}")
		{
			return !template.IsEmpty() ? TemplateRegex(beginTag, endTag, StandardRegexTagFilter).Replace(template, m => ProcessPropertyMatch(m, model, beginTag, endTag))
				: null;
		}

		public static String FormatMoney(this String amount)
		{
			return amount.ToDecimal().FormatMoney();
		}

		[DebuggerStepThrough]
		public static String FormatX(this String format, params Object[] values)
		{
			return FormatX(format, Factory.CurrentCultureInfo, values);
		}

		[DebuggerStepThrough]
		public static String FormatX(this String format, CultureInfo cultureInfo, params Object[] values)
		{
			var safeArgs = new ArrayList(values.Length);

			foreach (var arg in values)
			{
				safeArgs.Add(arg ?? String.Empty);
			}
			return String.Format(cultureInfo, format, safeArgs.ToArray());
		}

		[DebuggerStepperBoundary]
		public static String Join(this IEnumerable<String> values)
		{
			return String.Join(String.Empty, values);
		}

		private static Regex TemplateRegex(String beginTag, String endTag, String regexFilter)
		{
			var key = beginTag + (regexFilter == StandardRegexTagFilter ? "1" : "2") + endTag;
			if (!RegexTagFilters.ContainsKey(key))
			{
				RegexTagFilters[key] = new Regex(
					"{0}{1}{2}".FormatX(beginTag.Replace("{", @"\{"), regexFilter, endTag.Replace("}", @"\}")),
					RegexOptions.Compiled);
			}
			return RegexTagFilters[key];
		}
		private const String StandardRegexTagFilter = @"(?<name>(?:[A-z]|[0-9]|\.)+)";

		private static Dictionary<String, Regex> RegexTagFilters => _regexTagFilters ?? (_regexTagFilters = new Dictionary<String, Regex>());
		private static Dictionary<String, Regex> _regexTagFilters;

		private static String ProcessPropertyMatch(Match m, Object model, String beginTag, String endTag)
		{
			var fullName = m.Groups["name"].Value;
			var varName = fullName;
			var indexOfDot = varName.IndexOf('.');
			while (indexOfDot != -1)
			{
				var currentObjectName = varName.Substring(0, indexOfDot);
				varName = varName.Substring(indexOfDot + 1);
				indexOfDot = varName.IndexOf('.');
				model = model.GetPropertyValue(currentObjectName);
				if (model == null) return beginTag + fullName + endTag; //if not found - return unchanged 
			}
			var ret = model.GetPropertyValue(varName);
			return ret?.ToString() ?? beginTag + fullName + endTag; //if not found - return unchanged 
		}



		/// <summary>
		/// The Position of a character value at the nth Occurrence within a string. Will return -1 if not found
		/// </summary>
		/// <param name="value">The character to seek</param>
		/// <param name="occurrence">The nth occurrence to seek.</param>
		[DebuggerStepperBoundary]
		public static Int32 IndexOf(this String input, Char value, Int32 occurrence)
		{
			var result = -1;
			var matchCount = 0;
			var position = 0;
			while (matchCount < occurrence && position < input.Length)
			{
				if (input[position] == value) matchCount++;
				position++;
			}
			if (matchCount == occurrence) result = position - 1;
			return result;
		}

		/// <summary>
		/// The index of the last occurrence of the specified case insensitive string.
		/// </summary>
		/// <param name="value">The string to seek.</param>
		[DebuggerStepperBoundary]
		public static Int32 LastIndexOfIgnoringCase(this String input, String value, Int32 startIndex)
		{
			Contract.Requires(!value.IsEmpty());
			Contract.Requires(startIndex >= 0);

			if (startIndex <= (input.Length - 1).LimitMin(0))
			{
				return input.LastIndexOf(value, startIndex, StringComparison.OrdinalIgnoreCase);
			}
			else
			{
				return -1;
			}
		}

		/// <summary>
		/// This instance with a specified string inserted at a specified index position.
		/// </summary>
		/// <param name="startIndex">The index position of the insertion.</param>
		/// <param name="value">The string to insert.</param>
		/// <returns></returns>
		[DebuggerStepperBoundary]
		public static String Insert(this String input, Int32 startIndex, Char value)
		{
			Contract.Requires(startIndex >= 0);

			return InsertSafe(input, startIndex, value.ToString());
		}

		/// <summary>
		/// This instance with a specified string inserted at a specified index position.
		/// </summary>
		/// <param name="startIndex">The index position of the insertion.</param>
		/// <param name="value">The string to insert.</param>
		/// <returns></returns>
		[DebuggerStepperBoundary]
		public static String InsertSafe(this String input, Int32 startIndex, String value)
		{
			Contract.Requires(startIndex >= 0);
			var result = input;
			if (!value.IsEmpty() && startIndex >= 0 && startIndex <= input.Length)
			{
				result = input.Insert(startIndex, value);
			}
			return result;
		}

		public static String RemovePrefix(this String input, String prefix, StringComparison comparison = StringComparison.Ordinal)
		{
			if (prefix == null || !input.StartsWith(prefix, comparison))
			{
				return input;
			}
			return RemoveSafe(input, 0, prefix.Length);
		}

		public static String RemoveSuffix(this String input, String suffix, StringComparison comparison = StringComparison.Ordinal)
		{
			if (suffix == null || !input.EndsWith(suffix, comparison))
			{
				return input;
			}
			return RemoveSafe(input, input.Length - suffix.Length, suffix.Length);
		}

		[DebuggerStepperBoundary]
		public static String RemoveSafe(this String input, Int32 startIndex, Int32 count)
		{
			String result;
			if (startIndex < 0 || startIndex > input.Length)
			{
				result = input; // don't remove anything
			}
			else
			{
				if (startIndex + count > input.Length)
				{
					count = input.Length - startIndex;
				}

				result = input.Remove(startIndex, count);
			}
			return result;
		}

		public static IList<String> Split(this String input, String delimiter)
		{
			return new List<String>(input.Split(new[] { delimiter }, StringSplitOptions.None));
		}

		/// <summary>
		/// Split the current string into segments.
		/// </summary>
		/// <param name="segmentLength" type="int">Length of sub-strings to return.</param>
		/// <returns>A list of sub-strings of given segment length.</returns>
		public static IList<String> Split(this String input, Int32 segmentLength)
		{
			var result = new List<String>();
			for (var i = 0; i < input.Length; i += segmentLength)
			{
				result.Add(SubstringSafe(input, i, segmentLength));

			}
			return result;
		}

		/// <summary>
		/// Split the delimited current string safely.
		/// </summary>
		/// <returns>A list of sub-strings of given segment length.</returns>
		public static IList<String> SplitSafe(this String input, params Char[] seperator)
		{
			var result = new List<String>();
			if (!input.IsEmpty() && !seperator.IsEmpty())
			{
				result.AddRange(input.Split(seperator));
			}
			return result;
		}

		public static Boolean StartsWithIn(this String input, params String[] startsWithValues)
		{
			return startsWithValues.Any(input.StartsWith);
		}

		public static Int32 SequentialCountReversed(this String value, Int32 startIndex, Char lookupChar, Boolean untilNewLine = false)
		{
			startIndex--;
			if (value.IsEmpty() || startIndex <= 0 || startIndex > value.Length)
			{
				return 0;
			}

			var counter = 0;
			for (var idx = startIndex; idx > 0; idx--)
			{
				if (value[idx] == lookupChar)
				{
					counter++;
					continue;
				}

				if (untilNewLine && value[idx] != ValueLib.NewLine.CharValue)
				{
					continue;
				}
				break;
			}

			return counter;
		}

		/// <summary>
		/// Truncate a string from either the start or end upon the locating the first occurrence of a given character.
		/// </summary>
		/// <param name="value">String to truncate.</param>
		/// <param name="firstOccuranceOf">Character to locate in the String to mark the truncation point.</param>
		/// <param name="fromLeft">If true cut the beginning of the string when the first occurrence of the character is located from the left, 
		/// otherwise cut the end of the string when the first occurrence of the character is located from the end.</param>
		/// <returns>Truncated string if cutting length is shorter than the original string, otherwise an empty string is returned.</returns>
		public static String Cut(this String value, Char firstOccuranceOf, Boolean fromLeft = true)
		{
			if (value.IsEmpty()) return value;
			var truncationPoint = fromLeft ? value.IndexOf(firstOccuranceOf) : value.LastIndexOf(firstOccuranceOf);
			return truncationPoint == -1 ? String.Empty :
				fromLeft ? value.Substring(truncationPoint + 1) : value.Substring(0, truncationPoint);
		}

		/// <summary>
		/// Truncate a string by a given length.
		/// </summary>
		/// <param name="value">String to truncate.</param>
		/// <param name="cuttingLength">Number of characters to remove from the end of the string.</param>
		/// <returns>Truncated string if cutting length is shorter than the original string, otherwise an empty string is returned.</returns>
		public static String CutEnd(this String value, Int32 cuttingLength)
		{
			Contract.Requires(cuttingLength > 0);

			if (value.IsEmpty()) return value;
			return (cuttingLength >= value.Length) ? String.Empty : value.Substring(0, value.Length - cuttingLength);
		}

		/// <summary>
		/// Get the certain left most number of characters from a given string.
		/// </summary>
		/// <param name="value">String from which to extract a sub-string.</param>
		/// <param name="length">The left most number of characters required.</param>
		/// <returns>A string of <paramref name="length"/> characters if all arguments are valid, else an empty string is returned.</returns>
		[DebuggerStepThrough]
		public static String Left(this String value, Int32 length)
		{
			return SubstringSafe(value, 0, length);
		}

		/// <summary>
		/// Get the certain right most number of characters from a given string.
		/// </summary>
		/// <param name="value">String from which to extract a sub-string.</param>
		/// <param name="length">The right most number of characters required.</param>
		/// <returns>A string of <paramref name="length"/> characters if all arguments are valid, else an empty string is returned.</returns>
		[DebuggerStepThrough]
		public static String Right(this String value, Int32 length)
		{
			return SubstringSafe(value, value.Length - length, length);
		}

		/// <summary>
		/// Get a sub-string from within a given string.
		/// </summary>
		/// <param name="input">String from which to extract a sub-string.</param>
		/// <param name="startIndex">The first character position of the given string to extract from.</param>
		/// <returns>A string of <paramref name="startIndex"/> characters if all arguments are valid, else an empty string is returned.</returns>
		[DebuggerStepThrough]
		public static String SubstringSafe(this String input, Int32 startIndex)
		{
			return SubstringCore(input, startIndex, input.Length);
		}

		/// <summary>
		/// Get a sub-string from within a given string.
		/// </summary>
		/// <param name="input">String from which to extract a sub-string.</param>
		/// <param name="startIndex">The first character position of the given string to extract from.</param>
		/// <param name="length">The number of characters required.</param>
		/// <returns>A string of <paramref name="length"/> characters if all arguments are valid, else an empty string is returned.</returns>
		[DebuggerStepThrough]
		public static String SubstringSafe(this String input, Int32 startIndex, Int32 length)
		{
			return SubstringCore(input, startIndex, length);
		}

		/// <summary>
		/// A sub-string of this instance, from the specified position (empty if beyond the end of the string), up to the specified maximum length.
		/// </summary>
		private static String SubstringCore(String input, Int32 startIndex, Int32 length)
		{
			var result = String.Empty;
			if (input == null)
			{
				return String.Empty;
			}

			if (startIndex < 0 || length < 0)
			{
				return input;
			}

			if (startIndex < input.Length && length > 0)
			{
				Int32 internalStartIndex, lSafeLength;
				internalStartIndex = Math.Min(startIndex, input.Length - 1);
				lSafeLength = Math.Min(Math.Min(input.Length, input.Length - internalStartIndex), length);
				result = input.Substring(startIndex, lSafeLength);
			}
			return result;
		}

		public static KeyValuePairs ToKeyValuePairs(this String value, Char pairDelimiter, Char keyValueDelimiter, Boolean caseInsensitive = false)
		{
			if (value.IsEmpty()) throw new ArgumentException("primary value cannot be empty", "primary");
			if (pairDelimiter == keyValueDelimiter) throw new ArgumentException("pairDelimiter and keyValueDelimiter cannot be the same value");

			return new KeyValuePairs(value, pairDelimiter, keyValueDelimiter, caseInsensitive);
		}

		public static String ToValidFileName(this String fileName)
		{
			var invalidChars = new String(Path.GetInvalidFileNameChars());
			foreach (var invalidChar in invalidChars)
			{
				fileName = fileName.Replace(invalidChar, '_');
			}
			return fileName;
		}

		/// <summary>
		/// Get an NON NULL value by returning the final value if the primary value is null.
		/// </summary>
		/// <param name="nullableValue">A nullable first preference integer.</param>
		/// <param name="replacementValue">The absolute non nullable integer.</param>
		/// <returns>A first or second preference integer that is NOT null.</returns>
		/// <remarks>Simply nest this method call if you want multiple fall backs like a tertiary 
		/// preference eg: FallbackOnNull(firstPreint?, FallbackOnNull(seondPreint?, finalInt)) </remarks>
		public static String ValueOnNull(this String nullableValue, String replacementValue)
		{
			return nullableValue != null ? nullableValue : replacementValue;
		}

		#region ToDateTime

		/// <summary>
		/// Exception protected String to DateTime conversion.
		/// </summary>
		/// <param name="value">Given date time value as a string.</param>
		/// <param name="dateTimeFormat">Optional DateTime format string.</param>
		/// <returns>Either the correctly converted DateTime or the DateTime.MinValue if the given value is invalid in any way.</returns>
		public static DateTime ToDateTime(this String value, String dateTimeFormat = null)
		{
			DateTime result = DateTime.MinValue;
			if (!value.IsEmpty())
			{
				if (dateTimeFormat.IsEmpty())
				{
					TryParse(value, out result);
				}
				else
				{
					ParseKnownStringToDateTime(value, ref result, dateTimeFormat);
				}
			}
			return result;
		}

		/// <summary>
		/// Exception protected String to DateTime conversion and indicates whether the conversion was successful.
		/// </summary>
		/// <param name="value">Given date time value as a string.</param>
		/// <param name="result">Either the correctly converted DateTime or the DateTime.MinValue if the given value is invalid in any way.</param>
		/// <returns>Whether the conversion was successful.</returns>
		private static Boolean TryParse(String value, out DateTime result)
		{
			var successful = false;
			DateTime lInnerResult = DateTime.MinValue;
			try
			{
				successful = DateTime.TryParse(value, out lInnerResult);
			}
			catch (ArithmeticException)
			{
			}
			finally
			{
				result = lInnerResult;
			}

			if (!successful)
			{
				successful = ParseKnownStringToDateTime(value, ref result);
			}
			return successful;
		}

		private static Boolean ParseKnownStringToDateTime(String sourceDateTime, ref DateTime result, String dateTimeFormat = null)
		{
			var isParsed = false;
			try
			{
				String[] supportedDateTimeFormats;
				if (dateTimeFormat == null)
				{
					supportedDateTimeFormats = new[] { "dd/MM/yyyy hh:mm tt", "dd/MM/yyyy hh:mm:ss tt", "d/MM/yyyy hh:mm:ss tt" };
				}
				else
				{
					supportedDateTimeFormats = new[] { dateTimeFormat };
				}

				result = DateTime.ParseExact(sourceDateTime, supportedDateTimeFormats, null, DateTimeStyles.None);
				isParsed = true;
			}
			catch (FormatException) { }
			catch (InvalidCastException) { }
			return isParsed;
		}

		#endregion

		#region Padding

		/// <summary>
		/// Left pads the passed String using the passed pad String for the total number of spaces. 
		/// It will not cut-off the pad even if it causes the String to exceed the total width.
		/// </summary>
		/// <param name="totalWidth">The total width of the resulting String</param>
		/// <returns>Copy of String with the padding applied</returns>
		/// <remarks>Allows a padding String, as opposed to a padding character.</remarks>
		public static String AddStart(this String value, Int32 totalWidth)
		{
			return AddPadLeftCore(value, totalWidth, ValueLib.Space.StringValue, false);
		}

		/// <summary>
		/// Left pads the passed String using the passed pad String for the total number of spaces. 
		/// It will not cut-off the pad even if it causes the String to exceed the total width.
		/// </summary>
		/// <param name="add">The pad String</param>
		/// <param name="totalWidth">The total width of the resulting String</param>
		/// <returns>Copy of String with the padding applied</returns>
		/// <remarks>Allows a padding String, as opposed to a padding character.</remarks>
		public static String AddStart(this String value, Int32 totalWidth, String add)
		{
			return AddPadLeftCore(value, totalWidth, add, false);
		}

		public static String PadCentre(this String value, Int32 totalWidth, Char pad = ' ')
		{
			value = value.Left(totalWidth);  // Ensure total length of value doesn't extend beyond totalWidth.
			var leftPadding = Math.Round((Decimal)(totalWidth / 2)).ToInt() - Math.Round((Decimal)(value.Length / 2)).ToInt();
			return new String(pad, leftPadding) + value;
		}


		/// <summary>
		/// Left pads the passed String using the passed pad String for the total number of spaces. 
		/// </summary>
		/// <param name="totalWidth">The total width of the resulting String</param>
		/// <returns>Copy of String with the padding applied</returns>
		/// <remarks>Allows a padding String, as opposed to a padding character.</remarks>
		public static String PadLeft(this String value, Int32 totalWidth)
		{
			return AddPadLeftCore(value, totalWidth, ValueLib.Space.StringValue, true);
		}

		/// <summary>
		/// Left pads the passed String using the passed pad String for the total number of spaces. 
		/// </summary>
		/// <param name="pad">The pad String</param>
		/// <param name="totalWidth">The total width of the resulting String</param>
		/// <returns>Copy of String with the padding applied</returns>
		/// <remarks>Allows a padding String, as opposed to a padding character.</remarks>
		public static String PadLeft(this String value, Int32 totalWidth, Char pad)
		{
			return AddPadLeftCore(value, totalWidth, pad.ToString(), true);
		}

		/// <summary>
		/// Left pads the passed String using the passed pad String for the total number of spaces. 
		/// </summary>
		/// <param name="pad">The pad String</param>
		/// <param name="totalWidth">The total width of the resulting String</param>
		/// <returns>Copy of String with the padding applied</returns>
		/// <remarks>Allows a padding String, as opposed to a padding character.</remarks>
		public static String PadLeft(this String value, Int32 totalWidth, String pad)
		{
			return AddPadLeftCore(value, totalWidth, pad, true);
		}

		/// <summary>
		/// Left pads the passed String using the passed pad String for the total number of spaces. 
		/// </summary>
		/// <param name="addPad">The pad String</param>
		/// <param name="totalWidth">The total width of the resulting String</param>
		/// <param name="cutOff">True to cut off the characters if exceeds the specified width.</param>
		/// <returns>Copy of String with the padding applied</returns>
		/// <remarks>Allows a padding String, as opposed to a padding character.</remarks>
		private static String AddPadLeftCore(String value, Int32 totalWidth, String addPad, Boolean cutOff)
		{
			var result = String.Empty;
			if (value.Length >= totalWidth)
			{
				return value;
			}

			var lAddPaddedString = new StringBuilder(totalWidth);
			lAddPaddedString.Append(value);

			while (lAddPaddedString.Length < totalWidth)
			{
				lAddPaddedString.Append(addPad);
			}

			if (cutOff)
			{
				result = (lAddPaddedString.ToString()).Substring(0, totalWidth);
			}
			else
			{
				result = lAddPaddedString.ToString();
			}
			return result;
		}

		/// <summary>
		/// Right pads the passed String using the passed pad String for the total number of spaces. 
		/// It will not cut-off the pad even if it causes the String to exceed the total width.
		/// </summary>
		/// <param name="aAdd">The pad String</param>
		/// <param name="totalWidth">The total width of the resulting String</param>
		/// <returns>Copy of String with the padding applied</returns>
		/// <remarks>Allows a padding String, as opposed to a padding character.</remarks>
		public static String AddEnd(this String value, Int32 totalWidth)
		{
			return AddPadRightCore(value, ValueLib.Space.StringValue, totalWidth, false);
		}

		/// <summary>
		/// Right pads the passed String using the passed pad String for the total number of spaces. 
		/// It will not cut-off the pad even if it causes the String to exceed the total width.
		/// </summary>
		/// <param name="add">The pad String</param>
		/// <param name="totalWidth">The total width of the resulting String</param>
		/// <returns>Copy of String with the padding applied</returns>
		/// <remarks>Allows a padding String, as opposed to a padding character.</remarks>
		public static String AddEnd(this String value, Int32 totalWidth, String add)
		{
			return AddPadRightCore(value, add, totalWidth, false);
		}

		/// <summary>
		/// Right pads the passed String using the passed pad String for the total number of spaces. 
		/// </summary>
		/// <param name="totalWidth">The total width of the resulting String</param>
		/// <returns>Copy of String with the padding applied</returns>
		/// <remarks>Allows a padding String, as opposed to a padding character.</remarks>
		public static String PadRight(this String value, Int32 totalWidth)
		{
			return AddPadRightCore(value, ValueLib.Space.StringValue, totalWidth, true);
		}

		/// <summary>
		/// Right pads the passed String using the passed pad String for the total number of spaces. 
		/// </summary>
		/// <param name="pad">The pad String</param>
		/// <param name="totalWidth">The total width of the resulting String</param>
		/// <returns>Copy of String with the padding applied</returns>
		/// <remarks>Allows a padding String, as opposed to a padding character.</remarks>
		public static String PadRight(this String value, Int32 totalWidth, Char pad)
		{
			return AddPadRightCore(value, pad.ToString(), totalWidth, true);
		}

		/// <summary>
		/// Right pads the passed String using the passed pad String for the total number of spaces. 
		/// </summary>
		/// <param name="pad">The pad String</param>
		/// <param name="totalWidth">The total width of the resulting String</param>
		/// <returns>Copy of String with the padding applied</returns>
		/// <remarks>Allows a padding String, as opposed to a padding character.</remarks>
		public static String PadRight(this String value, Int32 totalWidth, String pad)
		{
			return AddPadRightCore(value, pad, totalWidth, true);
		}

		/// <summary>
		/// Right pads the passed String using the passed pad String for the total number of spaces. 
		/// </summary>
		/// <param name="aAddPad">The pad String</param>
		/// <param name="totalWidth">The total width of the resulting String</param>
		/// <param name="aCutOff">True to cut off the characters if exceeds the specified width</param>
		/// <returns>Copy of String with the padding applied</returns>
		/// <remarks>Allows a padding String, as opposed to a padding character.</remarks>
		private static String AddPadRightCore(String value, String aAddPad, Int32 totalWidth, Boolean aCutOff)
		{
			var result = String.Empty;
			if (value.Length >= totalWidth)
			{
				return value;
			}

			var lAddPaddedString = new StringBuilder(totalWidth);
			while (lAddPaddedString.Length < totalWidth - value.Length)
			{
				lAddPaddedString.Append(aAddPad);
			}

			if (aCutOff)
			{
				result = (lAddPaddedString.ToString()).Substring(0, totalWidth - value.Length);
			}
			else
			{
				result = lAddPaddedString.ToString();
			}
			return (result += value);
		}

		#endregion

		public static Boolean Contains(this String value, Char searchValue)
		{
			return value.IndexOf(searchValue) != -1;
		}

		public static Boolean Contains(this String value, String searchValue, StringComparison stringComparison)
		{
			return value.IndexOf(searchValue, stringComparison) >= 0;
		}

		public static Boolean ContainsAny(this String value, params String[] searchValues)
		{
			var result = false;
			searchValues.ForEach(sv => result |= value.Contains(sv));
			return result;
		}

		public static Boolean ContainsAnyChar(this String value, String charValueacters)
		{
			Contract.Assert(!charValueacters.IsEmpty());

			foreach (var lChar in charValueacters)
			{
				if (value.Contains(lChar))
				{
					return true;
				}
			}
			return false;
		}

		public static Boolean ContainsAnyLetters(this String value)
		{
			foreach (var c in value)
			{
				if (Char.IsLetter(c))
				{
					return true;
				}
			}

			return false;
		}

		public static String KeepChars(this String value, String charValueactersToKeep)
		{
			return KeepChars(value, String.Empty);
		}

		public static String KeepChars(this String value, String charValueactersToKeep, String replaceCharacter)
		{
			var result = new StringBuilder(value.Length);

			foreach (var c in value)
			{
				if (charValueactersToKeep.Contains(c))
				{
					result.Append(c);
				}
				else
				{
					result.Append(replaceCharacter);
				}
			}
			return result.ToString();
		}

		public static String KeepCharsUntil(this String value, String charValueactersToKeep, Char[] charactersToStopOnEncountering)
		{
			var lIndexOfChars = value.IndexOfAny(charactersToStopOnEncountering);

			if (lIndexOfChars < 0)
			{
				return KeepChars(value, charValueactersToKeep);
			}
			else
			{
				return value.Substring(0, lIndexOfChars).KeepChars(charValueactersToKeep);
			}
		}

		/// <summary>
		/// KeepLeft part of string up until a given character is found.
		/// </summary>
		/// <param name="value">The string of which you wish to keep a left part of.</param>
		/// <param name="upToInstanceOfChar">The character from which you wish to ignore.</param>
		/// <returns>The given string or a shortened version if the <paramref name="upToInstanceOfChar"/> character exists.</returns>
		public static String KeepLeft(this String value, Char upToInstanceOfChar)
		{
			return value.Left(value.IndexOf(upToInstanceOfChar));
		}

		public static String ExcludeChars(this String value, String charValueactersToExclude)
		{
			var result = new StringBuilder(value.Length);

			foreach (var c in value)
			{
				if (!charValueactersToExclude.Contains(c)) result.Append(c);
			}

			return (result.ToString()).ToString(Factory.CurrentCultureInfo);
		}

		/// <summary>     
		/// Returns the number of lines in a string     
		/// </summary>     
		/// <param name="source">Source string</param>     
		/// <param name="searchChar">Character to search</param>     
		/// <returns>Number of times the searchChar resides in the source string.</returns>     
		public static Int64 Occurrences(this String source, Char[] searchChar)
		{
			Int64 count = 0;
			searchChar.ForEach(c => count += Occurrences(source, c));
			return count;
		}

		/// <summary>     
		/// Returns the number of times searchChar appears in source string     
		/// </summary>     
		/// <param name="source">Source string</param>     
		/// <param name="searchChar">Character to search</param>     
		/// <returns>Number of times the searchChar resides in the source string.</returns>     
		public static Int64 Occurrences(this String source, Char searchChar)
		{
			Int64 count = 0;
			var position = 0;
			while ((position = source.IndexOf(searchChar, position)) != -1)
			{
				count++;
				position++;         // Skip this occurrence!             
			}
			return count;
		}

		/// <summary>
		/// The number of times a given string exists within this string (case sensitive). "aaA".Occurences("aa") will return 1.
		/// </summary>
		public static Int32 Occurrences(this String value, String text)
		{
			return Regex.Matches(value, Regex.Escape(text)).Count;
		}

		/// <summary>
		/// The number of times a given string exists within this string, ignoring case. "aaA".Occurences("aa") will return 2.
		/// </summary>
		public static Int32 OccurrencesIgnoringCase(this String value, String text)
		{
			return Regex.Matches(value, Regex.Escape(text), RegexOptions.IgnoreCase).Count;
		}

		/// <summary>
		/// Split a string by a delimiter, but ignoring cases where the delimiter is escaped by another character.
		/// </summary>
		public static IList<String> SplitIgnoringEscapedDelimiter(this String value, Char delimiter, Char escapeCharacter)
		{
			return SplitIgnoringEscapedDelimiter(value, delimiter, escapeCharacter, false);
		}

		/// <summary>
		/// Split a string by a delimiter, but ignoring cases where the delimiter is escaped by another character.
		/// </summary>
		public static IList<String> SplitIgnoringEscapedDelimiter(this String value, Char delimiter, Char escapeCharacter, Boolean leaveSplitCharacter)
		{
			var result = new List<String>();

			var startPos = 0;
			for (var i = 0; i < value.Length; i++)
			{
				var currentCharacter = value[i];

				if (currentCharacter == escapeCharacter)
				{
					i++;
				}
				else if (currentCharacter == delimiter)
				{
					var splittedString = SubstringSafe(value, startPos, (i - startPos));
					if (leaveSplitCharacter)
					{
						splittedString += delimiter;
					}
					result.Add(splittedString);
					startPos = i + 1;
				}
			}
			var remainingString = SubstringSafe(value, startPos);
			if (remainingString.Length > 0)
			{
				result.Add(remainingString);
			}
			return result.ToArray();
		}

		private static String ToTitleCaseCore(this String value, Boolean ignoreShortWords)
		{
			if (value.Length == 0) return String.Empty;

			Words ignoreWords = null;
			if (ignoreShortWords)
			{
				var current = new AssemblyInfo(Assembly.GetEntryAssembly());
				using (TextReader reader = new StreamReader(current.GetEmbeddedResourceStream(Factory.CurrentCultureInfo.ResourceForShortNoTitleCaseWords())))
				{
					ignoreWords = new Words(reader.ReadToEnd(), Environment.NewLine);
				}
			}

			var result = new StringBuilder(value.Length);
			var lWordCollection = new Words(value);
			foreach (var iWord in lWordCollection)
			{
				if (ignoreShortWords == true && iWord != lWordCollection[0] && ignoreWords.Contains(iWord.ToLower()))
				{
					result.Append(iWord + ValueLib.Space.StringValue);
				}
				else
				{
					result.Append(iWord[0].ToString().ToUpper());
					result.Append(iWord.SubstringSafe(1).ToLower());
					result.Append(ValueLib.Space.StringValue);
				}
			}
			return result.ToString().Trim();
		}

		/// <summary>
		/// Does this String contain only digits.
		/// </summary>
		public static Boolean IsNumeric(this String value)
		{
			if (String.IsNullOrWhiteSpace(value))
			{
				return false;
			}
			var idx = 0;
			var decimalPointCounter = 0;
			foreach (var c in value)
			{
				if (!Char.IsDigit(c))
				{
					if (c == '.')
					{
						decimalPointCounter++;
						if (decimalPointCounter > 1)
						{
							return false;
						}
					}
					if (idx == 0 && c != '-' && c != '.')
					{
						return false;
					}
					if (idx > 0 && c != '.')
					{
						return false;
					}
				}
				idx++;
			}
			return true;
		}

		public static Boolean IsAlphabetic(this String value)
		{
			if (String.IsNullOrWhiteSpace(value))
			{
				return false;
			}
			foreach (var c in value)
			{
				if (!Char.IsLetter(c))
				{
					return false;
				}
			}
			return true;
		}

		public static Boolean IsAlphanumeric(this String value)
		{
			foreach (var c in value)
			{
				if (!Char.IsLetterOrDigit(c))
				{
					return false;
				}
			}
			return true;
		}

		public static Boolean IsAlphanumericWithPunctuation(this String value)
		{
			foreach (var c in value)
			{
				if (!Char.IsWhiteSpace(c) &&
					 !Char.IsPunctuation(c) &&
					 !Char.IsLetterOrDigit(c) &&
					 !Char.IsSeparator(c) &&
					 !Char.IsSymbol(c) &&
					 c != '\r' &&
					 c != '\n')
				{
					return false;
				}
			}

			return true;
		}

		/// <summary>
		/// Keeps ABCDEFGHIJKLMNOPQRSTUVWXYZ abcdefghijklmnopqrstuvwxyz 0123456789 (including space)
		/// </summary>
		/// <returns></returns>
		public static String KeepAlphanumericCharacters(this String value)
		{
			return KeepCharacters(value, c => Char.IsLetterOrDigit(c) || c == ' ');
		}

		/// <summary>
		/// Keeps digits: char.IsDigit returns true
		/// </summary>
		/// <returns></returns>
		public static String KeepNumericCharacters(this String value)
		{
			return KeepCharacters(value, Char.IsDigit);
		}

		/// <summary>
		/// Keeps chars according to the isOkToKeep's return value
		/// </summary>
		/// <param name="value"></param>
		/// <param name="isOkToKeep">return true if want to keep the char otherwise false</param>
		/// <returns></returns>
		public static String KeepCharacters(this String value, Func<Char, Boolean> isOkToKeep)
		{
			var buffer = value.ToCharArray();
			var insertPos = 0;
			for (var i = 0; i < buffer.Length; i++)
			{
				if (isOkToKeep(buffer[i]))
				{
					buffer[insertPos++] = buffer[i];
				}
			}
			return insertPos == buffer.Length ? value : new String(buffer, 0, insertPos);
		}

		/// <summary>
		/// Keeps ABCDEFGHIJKLMNOPQRSTUVWXYZ abcdefghijklmnopqrstuvwxyz
		/// </summary>
		/// <returns></returns>
		public static String KeepAlphabeticCharacters(this String value)
		{
			return KeepCharacters(value, Char.IsLetter);
			//return KeepChars(value, AlphabeticCharacters);
		}

		public static readonly String NumericCharacters = "0123456789";
		public static readonly String AlphabeticCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
		public static readonly String AlphanumericCharacters = AlphabeticCharacters + NumericCharacters;

		#region Words

		/// <summary>
		/// Extrapolate the first word in current string.
		/// </summary>
		/// <returns>Returns string of the first word from this string assuming the words are delimited by spaces.</returns>
		public static String Word(this String value)
		{
			return Word(value, ValueLib.Space.StringValue, 1, 1);
		}

		/// <summary>
		/// Extrapolate words in current string.
		/// </summary>
		/// <param name="positionOfWord">Position of word to be returned.  Must be 1 or greater.</param>
		/// <returns>Returns a string of a word at the given position from this string assuming the words are delimited by spaces.</returns>
		public static String Word(this String value, Int32 positionOfWord)
		{
			return Word(value, ValueLib.Space.StringValue, positionOfWord, 1);
		}

		/// <summary>
		/// Extrapolate words in current string.
		/// </summary>
		/// <param name="positionOfFirstWord">Position of first word to be returned.  Must be 1 or greater.</param>
		/// <param name="wordCount">Number of words to return.</param>
		/// <returns>Returns a string of a given number of words starting from the given position of the first word assuming the words are delimited
		/// by spaces.</returns>
		public static String Word(this String value, Int32 positionOfFirstWord, Int32 wordCount)
		{
			return Word(value, ValueLib.Space.StringValue, positionOfFirstWord, wordCount);
		}

		/// <summary>
		/// Extrapolate words in current string.
		/// </summary>
		/// <param name="delimiter">A string of characters that separate the words, usually a space.</param>
		/// <param name="positionOfFirstWord">Position of first word to be returned.  Must be 1 or greater.</param>
		/// <param name="wordCount">Number of words to return. If -1, then all words from the position of the first word will be returned.</param>
		/// <returns>Returns a string of a given number of words starting from the given position of the first word from a given delimited string.</returns>
		/// <modified Date="3 August 2009">Created.</modified>
		/// <modified Date="15 October 2009">Added post contracts.</modified>
		public static String Word(this String value, String delimiter, Int32 positionOfFirstWord, Int32 wordCount)
		{
			Contract.Requires(positionOfFirstWord > 1);
			Contract.Requires(wordCount >= -1);

			var result = String.Empty;
			if (String.IsNullOrEmpty(value) || wordCount.IsEmpty())
			{
				return result;
			}

			if (delimiter.IsEmpty())
			{
				delimiter = ValueLib.Space.StringValue;
			}

			var words = new Words(value.Split(delimiter.ToCharArray()));
			if (wordCount.Equals(-1))
			{
				wordCount = words.Count;
			}
			if (positionOfFirstWord <= words.Count)
			{
				positionOfFirstWord--;  // Adjust for zero based array.
				var lLastField = (positionOfFirstWord + wordCount).LimitMax(words.Count);
				for (var iIdx = positionOfFirstWord; iIdx < lLastField; iIdx++)
				{
					result += delimiter + words[iIdx];
				}
				result = result.Substring(delimiter.Length);
			}
			return result;
		}

		/// <summary>
		/// Extrapolate the last word of a space string like a sentence.
		/// </summary>
		/// <param name="value">Primary delimited string on which to extrapolate words from.</param>
		/// <returns>Returns the last word of the string.</returns>
		public static String LastWord(this String value)
		{
			return LastWord(value, ValueLib.Space.StringValue);
		}

		/// <summary>
		/// Extrapolate the last word of a delimited string like a sentence.
		/// </summary>
		/// <param name="value">Primary delimited string on which to extrapolate words from.</param>
		/// <param name="delimiter">A string of characters that separate the words, usually a space.</param>
		/// <returns>Returns the last word of the string.</returns>
		public static String LastWord(this String value, String delimiter)
		{
			var words = new Words(value, delimiter, 1, -1);
			return words.Count > 0 ? words[words.Count - 1] : String.Empty;
		}

		/// <summary>
		/// Removes occurrences of words in a string
		/// The match is case sensitive
		/// </summary>
		/// <param name="filterWords">Array of words to be removed from the string</param>
		/// <returns>Copy of the string with the words removed</returns>
		public static String RemoveWords(this String value, params String[] filterWords)
		{
			return MaskWords(value, Char.MinValue, filterWords);
		}

		/// <summary>
		/// Masks the occurrence of words in a string with a given character
		/// </summary>
		/// <param name="mask">The character mask to apply</param>
		/// <param name="filterWords">The words to be replaced</param>
		/// <returns>The copy of string with the mask applied</returns>
		public static String MaskWords(this String value, Char mask, params String[] filterWords)
		{
			Contract.Requires(filterWords != null);

			var result = value;
			var stringMask = mask == Char.MinValue ? String.Empty : mask.ToString();
			var totalMask = stringMask;

			foreach (var iFilterWord in filterWords)
			{
				var lRegEx = new Regex(iFilterWord, RegexOptions.IgnoreCase | RegexOptions.Multiline);
				if (stringMask.Length > 0)
				{
					for (var i = 1; i < iFilterWord.Length; i++)
						totalMask += stringMask;
				}

				result = lRegEx.Replace(result, totalMask);
				totalMask = stringMask;
			}
			return result;
		}

		#endregion

		/// <summary>
		/// Ensure any original upper case characters are retained.
		/// </summary>
		/// <param name="translatedVersion"></param>
		/// <param name="originalVersion"></param>
		/// <returns></returns>
		public static String MatchCase(this String translatedVersion, String originalVersion)
		{
			if (translatedVersion.Equals(originalVersion))
			{
				return translatedVersion;
			}

			var tvLength = translatedVersion.Length;
			var ovLength = originalVersion.Length;
			var matchVersionBuilder = new StringBuilder(tvLength);
			var oIdx = 0;
			for (var idx = 0; idx < tvLength; idx++)
			{
				var transalatedVersionCharacter = translatedVersion[idx];
				var originalVersionCharacter = ' ';  // Just a default buffer value - not actually used.
				while (oIdx < ovLength)
				{
					originalVersionCharacter = originalVersion[oIdx];

					if (originalVersionCharacter.In(' ', '_'))
					{
						oIdx++;
						continue;
					}

					var ovAsString = originalVersionCharacter.ToString();
					var tvAsString = transalatedVersionCharacter.ToString();
					if (!ovAsString.Equals(tvAsString, StringComparison.OrdinalIgnoreCase))
					{
						if (oIdx + 1 < ovLength - 1 && originalVersion[oIdx + 1].ToString().Equals(tvAsString, StringComparison.OrdinalIgnoreCase))
						{
							oIdx++;
							continue;
						}

						if (oIdx + 2 < ovLength - 2 && originalVersion[oIdx + 2].ToString().Equals(tvAsString, StringComparison.OrdinalIgnoreCase))
						{
							oIdx += 2;
							continue;
						}

						idx = tvLength;
					}
					break;
				};

				if (Char.IsLower(transalatedVersionCharacter) && Char.IsUpper(originalVersionCharacter))
				{
					matchVersionBuilder.Append(originalVersionCharacter.ToString().ToUpper());
				}
				else
				{
					matchVersionBuilder.Append(transalatedVersionCharacter);
				}
				oIdx++;
			}
			return matchVersionBuilder.ToString();
		}

		/// <summary>
		/// Divide a very long string by a given length and return a given sized list.
		/// </summary>
		/// <param name="value">The very long string.</param>
		/// <param name="elementCount">Maximum number of elements to return.</param>
		/// <param name="length">Maximum length of each element.</param>
		/// <returns></returns>
		public static IList<String> Divide(this String value, Int16 elementCount, Int16 length)
		{
			Contract.Requires(elementCount > 0);

			var internalValue = value;
			var result = new List<String>(elementCount);
			for (var iIdx = 0; iIdx < elementCount; iIdx++)
			{
				result.Add(internalValue.Substring(0, length));
				internalValue = internalValue.Remove(0, length);
			}
			return result;
		}

		#region Surrounding

		public static Boolean IsSurroundedBy(this String value, params ESurroundType[] surroundTypes)
		{
			var result = false;
			if (!value.IsEmpty() && value.Length > 1 && surroundTypes != null)
			{
				var firstChar = value[0];
				var lastChar = value[value.Length - 1];

				foreach (ESurroundType surroundType in surroundTypes)
				{
					switch (surroundType)
					{
						case ESurroundType.SingleQuote:
							result = firstChar.Equals('\'') && lastChar.Equals('\'');
							break;

						case ESurroundType.DoubleQuote:
							result = firstChar.Equals('"') && lastChar.Equals('"');
							break;

						case ESurroundType.Braces:
							result = firstChar.Equals('{') && lastChar.Equals('}');
							break;

						case ESurroundType.SquareBrackets:
							result = firstChar.Equals('[') && lastChar.Equals(']');
							break;

						case ESurroundType.RoundBrackets:
							result = firstChar.Equals('(') && lastChar.Equals(')');
							break;

						case ESurroundType.AngleBrackets:
							result = firstChar.Equals('<') && lastChar.Equals('>');
							break;

						case ESurroundType.DoubleAngleBrackets:
							result = firstChar.Equals('«') && lastChar.Equals('»');
							break;

						default:
							break;
					}
				}
			}
			return result;
		}

		public static String SurroundSQuote(this String value)
		{
			return Surround(ValueLib.DoubleQuotes.StringValue, ValueLib.DoubleQuotes.StringValue);
		}

		public static String SurroundDQuote(this String value)
		{
			return Surround(ValueLib.SingleQuotes.StringValue, ValueLib.SingleQuotes.StringValue);
		}

		public static String SurroundBy(this String value, ESurroundType surroundType)
		{
			switch (surroundType)
			{
				case ESurroundType.SingleQuote:
					return Surround(value, "\'", "\'");

				case ESurroundType.DoubleQuote:
					return Surround(value, "\"", "\"");

				case ESurroundType.Braces:
					return Surround(value, "{", "}");

				case ESurroundType.SquareBrackets:
					return Surround(value, "[", "]");

				case ESurroundType.RoundBrackets:
					return Surround(value, "(", ")");

				case ESurroundType.AngleBrackets:
					return Surround(value, "<", ">");

				case ESurroundType.DoubleAngleBrackets:
					return Surround(value, "«", "»");

				default:
					return value;
			}
		}

		public static String Surround(this String value, String surroundValue)
		{
			return Surround(value, surroundValue, surroundValue);
		}

		public static String Surround(this String value, String startSurroundValue, String endSurroundValue)
		{
			return startSurroundValue + value + endSurroundValue;
		}

		public static String Desurround(this String value)
		{
			return value.Substring(1, value.Length - 2);
		}

		#endregion

		#region Replace

		/// <summary>
		/// Replaces a given character with another character in a string. 
		/// The replacement is case insensitive
		/// </summary>
		/// <param name="val"></param>
		/// <param name="charToReplace">The character to replace</param>
		/// <param name="replacement">The character by which to be replaced</param>
		/// <returns>Copy of string with the characters replaced</returns>
		public static String ReplaceCaseInsenstive(this String value, Char charValueToReplace, Char replacement)
		{
			var lRegEx = new Regex(charValueToReplace.ToString(), RegexOptions.IgnoreCase | RegexOptions.Multiline);
			return lRegEx.Replace(value, replacement.ToString());
		}

		/// <summary>
		/// Replaces a given String with another String in a given String. 
		/// The replacement is case insensitive
		/// </summary>
		/// <param name="val"></param>
		/// <param name="stringToReplace">The String to replace</param>
		/// <param name="replacement">The String by which to be replaced</param>
		/// <returns>Copy of String with the String replaced</returns>
		public static String ReplaceCaseInsenstive(this String value, String stringToReplace, String replacement)
		{
			var regEx = new Regex(stringToReplace, RegexOptions.IgnoreCase | RegexOptions.Multiline);
			return regEx.Replace(value, replacement);
		}

		/// <summary>
		/// Replaces the first occurrence of a String with another String in a given String
		/// The replacement is case insensitive
		/// </summary>
		/// <param name="val"></param>
		/// <param name="stringToReplace">The String to replace</param>
		/// <param name="replacement">The String by which to be replaced</param>
		/// <returns>Copy of String with the String replaced</returns>
		public static String ReplaceFirstRegex(this String value, String stringToReplace, String replacement)
		{
			var regEx = new Regex(stringToReplace, RegexOptions.Multiline);
			return regEx.Replace(value, replacement, 1);
		}

		/// <summary>
		/// Replaces the first occurrence of a character with another character in a given String
		/// The replacement is case insensitive
		/// </summary>
		/// <param name="val"></param>
		/// <param name="charToReplace">The character to replace</param>
		/// <param name="replacement">The character by which to replace</param>
		/// <returns>Copy of String with the character replaced</returns>
		public static String ReplaceFirstRegex(this String value, Char charValueToReplace, Char replacement)
		{
			var regEx = new Regex(charValueToReplace.ToString(), RegexOptions.Multiline);
			return regEx.Replace(value, replacement.ToString(), 1);
		}

		/// <summary>
		/// Replaces the first occurrence of a character with another character in a given String
		/// The replacement is case insensitive
		/// </summary>
		/// <param name="val"></param>
		/// <param name="stringToReplace">The text to replace</param>
		/// <param name="replacement">The text by which to replace</param>
		/// <returns>Copy of String with the character replaced</returns>
		public static String ReplaceFirst(this String value, String stringToReplace, String replacement = "")
		{
			value = value ?? String.Empty;
			stringToReplace = stringToReplace ?? String.Empty;
			replacement = replacement ?? String.Empty;

			var pos = value.IndexOf(stringToReplace);
			if (pos < 0) return value;

			if (replacement.IsEmpty())
			{
				return value.Substring(0, pos) + value.Substring(pos + stringToReplace.Length);
			}
			else
			{
				return value.Substring(0, pos) + replacement + value.Substring(pos + stringToReplace.Length);
			}
		}

		/// <summary>
		/// Remove all the given characters (needles) from the source string (haystack).
		/// </summary>
		/// <param name="haystack">Source string.</param>
		/// <param name="needles">Characters to find and remove.</param>
		/// <returns>The source string if no needles found or the source string sans any needle characters.</returns>
		public static String Remove(this String haystack, Char[] needles)
		{
			if (!haystack.IsEmpty() && !needles.IsEmpty())
			{
				foreach (var needle in needles)
				{
					haystack = haystack.ReplaceAll(needle.ToString(), String.Empty);
				}
			}
			return haystack;
		}

		/// <summary>
		/// Replace all the given characters (needles) with the replacement character from the source string (haystack).
		/// </summary>
		/// <param name="haystack">Source string.</param>
		/// <param name="needles">Characters to find and replace.</param>
		/// <param name="replacement">Replace character for all the needles.</param>
		/// <returns>The source string if no needles found or the source string with replacement characters instead of any needle characters.</returns>
		public static String Replace(this String haystack, Char[] needles, Char replacement)
		{
			var result = haystack;
			needles.ForEach(n => result = result.Replace(n, replacement));
			return result;
		}

		/// <summary>
		/// Replace all the given strings (needles) with the optional replacement string from the source string (haystack).
		/// </summary>
		/// <param name="haystack">Source string.</param>
		/// <param name="needles">Strings to find and replace.</param>
		/// <param name="replacement">Optional replacement string.  Empty string by default.</param>
		/// <returns>The source string if no needles found or the source string with replacement/default string instead of any needle strings.</returns>
		public static String ReplaceAll(this String haystack, IEnumerable<String> needles, String replacement = "")
		{
			if (!haystack.IsEmpty() && !needles.IsEmpty())
			{
				foreach (var needle in needles)
				{
					haystack = haystack.ReplaceAll(needle, replacement);
				}
			}
			return haystack;
		}

		public static String ReplaceAll(this String haystack, String needle, String replacement = "")
		{
			Int32 pos;
			// Avoid a possible infinite loop
			if (needle == replacement) return haystack;
			while ((pos = haystack.IndexOf(needle)) > -1)
			{
				haystack = haystack.Substring(0, pos) + replacement + haystack.Substring(pos + needle.Length);
			}
			return haystack;
		}

		/// <summary>
		/// Replaces the last occurrence of a character with another character in a given String
		/// The replacement is case insensitive
		/// </summary>
		/// <param name="val"></param>
		/// <param name="charToReplace">The character to replace</param>
		/// <param name="replacement">The character by which to replace</param>
		/// <returns>Copy of String with the character replaced</returns>
		public static String ReplaceLast(this String value, Char charValueToReplace, Char replacement)
		{
			var index = value.LastIndexOf(charValueToReplace);
			if (index < 0)
			{
				return value;
			}
			else
			{
				var sb = new StringBuilder(value.Length - 2);
				sb.Append(value.Substring(0, index));
				sb.Append(replacement);
				sb.Append(value.Substring(index + 1, value.Length - index - 1));
				return sb.ToString();
			}
		}

		/// <summary>
		/// Replaces the last occurrence of a String with another String in a given String
		/// The replacement is case insensitive
		/// </summary>
		/// <param name="val"></param>
		/// <param name="stringToReplace">The String to replace</param>
		/// <param name="replacement">The String by which to be replaced</param>
		/// <returns>Copy of String with the String replaced</returns>
		public static String ReplaceLast(this String value, String stringToReplace, String replacement = "")
		{
			var index = value.LastIndexOf(stringToReplace);
			if (index < 0)
			{
				return value;
			}
			else
			{
				var sb = new StringBuilder(value.Length - stringToReplace.Length + replacement.Length);
				sb.Append(value.Substring(0, index));
				if (!replacement.IsEmpty())
				{
					sb.Append(replacement);
				}
				sb.Append(value.Substring(index + stringToReplace.Length,
					 value.Length - index - stringToReplace.Length));

				return sb.ToString();
			}
		}

		/// <summary>
		/// Replaces all occurrences of chars with separator. Contiguous chars are replaced with a single separator
		/// </summary>
		/// <param name="src"></param>
		/// <param name="chars"></param>
		/// <param name="separator"></param>
		/// <returns></returns>
		public static String ReplaceChars(this String src, Char[] chars, String separator = " ")
		{
			return String.Join(separator, src.Split(chars, StringSplitOptions.RemoveEmptyEntries));
		}

		#endregion

		/// <summary>
		/// Removes new line characters from a String
		/// </summary>
		/// <returns>Returns copy of String with the new line characters removed</returns>
		public static String RemoveNewLines(this String value)
		{
			return RemoveNewLines(value, false);
		}

		/// <summary>
		/// Removes new line characters from a String
		/// </summary>
		/// <param name="addSpace">True to add a space after removing a new line character</param>
		/// <returns>Returns a copy of the String after removing the new line character</returns>
		public static String RemoveNewLines(this String value, Boolean addSpace)
		{
			var lReplace = addSpace ? ValueLib.Space.StringValue : String.Empty;
			var lPattern = @"[\r|\n]";
			var lRegEx = new Regex(lPattern, RegexOptions.IgnoreCase | RegexOptions.Multiline);
			return lRegEx.Replace(value, lReplace);
		}

		/// <summary>
		/// Removes a non numeric character from a String
		/// </summary>
		/// <returns>Copy of the String after removing non numeric characters</returns>
		public static String RemoveNonNumeric(this String value)
		{
			var result = new StringBuilder();
			for (var iIdx = 0; iIdx < value.Length; iIdx++)
			{
				if (Char.IsNumber(value[iIdx]))
				{
					result.Append(value[iIdx]);
				}
			}
			return result.ToString();
		}

		/// <summary>
		/// Removes numeric characters from a given String
		/// </summary>
		/// <returns>Copy of the String after removing the numeric characters</returns>
		public static String RemoveNumeric(this String value)
		{
			var result = new StringBuilder();
			for (var iIdx = 0; iIdx < value.Length; iIdx++)
			{
				if (!Char.IsNumber(value[iIdx]))
				{
					result.Append(value[iIdx]);
				}
			}
			return result.ToString();
		}

		/// <summary>
		/// Reverses a String
		/// </summary>
		/// <returns>Copy of the reversed String</returns>
		public static String Reverse(this String value)
		{
			var lReverse = new Char[value.Length];
			for (Int32 iIdx = 0, iCountDown = value.Length - 1; iIdx < value.Length; iIdx++, iCountDown--)
			{
				if (Char.IsSurrogate(value[iCountDown]))
				{
					lReverse[iIdx + 1] = value[iCountDown--];
					lReverse[iIdx++] = value[iCountDown];
				}
				else
				{
					lReverse[iIdx] = value[iCountDown];
				}
			}
			return new String(lReverse);
		}

		/// <summary>
		/// Changes the whole String to lower case except for the first character.
		/// </summary>
		/// <returns>Copy of String with the sentence case applied</returns>
		/// <seealso cref="FirstCaps"/>
		public static String SentenceCase(this String value)
		{
			if (!value.IsEmpty())
			{
				value = value[0].ToString().ToUpper() + value.SubstringSafe(1).ToLower();
			}
			return value;
		}

		/// <summary>
		/// Changes the String as title case.
		/// </summary>
		/// <returns>Copy of String with the title case applied</returns>
		public static String ToTitleCase(this String value)
		{
			if (value.IsEmpty()) return value;
			return ToTitleCaseCore(value, true);
		}

		/// <summary>
		/// Changes the String as title case.
		/// </summary>
		/// <returns>Copy of String with the title case applied</returns>
		public static String ToTitleCaseAll(this String value)
		{
			if (value.IsEmpty()) return value;
			//return ToTitleCaseCore(value, true);
			return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(value);
		}

		/// <summary>
		/// Capitalises only the first character of the string. The rest of the string is untouched unlike what the Sentence() method does.
		/// </summary>
		/// <param name="value"></param>
		/// <returns>Only the first character of the string is capitalised leaving the rest of the string untouched.</returns>
		/// <seealso cref="SentenceCase"/>
		public static String FirstCaps(this String value)
		{
			if (!value.IsEmpty())
			{
				value = value[0].ToString().ToUpper() + value.SubstringSafe(1);
			}
			return value;
		}

		/// <summary>
		/// Lower cases only the first character of the string. The rest of the string is untouched.
		/// </summary>
		/// <param name="value"></param>
		/// <returns>Only the first character of the string is lower cased leaving the rest of the string untouched.</returns>
		/// <seealso cref="SentenceCase"/>
		public static String FirstLower(this String value)
		{
			if (!value.IsEmpty())
			{
				value = value[0].ToString().ToLower() + value.SubstringSafe(1);
			}
			return value;
		}

		public static Byte[] ToByteArray(this String value)
		{
			var result = new List<Byte>(value.Length);
			foreach (var lChar in value.ToCharArray())
			{
				result.Add(Convert.ToByte(lChar));
			}
			return result.ToArray();
		}

		/// <summary>
		/// Binary Serialisation to a stream
		/// </summary>
		/// <param name="value"></param>
		/// <param name="stream">The file where serialised data has to be stored</param>
		public static void Serialize(this String value, Stream stream)
		{
			try
			{
				IFormatter formatter = new BinaryFormatter();
				formatter.Serialize(stream, value);
			}
			catch (Exception)
			{
				throw;
			}
		}

		/// <summary>
		/// String to MemoryStream, uses UTF8 by default.
		/// </summary>
		/// <param name="src"></param>
		/// <param name="encoding"></param>
		/// <returns>Memory Stream</returns>
		public static MemoryStream ToMemoryStream(this String src, System.Text.Encoding encoding = null)
		{
			if (encoding == null) { encoding = System.Text.Encoding.UTF8; }
			return new MemoryStream(encoding.GetBytes(src));
		}

		/// <summary>
		/// Removes multiple spaces between words
		/// </summary>
		/// <param name="value">Given string to trim.</param>
		/// <returns>Returns a copy of the string after removing the extra spaces</returns>
		public static String TrimIntra(this String value)
		{
			if (value.IsEmpty())
			{
				return value;
			}
			else
			{
				return Regex.Replace(value, @"[\s]+", ValueLib.Space.StringValue, RegexOptions.Compiled);
			}
		}

		/// <summary>
		/// Wraps the passed string onto a new line, when each line length exceeds a given total number of characters, by ensuring words are not cut off.
		/// </summary>
		/// <param name="value">String to line wrap.</param>
		/// <param name="charValueCount">The number of characters which no line should exceed.  This value must be 15 or greater characters.</param>
		/// <returns>The formatted copy of the string after applying the line wrapping.</returns>
		public static String WordWrap(this String value, Int32 charValueCount)
		{
			Contract.Requires(charValueCount >= 15);

			return WordCharWrap(value, charValueCount, false);
		}

		/// <summary>
		/// Wraps the passed string onto a new line, when each line length exceeds a given total number of characters.
		/// Wraps the passed string at the passed total number of characters (if cuttOff is true)
		/// or at the next white-space (if cutOff is false). 
		/// </summary>
		/// <param name="val"></param>
		/// <param name="charCount">The maximum number of characters per line.  This value must be 1 or greater characters.</param>
		/// <returns></returns>
		public static String CharWrap(this String value, Int32 charValueCount)
		{
			Contract.Requires(charValueCount >= 1);

			return WordCharWrap(value, charValueCount, true);
		}

		private static String WordCharWrap(String value, Int32 charValueCount, Boolean cutOff)
		{
			if (value.IsEmpty()) return String.Empty;

			var builder = new StringBuilder(value.Length + 100);
			var breakText = Environment.NewLine;
			var counter = 0;

			if (cutOff)
			{
				while (counter < value.Length)
				{
					if (value.Length > counter + charValueCount)
					{
						builder.Append(value.Substring(counter, charValueCount));
						builder.Append(breakText);
					}
					else
					{
						builder.Append(value.Substring(counter));
					}
					counter += charValueCount;
				}
			}
			else
			{
				var wordCollection = new Words(value);
				for (var i = 0; i < wordCollection.Count - 1; i++)
				{
					// added one to represent the space.
					counter += wordCollection[i].Length + 1;
					if (i != 0 && counter > charValueCount)
					{
						builder.Append(breakText);
						counter = 0;
					}

					builder.Append(wordCollection[i] + ' ');
				}
			}
			// to get rid of the extra space at the end.
			return builder.ToString().TrimEnd();
		}

		/// <summary>
		/// Checks if the string is a valid Email.
		/// </summary>
		/// <param name="val"></param>
		/// <returns>Returns true if it is a valid email address</returns>
		public static Boolean IsValidEmail(this String val)
		{
			if (val.IsEmpty()) return false;

			var expresion = @"^(?:[a-zA-Z0-9_'^&/+-])+(?:\.(?:[a-zA-Z0-9_'^&/+-])+)*@(?:(?:\[?(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?))\.){3}(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\]?)|(?:[a-zA-Z0-9-]+\.)+(?:[a-zA-Z]){2,}\.?)$";
			var regex = new Regex(expresion, RegexOptions.IgnoreCase);
			return regex.IsMatch(val);
		}

		public static Boolean HavePermissionsOnRegistryKey(this String key, RegistryPermissionAccess accesslevel)
		{
			try
			{
				var registryPermission = new RegistryPermission(accesslevel, key);
				registryPermission.Demand();
			}
			catch (SecurityException)
			{
				return false;
			}

			return true;
		}

		public static Boolean RegistryKeyCanWrite(this String key)
		{
			return key.HavePermissionsOnRegistryKey(RegistryPermissionAccess.Write);
		}

		public static Boolean RegistryKeyCanRead(this String key)
		{
			return key.HavePermissionsOnRegistryKey(RegistryPermissionAccess.Read);
		}

		/// <summary>
		/// Checks if the string is a valid URI
		/// Test Coverage: Included
		/// </summary>
		/// <param name="val"></param>
		/// <returns>Returns true if it is a valid URI</returns>
		public static Boolean IsValidUri(this String val)
		{
			if (val.IsEmpty()) return false;

			return Uri.IsWellFormedUriString(val, UriKind.Absolute);
		}
		/// <summary>
		/// Checks if the string is valid IP address.
		/// Validates IPv4 as well as IPv6 addresses
		/// Test Coverage: Included
		/// </summary>
		/// <param name="val"></param>
		/// <returns>Returns true if it is valid IP</returns>
		public static Boolean IsValidIp(this String val)
		{
			if (val.IsEmpty()) return false;

			const String strIPv4Pattern = @"\A(25[0-5]|2[0-4]\d|[0-1]?\d?\d)(\.(25[0-5]|2[0-4]\d|[0-1]?\d?\d)){3}\z";
			const String strIPv6Pattern = @"\A(?:[0-9a-fA-F]{1,4}:){7}[0-9a-fA-F]{1,4}\z";
			const String strIPv6PatternHexCompressed = @"\A((?:[0-9A-Fa-f]{1,4}(?::[0-9A-Fa-f]{1,4})*)?)::((?:[0-9A-Fa-f]{1,4}(?::[0-9A-Fa-f]{1,4})*)?)\z";
			const String strIPv6Pattern6Hex4Dec = @"\A((?:[0-9A-Fa-f]{1,4}:){6,6})(25[0-5]|2[0-4]\d|[0-1]?\d?\d)(\.(25[0-5]|2[0-4]\d|[0-1]?\d?\d)){3}\z";
			const String strIPv6PatternHex4DecCompressed = @"\A((?:[0-9A-Fa-f]{1,4}(?::[0-9A-Fa-f]{1,4})*)?) ::((?:[0-9A-Fa-f]{1,4}:)*)(25[0-5]|2[0-4]\d|[0-1]?\d?\d)(\.(25[0-5]|2[0-4]\d|[0-1]?\d?\d)){3}\z";
			var checkstrIPv4Pattern = new Regex(strIPv4Pattern);
			var checkstrIPv6Pattern = new Regex(strIPv6Pattern);
			var checkstrIPv6PatternHexCompressed = new Regex(strIPv6PatternHexCompressed);
			var checkStrIPv6Pattern6Hex4Dec = new Regex(strIPv6Pattern6Hex4Dec);
			var checkStrIPv6PatternHex4DecCompressed = new Regex(strIPv6PatternHex4DecCompressed);
			return checkstrIPv4Pattern.IsMatch(val, 0) ||
				  checkstrIPv6Pattern.IsMatch(val, 0) ||
				  checkstrIPv6PatternHexCompressed.IsMatch(val, 0) ||
				  checkStrIPv6Pattern6Hex4Dec.IsMatch(val, 0) ||
				  checkStrIPv6PatternHex4DecCompressed.IsMatch(val, 0);
		}

		/// <summary>
		/// Checks if the string is Palindrome
		/// </summary>
		/// <param name="val"></param>
		/// <returns>Returns true if it is a palindrome</returns>
		public static Boolean IsPalindrome(this String val)
		{
			if (val.IsEmpty()) return false;
			return val.ToLower() == val.ToLower().Reverse();
		}

		/// <summary>
		/// Encodes to Base64
		/// </summary>
		/// <param name="val"></param>
		/// <returns>Base 64 Encoded string</returns>
		public static String ToBase64String(this String val)
		{
			var toEncodeAsBytes = Encoding.ASCII.GetBytes(val);
			var returnValue = Convert.ToBase64String(toEncodeAsBytes);
			return returnValue;
		}

		/// <summary>
		/// Decodes a Base64 encoded String
		/// </summary>
		/// <param name="val"></param>
		/// <returns>Base 64 decoded string</returns>
		public static String FromBase64String(this String val)
		{
			var encodedDataAsBytes = Convert.FromBase64String(val);
			var returnValue = Encoding.ASCII.GetString(encodedDataAsBytes);
			return returnValue;
		}

		/// <summary>
		/// Encrypts a string to using MD5 algorithm
		/// </summary>
		/// <param name="val"></param>
		/// <returns>string representation of the MD5 encryption</returns>
		public static String ToMd5String(this String val)
		{
			var builder = new StringBuilder();
			using (var md5Hasher = MD5.Create())
			{
				var data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(val));

				for (var i = 0; i < data.Length; i++)
				{
					builder.Append(data[i].ToString("x2"));
				}
			}
			return builder.ToString();
		}
		/// <summary>
		/// Verifies the string against the encrypted value for equality
		/// </summary>
		/// <param name="val"></param>
		/// <param name="hash">The encrypted value of the string</param>
		/// <returns>true is the given string is equal to the string encrypted</returns>
		public static Boolean VerifyMd5String(this String val, String hash)
		{
			var hashOfInput = ToMd5String(val);
			StringComparer comparer = StringComparer.OrdinalIgnoreCase;
			return 0 == comparer.Compare(hashOfInput, hash) ? true : false;
		}

		public static String Encrypt(this String plainText, String passPhrase)
		{
			var initVectorBytes = Encoding.UTF8.GetBytes(initVector);
			var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
			var password = new PasswordDeriveBytes(passPhrase, null);
			var keyBytes = password.GetBytes(keysize / 8);
			var symmetricKey = new RijndaelManaged();
			symmetricKey.Mode = CipherMode.CBC;
			ICryptoTransform encryptor = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes);
			var memoryStream = new MemoryStream();
			var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
			cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
			cryptoStream.FlushFinalBlock();
			var cipherTextBytes = memoryStream.ToArray();
			memoryStream.Close();
			cryptoStream.Close();
			return Convert.ToBase64String(cipherTextBytes);
		}
		// This size of the IV (in bytes) must = (keysize / 8).  Default keysize is 256, so the IV must be
		// 32 bytes long.  Using a 16 character string here gives us 32 bytes when converted to a byte array.
		private const String initVector = "pemgail9uzpgzl88";
		// This constant is used to determine the keysize of the encryption algorithm
		private const Int32 keysize = 256;

		public static String EncryptSha1(this String password)
		{
			using (var hh = new SHA1Managed())
			{
				var combined = Encoding.ASCII.GetBytes(password);
				return BitConverter.ToString(hh.ComputeHash(combined)).ToLower().Replace("-", String.Empty);
			}
		}

		public static String Decrypt(this String cipherText, String passPhrase)
		{
			var initVectorBytes = Encoding.ASCII.GetBytes(initVector);
			var cipherTextBytes = Convert.FromBase64String(cipherText);
			var password = new PasswordDeriveBytes(passPhrase, null);
			var keyBytes = password.GetBytes(keysize / 8);
			var symmetricKey = new RijndaelManaged();
			symmetricKey.Mode = CipherMode.CBC;
			ICryptoTransform decryptor = symmetricKey.CreateDecryptor(keyBytes, initVectorBytes);
			var memoryStream = new MemoryStream(cipherTextBytes);
			var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
			var plainTextBytes = new Byte[cipherTextBytes.Length];
			var decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
			memoryStream.Close();
			cryptoStream.Close();
			return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
		}

		/// <summary>Determine if two strings start the same.</summary>
		/// <returns>True if both strings start the same</returns>
		public static Boolean StartsTheSameAs(this String primaryValue, String comparingValue)
		{
			var minLength = Math.Min(primaryValue.Length, comparingValue.Length);
			var maxLength = Math.Min(primaryValue.Length, comparingValue.Length);
			if (maxLength - minLength > 1 || minLength == 0)
			{
				return false;
			}

			var count = 0;
			while (count < minLength)
			{
				if (primaryValue[count] != comparingValue[count])
				{
					break;
				}
				count++;
			}
			return (count == minLength && minLength == maxLength - 1) || (count == minLength - 1 && minLength == maxLength);
		}

		/// <summary>
		/// Removes all HTML tags from the passed string.
		/// Test Coverage: Included
		/// </summary>
		/// <param name="val"></param>
		/// <returns></returns>
		public static String StripTags(this String val)
		{
			var stripTags = new Regex("<(.|\n)+?>");
			return stripTags.Replace(val, String.Empty);
		}

		/// <summary>
		/// Converts each new line (\n) and carriage return (\r) symbols to the HTML <br /> tag.
		/// Test Coverage: Included
		/// </summary>
		/// <param name="val"></param>
		/// <returns></returns>
		public static String NewLineToBreak(this String val)
		{
			var regEx = new Regex(@"[\n|\r]+");
			return regEx.Replace(val, ValueLib.HtmlBreak.StringValue);
		}

		public static String RemoveNoise(this String value)
		{
			var safeValue = value ?? String.Empty;
			var sb = new StringBuilder(safeValue.Length);
			foreach (var c in safeValue)
			{
				if (!Char.IsControl(c))
				{
					sb.Append(c);
				}
			}
			return sb.ToString();
		}

		/// <summary>
		/// Replace any noise AKA escape characters with spaces.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static String ReplaceNoise(this String value)
		{
			var safeValue = value ?? String.Empty;
			var sb = new StringBuilder(safeValue.Length);
			foreach (var c in safeValue)
			{
				if (!Char.IsControl(c))
				{
					sb.Append(c);
				}
				else
				{
					sb.Append(' ');
				}
			}
			return sb.ToString();
		}

		public static String Clean(this String value, Char[] characters = null)
		{
			if (value.IsEmpty())
			{
				return value;
			}

			if (characters != null)
			{
				var result = value;
				characters.ForEach(c => result = result.Replace(c.ToString(), String.Empty));
				return result;
			}
			return Regex.Replace(value, @"[^\w\s]", String.Empty, RegexOptions.Compiled).TrimIntra();
		}

		public static String Annul(this String value, IEnumerable<String> annulWords)
		{
			if (value.IsEmpty() || annulWords.IsEmpty())
			{
				return String.Empty;
			}

			annulWords.ForEach(w => value = value.Replace(w, ValueLib.Space.StringValue));
			return value;
		}

		public static String CleanPunctuation(this String value)
		{
			var result = new StringBuilder();
			foreach (var c in value.ToCharArray())
			{
				if (!Char.IsPunctuation(c))
				{
					result.Append(c);
				}
			}
			return result.ToString();
		}

		public static String GetChecksum(this String value, Int16 length = 2)
		{
			String result = null;
			if (!value.IsEmpty())
			{
				var bytes = Encoding.Unicode.GetBytes(value);
				var checksum = 0;
				foreach (var character in bytes)
				{
					checksum += character;
				}
				checksum &= 0xff;
				result = checksum.ToString("X2").Left(length);
			}
			return result;
		}

		/// <summary>
		/// Method checks if passed string is DateTime
		/// </summary>
		/// <param name="text">string text for checking</param>
		/// <returns>True if text is DateTime.</returns>
		public static Boolean IsDateTime(this String text)
		{
			var isDateTime = false;

			// Check for empty string.
			if (String.IsNullOrEmpty(text))
			{
				return false;
			}

			isDateTime = TryParse(text, out DateTime dateTime);

			return isDateTime;
		}

		public static Boolean IsGuid(this String value)
		{
			return Guid.TryParse(value, out Guid dummy);
		}

		/// <summary>
		/// Determines whether the specified string corresponds to a valid 
		/// 24-hour time (between '0:00:00' and '23:59:59').
		/// </summary>
		/// <param name="time">
		/// Time string in a 24-hour format with required hour and minute parts
		/// and optional second (all separated by colons), 
		/// such as '18:30:56' or '18:30'.
		/// </param>
		/// <returns>
		/// <c>true</c> if the specified string corresponds to a valid time; 
		/// otherwise, <c>false</c>.
		/// </returns>
		public static Boolean IsTime24(this String time)
		{
			if (time.IsEmpty())
			{
				return false;
			}

			time = time.Trim();

			const String pattern = @"^\d{1,2}:\d\d(:\d\d){0,1}$";
			var regex = new Regex(pattern);
			Match match = regex.Match(time);
			if (!match.Success)
			{
				return false;
			}

			// Make sure that numeric values are valid.
			var digits = time.Split(':');

			// At the least, we must have hour and minute.
			if (digits.Length < 2)
				return false;

			try
			{
				// Make sure the hour part is between 0 and 23.
				if (Int32.Parse(digits[0]) > 23)
				{
					return false;
				}

				// Make sure the minute part is between 0 and 59.
				if (Int32.Parse(digits[1]) > 59)
				{
					return false;
				}

				// Make sure the second part is between 0 and 59.
				if (digits.Length == 3 && Int32.Parse(digits[2]) > 59)
				{
					return false;
				}
			}
			catch (ArithmeticException)
			{
				return false;
			}

			return true;
		}

		public static String Replace(this String value, String oldValue)
		{
			return value.Replace(oldValue, String.Empty);
		}

		/// <summary>
		/// Case insensitive Replace.
		/// </summary>
		/// <param name="original"></param>
		/// <param name="pattern"></param>
		/// <param name="replacement"></param>
		/// <returns>Returns the original string with any found patterns replaced by the given replacement.</returns>
		/// <remarks>Apparently faster than the Regex (ReplaceCaseInsensitive) version.</remarks>
		public static String ReplaceEx(this String original, String pattern, String replacement)
		{
			Int32 count;
			Int32 position0;
			Int32 position1;
			count = 0;
			position0 = 0;
			position1 = 0;

			var upperString = original.ToUpper();
			var upperPattern = pattern.ToUpper();
			var inc = (original.Length / pattern.Length) * (replacement.Length - pattern.Length);
			var chars = new Char[original.Length + Math.Max(0, inc)];
			position1 = upperString.IndexOf(upperPattern, position0, StringComparison.OrdinalIgnoreCase);
			while (position1 != -1)
			{
				for (var idx = position0; idx <= position1 - 1; idx++)
				{
					chars[count] = original[idx];
					count += 1;
				}

				for (var idx = 0; idx <= replacement.Length - 1; idx++)
				{
					chars[count] = replacement[idx];
					count += 1;
				}

				position0 = position1 + pattern.Length;
				position1 = upperString.IndexOf(upperPattern, position0, StringComparison.OrdinalIgnoreCase);
			}
			if (position0 == 0)
			{
				return original;
			}

			for (var idx = position0; idx <= original.Length - 1; idx++)
			{
				chars[count] = original[idx];
				count += 1;
			}
			return new String(chars, 0, count);
		}

		public static String ToUpperSafe(this String value)
		{
			return value?.ToUpper();
		}

		public static String ToLowerSafe(this String value)
		{
			return value?.ToLower();
		}

		/// <summary>
		/// Compares with a string based on letter pair matches.
		/// </summary>
		/// <param name="value">Primary string.</param>
		/// <param name="similar">Search string for comparison.</param>
		/// <returns>The percentage match from 0.0 to 1.0 where 1.0 is 100%</returns>
		public static Double Similarity(this String value, String similar)
		{
			IList<String> pairs1 = WordLetterPairs(value.ToUpper());
			IList<String> pairs2 = WordLetterPairs(similar.ToUpper());

			var intersection = 0;
			var union = pairs1.Count + pairs2.Count;

			for (var i = 0; i < pairs1.Count; i++)
			{
				for (var j = 0; j < pairs2.Count; j++)
				{
					if (pairs1[i] == pairs2[j])
					{
						intersection++;
						pairs2.RemoveAt(j);//Must remove the match to prevent "GGGG" from appearing to match "GG" with 100% success

						break;
					}
				}
			}

			return (2.0 * intersection) / union;
		}

		/// <summary>
		/// Gets all letter pairs for each
		/// individual word in the string
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		private static IList<String> WordLetterPairs(String str)
		{
			// Tokenize the string and put the tokens/words into an array
			var words = Regex.Split(str, @"\s");

			// For each word
			return words.Where(word => !String.IsNullOrEmpty(word)).SelectMany(LetterPairs).ToList();
		}

		/// <summary>
		/// Generates an array containing every 
		/// two consecutive letters in the input string
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		private static String[] LetterPairs(String str)
		{
			var numPairs = str.Length - 1;
			var pairs = new String[numPairs];
			for (var i = 0; i < numPairs; i++)
			{
				pairs[i] = str.Substring(i, 2);
			}
			return pairs;
		}
	}
}

