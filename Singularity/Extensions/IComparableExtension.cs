using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;

// ReSharper disable once CheckNamespace

namespace Singularity
{
	[DebuggerStepThrough]
	public static class ComparableExtension
	{
		/// <summary>
		/// Get an NON NULL value by returning the final value if the primary value is null.
		/// </summary>
		/// <param name="nullableValue">A nullable first preference integer.</param>
		/// <param name="replacementValue">The absolute non nullable integer.</param>
		/// <returns>A first or second preference integer that is NOT null.</returns>
		/// <remarks>Simply nest this method call if you want multiple fallbacks like a tertiary 
		/// preference eg: FallbackOnNull(firstPreint?, FallbackOnNull(seondPreint?, finalInt)) </remarks>
		[DebuggerStepThrough]
		public static T ValueOnNull<T>(this T? nullableValue, T replacementValue) where T : struct
		{
			return nullableValue.HasValue ? nullableValue.Value : replacementValue;
		}

		[DebuggerStepThrough]
		public static T ValueOnNull<T>(this T? nullableValue) where T : struct
		{
			return nullableValue.HasValue ? nullableValue.Value : new T();
		}

		/// <summary>
		/// Is this IComparable within a given range?
		/// </summary>
		[DebuggerStepThrough]
		public static Boolean IsInRange(this IComparable value, IComparable lowValue, IComparable highValue)
		{
			Contract.Assert(lowValue.CompareTo(highValue).In(-1, 0));

			return (value.CompareTo(lowValue).In(0, 1) && value.CompareTo(highValue).In(-1, 0));
		}

		#region Limits

		/// <summary>
		/// Returns either the given input or maximum value, effectively limiting the given input value to the given maximum argument.
		/// </summary>
		/// <param name="input">Number to be limited.</param>
		/// <param name="maxValue">The maximum value.</param>
		/// <returns></returns>
		[DebuggerStepThrough]
		public static IComparable LimitMax(this IComparable input, IComparable maxValue)
		{
			return input.CompareTo(maxValue).Equals(1) ? maxValue : input;
		}

		/// <summary>
		/// Returns either the given input or minimum value, effectively limiting the given input value to the given minimum argument.
		/// </summary>
		/// <param name="input">Number to be limited.</param>
		/// <param name="maxValue">The minimum value.</param>
		/// <returns></returns>
		public static IComparable LimitMin(this IComparable input, IComparable minValue)
		{
			return input.CompareTo(minValue).Equals(-1) ? minValue : input;
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
		public static IComparable LimitInRange(this IComparable input, IComparable lowLimit, IComparable highLimit, out Boolean wasOutOfRange)
		{
			if (lowLimit.CompareTo(highLimit) > 0)
			{
				lowLimit = lowLimit.Swap(ref highLimit);
			}

			IComparable result = LimitMax(LimitMin(input, lowLimit), highLimit);
			wasOutOfRange = input != result;
			return result;
		}

		public static Boolean IsOutOfRange(this IComparable input, IComparable aLowLimt, IComparable aHighLimit)
		{
			Boolean result;
			LimitInRange(input, aLowLimt, aHighLimit, out result);
			return result;
		}

		#endregion

		/// <summary>
		/// Else IF.
		/// </summary>
		/// <typeparam name="T">Any object or struct type.</typeparam>
		/// <param name="predicate">Predicate of a condition.</param>
		/// <param name="sourceValue">Current value of an object or struct to apply condition on.</param>
		/// <param name="trueValue">True value to return if condition is true.</param>
		/// <returns>Given true value or source value depending on whether the condition is true or false.</returns>
		[DebuggerStepThrough]
		public static T ElseIf<T>(this T sourceValue, Func<T, Boolean> predicate, T trueValue)
		{
			return predicate.Invoke(sourceValue) ? trueValue : sourceValue;
		}


		/// <summary>
		/// Inline IF.
		/// </summary>
		/// <typeparam name="T">Any object or struct type.</typeparam>
		/// <param name="predicate">Predicate of a condition.</param>
		/// <param name="sourceValue">Current value of an object or struct to apply condition on.</param>
		/// <param name="trueValue">True value to return if condition is true.</param>
		/// <param name="falseValue">False value to return if condition is false.</param>
		/// <returns>Given true value or false value depending on whether the condition is true or false.</returns>
		[DebuggerStepThrough]
		public static T If<T>(this T sourceValue, Func<T, Boolean> predicate, T trueValue, T falseValue)
		{
			return predicate.Invoke(sourceValue) ? trueValue : falseValue;
		}
	}
}
