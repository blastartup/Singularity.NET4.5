using System;
using System.Diagnostics;

// ReSharper disable InconsistentNaming
// ReSharper disable once CheckNamespace

namespace Singularity
{
	[DebuggerStepThrough]
	public static class BooleanExtension
	{
		public static BoolWord ToBoolWord(this Boolean value, EBoolWordStyle boolWordType = EBoolWordStyle.TrueFalse)
		{
			return new BoolWord(value, boolWordType);
		}

		public static Int32 PlusMinus(this Boolean value)
		{
			return -1 * (!value).ToInt() + 1 * value.ToInt();
		}

		/// <summary>
		/// In Line Case (boolean) will return either a given true, false or null value depending on the source value of a nullable boolean.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="sourceValue">Nullable boolean to dictate the return value.</param>
		/// <param name="trueValue">Value to return if the source value is true.</param>
		/// <param name="falseValue">Value to return if the source value is false.</param>
		/// <param name="nullValue">Value to return if the source value is null.</param>
		/// <returns></returns>
		public static T ICase<T>(this Boolean? sourceValue, T trueValue, T falseValue, T nullValue)
		{
			if (sourceValue != null)
			{
				return sourceValue == true ? trueValue : falseValue;
			}
			return nullValue;
		}
	}
}
