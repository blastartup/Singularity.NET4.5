using System;
using System.Text;

// ReSharper disable once CheckNamespace

namespace Singularity
{
	/// <summary>
	/// This class implements the Verhoeff check digit scheme.
	/// This is one of the best available check digit algorithms
	/// that works with any length input.
	/// See:    http://www.cs.utsa.edu/~wagner/laws/verhoeff.html
	///         http://www.augustana.ca/~mohrj/algorithms/checkdigit.html
	///         http://modp.com/release/checkdigits/
	/// </summary>
	public sealed class VerhoeffCheckDigit
	{
		#region Public Static Methods

		#region AppendCheckDigit Method
		//-----------------------------------------------------------------------------------------------
		/// <summary>
		/// Calculates the Verhoeff check digit for the given input, then returns
		/// the input with the check digit appended at the end.
		/// </summary>
		/// <param name="input">The string for which the check digit is to be calculated.</param>
		/// <returns>The input with the calculated check digit appended.</returns>
		public static String AppendCheckDigit(String input)
		{
			Int32[] resultArray = Instance._AppendCheckDigit(_ConvertToIntArray(input));

			StringBuilder resultString = new StringBuilder();

			for (Int32 i = 0; i < resultArray.Length; i++)
			{
				resultString.Append(resultArray[i]);
			}

			return resultString.ToString();
		}

		/// <summary>
		/// Calculates the Verhoeff check digit for the given input, then returns
		/// the input with the check digit appended at the end.
		/// </summary>
		/// <param name="input">The long integer for which the check digit is to be calculated.</param>
		/// <returns>The input with the calculated check digit appended.</returns>
		public static Int64 AppendCheckDigit(Int64 input)
		{
			Int32[] resultArray = Instance._AppendCheckDigit(_ConvertToIntArray(input));
			return _ConvertToLong(resultArray);
		}

		/// <summary>
		/// Calculates the Verhoeff check digit for the given input, then returns
		/// the input with the check digit appended at the end.
		/// </summary>
		/// <param name="input">The integer for which the check digit is to be calculated.</param>
		/// <returns>The input with the calculated check digit appended.</returns>
		public static Int32 AppendCheckDigit(Int32 input)
		{
			Int32[] resultArray = Instance._AppendCheckDigit(_ConvertToIntArray(input));
			Int64 resultLong = _ConvertToLong(resultArray);
			return (Int32)resultLong;
		}

		/// <summary>
		/// Calculates the Verhoeff check digit for the given input, then returns
		/// the input with the check digit appended at the end.
		/// </summary>
		/// <param name="input">The integer array for which the check digit is to be calculated.</param>
		/// <returns>The input with the calculated check digit appended.</returns>
		public static Int32[] AppendCheckDigit(Int32[] input)
		{
			return Instance._AppendCheckDigit(input);
		}
		//-----------------------------------------------------------------------------------------------
		#endregion

		#region CalculateCheckDigit  Method
		//-----------------------------------------------------------------------------------------------
		/// <summary>
		/// Calculates the Verhoeff check digit for the given input.
		/// </summary>
		/// <param name="input">The string for which the check digit is to be calculated.</param>
		/// <returns>The check digit for the input.</returns>
		public static Int32 CalculateCheckDigit(String input)
		{
			return Instance._CalculateCheckDigit(_ConvertToIntArray(input));
		}

		/// <summary>
		/// Calculates the Verhoeff check digit for the given input.
		/// </summary>
		/// <param name="input">The long integer for which the check digit is to be calculated.</param>
		/// <returns>The check digit for the input.</returns>
		public static Int32 CalculateCheckDigit(Int64 input)
		{
			return Instance._CalculateCheckDigit(_ConvertToIntArray(input));
		}

		/// <summary>
		/// Calculates the Verhoeff check digit for the given input.
		/// </summary>
		/// <param name="input">The integer for which the check digit is to be calculated.</param>
		/// <returns>The check digit for the input.</returns>
		public static Int32 CalculateCheckDigit(Int32 input)
		{
			return Instance._CalculateCheckDigit(_ConvertToIntArray(input));
		}

		/// <summary>
		/// Calculates the Verhoeff check digit for the given input.
		/// </summary>
		/// <param name="input">The integer array for which the check digit is to be calculated.</param>
		/// <returns>The check digit for the input.</returns>
		public static Int32 CalculateCheckDigit(Int32[] input)
		{
			return Instance._CalculateCheckDigit(input);
		}
		//-----------------------------------------------------------------------------------------------
		#endregion

		#region Check  Method
		//-----------------------------------------------------------------------------------------------
		/// <summary>
		/// Verifies that a given string has a valid Verhoeff check digit as the last digit.
		/// </summary>
		/// <param name="input">The string for which the check digit is to be checked. The check digit is the last digit in the string.</param>
		/// <returns>Returns true if the last digit of the input is the valid check digit for
		/// the input. Otherwise returns false.</returns>
		public static Boolean Check(String input)
		{
			return Instance._Check(_ConvertToIntArray(input));
		}

		/// <summary>
		/// Verifies that a given long integer has a valid Verhoeff check digit as the last digit.
		/// </summary>
		/// <param name="input">The long integer for which the check digit is to be checked. The check digit is the last digit in the input.</param>
		/// <returns>Returns true if the last digit of the input is the valid check digit for
		/// the input. Otherwise returns false.</returns>
		public static Boolean Check(Int64 input)
		{
			return Instance._Check(_ConvertToIntArray(input));
		}

		/// <summary>
		/// Verifies that a given integer has a valid Verhoeff check digit as the last digit.
		/// </summary>
		/// <param name="input">The integer for which the check digit is to be checked. The check digit is the last digit in the input.</param>
		/// <returns>Returns true if the last digit of the input is the valid check digit for
		/// the input. Otherwise returns false.</returns>
		public static Boolean Check(Int32 input)
		{
			return Instance._Check(_ConvertToIntArray(input));
		}

		/// <summary>
		/// Verifies that a given integer array has a valid Verhoeff check digit as the last digit
		/// in the array.
		/// </summary>
		/// <param name="input">The integer array for which the check digit is to be checked. The check digit is the last element of the array.</param>
		/// <returns>Returns true if the last digit of the input is the valid check digit for
		/// the input. Otherwise returns false.</returns>
		public static Boolean Check(Int32[] input)
		{
			return Instance._Check(input);
		}

		/// <summary>
		/// Verifies the Verhoeff check digit for a given string.
		/// </summary>
		/// <param name="input">The string for which the check digit is to be verified. The input 
		/// does not include the check digit.</param>
		/// <param name="checkDigit">The check digit to be verified.</param>
		/// <returns>Returns true if the check digit is valid for
		/// the input. Otherwise returns false.</returns>
		public static Boolean Check(String input, Int32 checkDigit)
		{
			return Instance._Check(_ConvertToIntArray(input), checkDigit);
		}

		/// <summary>
		/// Verifies the Verhoeff check digit for a given long integer.
		/// </summary>
		/// <param name="input">The long integer for which the check digit is to be verified. The input 
		/// does not include the check digit.</param>
		/// <param name="checkDigit">The check digit to be verified.</param>
		/// <returns>Returns true if the check digit is valid for
		/// the input. Otherwise returns false.</returns>
		public static Boolean Check(Int64 input, Int32 checkDigit)
		{
			return Instance._Check(_ConvertToIntArray(input), checkDigit);
		}

		/// <summary>
		/// Verifies the Verhoeff check digit for a given integer.
		/// </summary>
		/// <param name="input">The integer for which the check digit is to be verified. The input 
		/// does not include the check digit.</param>
		/// <param name="checkDigit">The check digit to be verified.</param>
		/// <returns>Returns true if the check digit is valid for
		/// the input. Otherwise returns false.</returns>
		public static Boolean Check(Int32 input, Int32 checkDigit)
		{
			return Instance._Check(_ConvertToIntArray(input), checkDigit);
		}

		/// <summary>
		/// Verifies the Verhoeff check digit for a given integer array.
		/// </summary>
		/// <param name="input">The integer array for which the check digit is to be verified. The input 
		/// does not include the check digit.</param>
		/// <param name="checkDigit">The check digit to be verified.</param>
		/// <returns>Returns true if the check digit is valid for
		/// the input. Otherwise returns false.</returns>
		public static Boolean Check(Int32[] input, Int32 checkDigit)
		{
			return Instance._Check(input, checkDigit);
		}
		//-----------------------------------------------------------------------------------------------
		#endregion

		#endregion

		#region Private Static Variables
		//-----------------------------------------------------------------------------------------------
		private static VerhoeffCheckDigit _instance = null;
		//-----------------------------------------------------------------------------------------------
		#endregion

		#region Private Instance Variables
		//-----------------------------------------------------------------------------------------------
		private Int32[][] _op = new Int32[10][];
		private Int32[] _inv = { 0, 4, 3, 2, 1, 5, 6, 7, 8, 9 };
		private Int32[][] _f = new Int32[8][];
		//-----------------------------------------------------------------------------------------------
		#endregion

		#region Private Constructor
		//-----------------------------------------------------------------------------------------------
		private VerhoeffCheckDigit()
		{
			_op[0] = new Int32[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
			_op[1] = new Int32[] { 1, 2, 3, 4, 0, 6, 7, 8, 9, 5 };
			_op[2] = new Int32[] { 2, 3, 4, 0, 1, 7, 8, 9, 5, 6 };
			_op[3] = new Int32[] { 3, 4, 0, 1, 2, 8, 9, 5, 6, 7 };
			_op[4] = new Int32[] { 4, 0, 1, 2, 3, 9, 5, 6, 7, 8 };
			_op[5] = new Int32[] { 5, 9, 8, 7, 6, 0, 4, 3, 2, 1 };
			_op[6] = new Int32[] { 6, 5, 9, 8, 7, 1, 0, 4, 3, 2 };
			_op[7] = new Int32[] { 7, 6, 5, 9, 8, 2, 1, 0, 4, 3 };
			_op[8] = new Int32[] { 8, 7, 6, 5, 9, 3, 2, 1, 0, 4 };
			_op[9] = new Int32[] { 9, 8, 7, 6, 5, 4, 3, 2, 1, 0 };

			_f[0] = new Int32[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };  // identity permutation
			_f[1] = new Int32[] { 1, 5, 7, 6, 2, 8, 3, 0, 9, 4 };  // "magic" permutation
			for (Int32 i = 2; i < 8; i++)
			{
				// iterate for remaining permutations
				_f[i] = new Int32[10];
				for (Int32 j = 0; j < 10; j++)
					_f[i][j] = _f[i - 1][_f[1][j]];
			}
		}
		//-----------------------------------------------------------------------------------------------
		#endregion

		#region Private Static Properties
		//-----------------------------------------------------------------------------------------------
		private static VerhoeffCheckDigit Instance
		{
			get
			{
				if (_instance == null)
					_instance = new VerhoeffCheckDigit();
				return _instance;
			}
		}
		//-----------------------------------------------------------------------------------------------
		#endregion

		#region Private Static Methods
		//-----------------------------------------------------------------------------------------------
		private static Int32[] _ConvertToIntArray(String input)
		{
			Int32[] inputArray = new Int32[input.Length];

			for (Int32 i = 0; i < input.Length; i++)
				inputArray[i] = Convert.ToInt32(input.Substring(i, 1));

			return inputArray;
		}

		private static Int32[] _ConvertToIntArray(Int64 input)
		{
			return _ConvertToIntArray(input.ToString());
		}

		private static Int32[] _ConvertToIntArray(Int32 input)
		{
			return _ConvertToIntArray(input.ToString());
		}

		private static Int64 _ConvertToLong(Int32[] input)
		{
			Int64 result = 0;
			Int64 power = 1;

			for (Int32 i = 0; i < input.Length; i++)
			{
				result += input[input.Length - (i + 1)] * power;
				power *= 10;
			}

			return result;
		}
		//-----------------------------------------------------------------------------------------------
		#endregion

		#region Private Instance Methods
		//-----------------------------------------------------------------------------------------------
		private Int32[] _AppendCheckDigit(Int32[] input)
		{
			Int32 checkDigit = _CalculateCheckDigit(input);
			Int32[] result = new Int32[input.Length + 1];
			input.CopyTo(result, 0);
			result[result.Length - 1] = checkDigit;

			return result;
		}

		private Int32 _CalculateCheckDigit(Int32[] input)
		{
			// First we need to reverse the order of the input digits
			Int32[] reversedInput = new Int32[input.Length];
			for (Int32 i = 0; i < input.Length; i++)
				reversedInput[i] = input[input.Length - (i + 1)];

			Int32 check = 0;
			for (Int32 i = 0; i < reversedInput.Length; i++)
				check = _op[check][_f[(i + 1) % 8][reversedInput[i]]];
			Int32 checkDigit = _inv[check];

			return checkDigit;
		}

		private Boolean _Check(Int32[] input)
		{
			// First we need to reverse the order of the input digits
			Int32[] reversedInput = new Int32[input.Length];
			for (Int32 i = 0; i < input.Length; i++)
				reversedInput[i] = input[input.Length - (i + 1)];

			Int32 check = 0;
			for (Int32 i = 0; i < reversedInput.Length; i++)
				check = _op[check][_f[i % 8][reversedInput[i]]];

			return (check == 0);
		}

		private Boolean _Check(Int32[] input, Int32 checkDigit)
		{
			Int32[] newInput = new Int32[input.Length + 1];
			input.CopyTo(newInput, 0);
			newInput[newInput.Length - 1] = checkDigit;
			return _Check(newInput);
		}
		//-----------------------------------------------------------------------------------------------
		#endregion
	}
}
