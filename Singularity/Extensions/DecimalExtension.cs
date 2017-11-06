using System;
using System.Diagnostics;
using System.Globalization;

namespace Singularity
{
	[DebuggerStepThrough]
	public static class DecimalExtension
	{
		public static Boolean IsWithinSqlPrecisionAndScale(this Decimal value, Int32 precision, Int32 scale)
		{
			Decimal maxIntegralPart = GetMaxIntegralPart(precision, scale);
			return (Math.Abs(Decimal.Truncate(value)) <= maxIntegralPart);
		}

		public static Int32 DecimalPlaces(this Decimal value)
		{
			Int32 decimalPlaces = 0;
			Decimal decimalPart = value - Decimal.Truncate(value);	// To stop potential overflow
			while (Decimal.Truncate(decimalPart) != decimalPart)
			{
				decimalPart *= 10;
				decimalPlaces++;
			}
			return decimalPlaces;
		}

		private static String DecimalSeparator => Factory.CurrentCultureInfo.NumberFormat.NumberDecimalSeparator;

		private static String GroupSeparator => Factory.CurrentCultureInfo.NumberFormat.NumberGroupSeparator;

		private static Decimal GetMaxIntegralPart(Int32 precision, Int32 scale)
		{
			return (Decimal)Math.Pow(10, precision - scale) - 1;
		}

		#region Limits

		/// <summary>
		/// Returns either the given input or maximum value, effectively limiting the given input value to the given maximum argument.
		/// </summary>
		/// <param name="input">Number to be limited.</param>
		/// <param name="maxValue">The maximum value.</param>
		/// <returns></returns>
		public static Decimal LimitMax(this Decimal input, Decimal maxValue)
		{
			return maxValue * Convert.ToDecimal(input > maxValue) + input * Convert.ToDecimal(input <= maxValue);
		}

		/// <summary>
		/// Returns either the given input or minimum value, effectively limiting the given input value to the given minimum argument.
		/// </summary>
		/// <param name="input">Number to be limited.</param>
		/// <param name="minValue">The minimum value.</param>
		/// <returns></returns>
		public static Decimal LimitMin(this Decimal input, Decimal minValue)
		{
			return minValue * Convert.ToDecimal(input < minValue) + input * Convert.ToDecimal(input >= minValue);
		}

		/// <summary>
		/// Returns either the given input, maximum or minimum value, effectively limiting the given input value to range within the high low limits.
		/// </summary>
		/// <param name="input">Number to be limited</param>
		/// <param name="lowLimit">The minimum value.</param>
		/// <param name="highLimit">The maximum value.</param>
		/// <returns>A number between the low and high limits inclusive.</returns>
		public static Decimal LimitInRange(this Decimal input, Decimal lowLimit, Decimal highLimit)
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
		public static Decimal LimitInRange(this Decimal input, Decimal lowLimit, Decimal highLimit, out Boolean wasOutOfRange)
		{
			if (lowLimit > highLimit)
			{
				lowLimit = lowLimit.Swap(ref highLimit);
			}

			Decimal result = input.LimitMin(lowLimit).LimitMax(highLimit);
			wasOutOfRange = input != result;
			return result;
		}

		public static Boolean IsOutOfRange(this Decimal input, Decimal lowLimit, Decimal highLimit)
		{
			Boolean result = false;
			LimitInRange(input, lowLimit, highLimit, out result);
			return result;
		}

		#endregion

		public static String FormatMoney(this Decimal amount)
		{
			return amount.ToString("0.00");
		}

		public static String FormatCurrency(this Decimal amount)
		{
			return amount.ToString("C2", CultureInfo.CreateSpecificCulture("en-AU"));
		}
	}
}
