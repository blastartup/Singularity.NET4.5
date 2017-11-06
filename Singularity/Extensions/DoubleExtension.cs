using System;
using System.Diagnostics;

namespace Singularity
{
	[DebuggerStepThrough]
	public static class DoubleExtension
	{
		/// <summary>
		/// Calculates the percentage of the number
		/// </summary>
		/// <param name="value">The value against percentage to be calculated</param>
		/// <param name="percentile"></param>
		/// <param name="roundOffTo">Precision of the result</param>
		/// <returns></returns>
		public static Double Percentage(this Double value, Double percentile, Int32 roundOffTo)
		{
			return Math.Round((value / 100d) * percentile, roundOffTo);
		}

		public static Double ToRadians(this Double degrees)
		{
			return (degrees * Math.PI / 180.0);
		}

		public static Double ToDegrees(this Double radians)
		{
			return (radians / Math.PI * 180.0);
		}

		#region Limits

		/// <summary>
		/// Returns either the given input or maximum value, effectively limiting the given input value to the given maximum argument.
		/// </summary>
		/// <param name="input">Number to be limited.</param>
		/// <param name="maxValue">The maximum value.</param>
		/// <returns></returns>
		public static Double LimitMax(this Double input, Double aMaxValue)
		{
			return aMaxValue * Convert.ToDouble(input > aMaxValue) + input * Convert.ToDouble(input <= aMaxValue);
		}

		/// <summary>
		/// Returns either the given input or minimum value, effectively limiting the given input value to the given minimum argument.
		/// </summary>
		/// <param name="input">Number to be limited.</param>
		/// <param name="maxValue">The minimum value.</param>
		/// <returns></returns>
		public static Double LimitMin(this Double input, Double aMinValue)
		{
			return aMinValue * Convert.ToDouble(input < aMinValue) + input * Convert.ToDouble(input >= aMinValue);
		}

		/// <summary>
		/// Returns either the given input, maximum or minimum value, effectively limiting the given input value to range within the high low limits.
		/// </summary>
		/// <param name="input">Number to be limited</param>
		/// <param name="lowLimit">The minimum value.</param>
		/// <param name="highLimit">The maximum value.</param>
		/// <returns>A number between the low and high limits inclusive.</returns>
		public static Double LimitInRange(this Double input, Double lowLimit, Double highLimit)
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
		public static Double LimitInRange(this Double input, Double lowLimit, Double highLimit, out Boolean wasOutOfRange)
		{
			if (lowLimit > highLimit)
			{
				lowLimit = lowLimit.Swap(ref highLimit);
			}

			Double result = input.LimitMin(lowLimit).LimitMax(highLimit);
			wasOutOfRange = !input.Equals(result);
			return result;
		}

		public static Boolean IsOutOfRange(this Double input, Double lowLimit, Double highLimit)
		{
			Boolean result = false;
			LimitInRange(input, lowLimit, highLimit, out result);
			return result;
		}

		#endregion

	}
}
