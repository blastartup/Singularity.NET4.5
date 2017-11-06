using System;

namespace Singularity
{
	/// <summary>
	/// Give English names to common punctuation marks as either char or string types.
	/// </summary>
	public static class ValueLib
	{
		public static class Ampersand
		{
			public static Char CharValue = '&';
			public static String StringValue = CharValue.ToString();
		}

		public static class Asterix
		{
			public static Char CharValue = '*';
			public static String StringValue = CharValue.ToString();
		}

		public static class AttributeMark
		{
			public static Char CharValue = Convert.ToChar(255);
			public static String StringValue = CharValue.ToString();
		}

		public static class BackSlash
		{
			public static Char CharValue = '\\';
			public static String StringValue = CharValue.ToString();
		}

		public static class Colon
		{
			public static Char CharValue = ':';
			public static String StringValue = CharValue.ToString();
		}

		public static class Comma
		{
			public static Char CharValue = ',';
			public static String StringValue = CharValue.ToString();
		}

		public static class CommaSpace
		{
			public static Char[] CharValue = new Char[] { ',', ' ' };
			public static String StringValue = ", ";
		}

		public static class Cr
		{
			public static Char CharValue = '\r';
			public static String StringValue = CharValue.ToString();
		}

		public static class DbNullDescription
		{
			public static String StringValue = "DBNull";
		}

		public static class DecimalPoint
		{
			public static Char CharValue = '.';
			public static String StringValue = CharValue.ToString();
		}

		public static class DollarSign
		{
			public static Char CharValue = '$';
			public static String StringValue = CharValue.ToString();
		}

		public static class DoubleQuotes
		{
			public static Char CharValue = '"';
			public static String StringValue = CharValue.ToString();
		}

		public static class EmptyDescription
		{
			public static String StringValue = "Empty";
		}

		/// <summary>
		/// A static class containing the char value array and string value of the operating system specific end of line delimiter(s) viz. CR and/or LF.
		/// </summary>
		public static class EndOfLine
		{
			/// <summary>The string value of the end of line delimiter(s).</summary>
			public static String StringValue = Environment.NewLine;
		}

		public static class EqualsSign
		{
			public static Char CharValue = '=';
			public static String StringValue = CharValue.ToString();
		}

		/// <summary>
		/// A static class containing the char value and string value of the full stop.
		/// </summary>
		public static class FullStop
		{
			/// <summary>The char value of the full stop.</summary>
			public static Char CharValue = '.';
			/// <summary>The string value of the full stop.</summary>
			public static String StringValue = CharValue.ToString();
		}

		/// <summary>
		/// A static class containing the char value and string value of the forward slash.
		/// </summary>
		public static class ForwardSlash
		{
			/// <summary>The char value of the forward slash.</summary>
			public static Char CharValue = '/';
			/// <summary>The string value of the forward slash.</summary>
			public static String StringValue = CharValue.ToString();
		}

		public static class HtmlBreak
		{
			public static String StringValue = "<br/>";
		}

		public static class HtmlSpace
		{
			public static String StringValue = "&nbsp;";
		}

		public static class InvalidDescription
		{
			public static String StringValue = "Invalid";
		}

		public static class KeyValueDelimiter
		{
			public static Char CharValue = '=';
			public static String StringValue = CharValue.ToString();
		}

		public static class MinusSign
		{
			public const Char CharValue = '-';
			public static String StringValue = CharValue.ToString();
		}

		public static class NewLine
		{
			public static Char CharValue = '\n';
			public static String StringValue = CharValue.ToString();
		}

		public static class NotAgedDescription
		{
			public static String StringValue = "Not Aged";
		}

		public static class NullDescription
		{
			public static String StringValue = "Null";
		}

		public static class Pipe
		{
			public static Char CharValue = '|';
			public static String StringValue = CharValue.ToString();
		}

		public static class Plus
		{
			public static Char CharValue = '+';
			public static String StringValue = CharValue.ToString();
		}

		public static class SemiColon
		{
			public static Char CharValue = ';';
			public static String StringValue = CharValue.ToString();
		}

		public static class SingleLineCSharpComments
		{
			public static Char[] CharValue = StringValue.ToCharArray();
			public static String StringValue = "//";
		}

		public static class SingleQuotes
		{
			public static Char CharValue = '\'';
			public static String StringValue = CharValue.ToString();
		}

		public static class Space
		{
			public const Char CharValue = ' ';
			public const String StringValue = " ";
		}

		public static class Tab
		{
			public static Char CharValue = '\t';
			public static String StringValue = CharValue.ToString();
		}

		public static class Tilde
		{
			public const Char CharValue = '~';
			public const String StringValue = "~";
		}

		public static class SubValueMark
		{
			public static Char CharValue = Convert.ToChar(253);
			public static String StringValue = CharValue.ToString();
		}

		public static class UnknownCode
		{
			public static Char[] CharValue = StringValue.ToCharArray();
			public static String StringValue = "UNK";
		}

		public static class ValueMark
		{
			public static Char CharValue = Convert.ToChar(254);
			public static String StringValue = CharValue.ToString();
		}

		public static class Void
		{
			public static Char CharValue = Convert.ToChar(255);
			public static String StringValue = CharValue.ToString();
		}

		public static class Zero
		{
			public static Char CharValue = '0';
			public static String StringValue = CharValue.ToString();
		}

		internal static readonly String AllowedCharsDefaultSet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890.,-()/=!\"%&*;<>";
		internal static readonly String AllowedCharsCaseNotCaseSensitiveSet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz1234567890.,-()/=!\"%&*;<>";
		internal static readonly String BarcodeAlphabet = "ACEFGHJKMNPQRTVWXYZ0123456789";  // Unambiguous digits used for short meaningful codes eg: 10:10 lat/long GPS short code or barcodes.  Length 29.
		internal static readonly String AlphaNumeric = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";  // Length 36.
		public static readonly String IllegalSpreadsheetTitleCharacters = "[]*/:?\\";
		public static readonly String IllegalEnumNameCharacters = "[]*/:?\\-=+_{}()'\"!@#$%^&*`~ ";
	}
}
