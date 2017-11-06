using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;

namespace Singularity
{
	[DebuggerStepThrough]
	public static class IntExtension
	{
		/// <summary>
		/// Convert to Ordinal number
		/// </summary>
		/// <param name="value">Integer to convert.</param>
		/// <returns>String representation of the Ordinal number</returns>
		public static String ToOrdinalString(this Int32 value)
		{
			Contract.Requires(value > 0);

			String result = String.Empty;

			Int32 lastTwoDigits = value % 100;
			Int32 lastDigit = lastTwoDigits % 10;
			String lSuffix;
			switch (lastDigit)
			{
				case 1:
					lSuffix = "st";
					break;

				case 2:
					lSuffix = "nd";
					break;

				case 3:
					lSuffix = "rd";
					break;

				default:
					lSuffix = "th";
					break;
			}
			if (lastTwoDigits <= 13)
			{
				lSuffix = "th";
			}
			result = "{0}{1}".FormatX(value, lSuffix);

			//Contract.Ensures(!lResult.IsEmpty());

			return result;
		}

		/// <summary>
		/// Checks if the number is Prime
		/// </summary>
		/// <param name="val"></param>
		/// <returns>true if the number is prime</returns>
		public static Boolean IsPrime(this Int32 value)
		{
			if ((value & 1) == 0)
			{
				return value == 2;
			}
			Int32 lSquareRoot = (Int32)Math.Sqrt((Double)value);
			for (Int32 i = 3; i <= lSquareRoot; i += 2)
			{
				if ((value % i) == 0)
					return false;
			}
			return true;
		}

		/// <summary>
		/// Calculates the factorial of a number
		/// </summary>
		/// <param name="val"></param>
		/// <returns>Factorial of the number</returns>
		public static Double Factorial(this Int32 value)
		{
			if (value <= 1)
				return 1;
			else
				return value * Factorial(value - 1);
		}

		/// <summary>
		/// Calculates the percentage of the number
		/// </summary>
		/// <param name="val"></param>
		/// <param name="value">The value against percentage to be calculated</param>
		/// <param name="roundOffTo">Precision of the result</param>
		/// <returns></returns>
		public static Double PercentOf(this Int32 value, Double percentile, Int32 roundOffTo)
		{
			return Math.Round((value / 100d) * percentile, roundOffTo);
		}

		/// <summary>
		/// Calculates the power of a number
		/// Test Coverage: Included
		/// </summary>
		/// <param name="val"></param>
		/// <param name="off">The number raised to</param>
		/// <returns>The number raised to the power</returns>
		public static Double PowerOf(this Int32 value, Int32 power)
		{
			return Math.Pow(value, power);
		}

		/// <summary>
		/// Is this int within a given range?
		/// </summary>
		public static Boolean IsInRange(this Int32 value, Int32 lowValue, Int32 highValue)
		{
			return (value >= lowValue && value <= highValue);
		}

		#region Limits

		/// <summary>
		/// Returns either the given input or maximum value, effectively limiting the given input value to the given maximum argument.
		/// </summary>
		/// <param name="input">Number to be limited.</param>
		/// <param name="maxValue">The maximum value.</param>
		/// <returns></returns>
		public static Int32 LimitMax(this Int32 input, Int32 aMaxValue)
		{
			return aMaxValue * Convert.ToInt32(input > aMaxValue) + input * Convert.ToInt32(input <= aMaxValue);
		}

		/// <summary>
		/// Returns either the given input or minimum value, effectively limiting the given input value to the given minimum argument.
		/// </summary>
		/// <param name="input">Number to be limited.</param>
		/// <param name="maxValue">The minimum value.</param>
		/// <returns></returns>
		public static Int32 LimitMin(this Int32 input, Int32 aMinValue)
		{
			return aMinValue * Convert.ToInt32(input < aMinValue) + input * Convert.ToInt32(input >= aMinValue);
		}

		/// <summary>
		/// Returns either the given input, maximum or minimum value, effectively limiting the given input value to range within the high low limits.
		/// </summary>
		/// <param name="input">Number to be limited</param>
		/// <param name="lowLimit">The minimum value.</param>
		/// <param name="highLimit">The maximum value.</param>
		/// <returns>A number between the low and high limits inclusive.</returns>
		public static Int32 LimitInRange(this Int32 input, Int32 lowLimit, Int32 highLimit)
		{
			Boolean notUsed;
			return LimitInRange(input, lowLimit, highLimit, out notUsed);
		}

		/// <summary>
		/// Returns either the given input, maximum or minimum value, effectively limiting the given input value to range within the high low limits.
		/// If the value is adjusted then set the aWasOutOfRange flag.
		/// </summary>
		/// <param name="input">Number to be limited</param>
		/// <param name="lowLimit">The minimum value.</param>
		/// <param name="highLimit">The maximum value.</param>
		/// <param name="wasOutOfRange">Returning value indicating whether the given value was outside the range.</param>
		/// <returns></returns>
		public static Int32 LimitInRange(this Int32 input, Int32 lowLimit, Int32 highLimit, out Boolean wasOutOfRange)
		{
			if (lowLimit > highLimit)
			{
				lowLimit = lowLimit.Swap(ref highLimit);
			}

			Int32 result = input.LimitMin(lowLimit).LimitMax(highLimit);
			wasOutOfRange = input != result;
			return result;
		}

		public static Boolean IsOutOfRange(this Int32 input, Int32 lowLimt, Int32 highLimit)
		{
			Boolean result = false;
			LimitInRange(input, lowLimt, highLimit, out result);
			return result;
		}

		#endregion

		/// <summary>
		/// Is the current number odd or even?
		/// </summary>
		/// <param name="input">Number to check</param>
		/// <returns>True indicate the number is indeed even, otherwise false is returned.</returns>
		public static Boolean IsEven(this Int32 input)
		{
			return input % 2 == 0;
		}
	}
}
