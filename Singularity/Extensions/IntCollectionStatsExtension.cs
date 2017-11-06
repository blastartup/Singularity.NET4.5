using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Singularity
{
	[DebuggerStepThrough]
	public static class IntCollectionStatsExtension
	{
		public static Double StandardDeviation(this IEnumerable<Int32> value)
		{
			Double lSumOfSquares = 0;
			Func<Int32, Int32> lConversion = ToInt;
			Double lAverage = value.Average<Int32>(lConversion);
			foreach (Int32 lItem in value)
			{
				lSumOfSquares += Math.Pow(((Double)lItem - lAverage), 2);
			}
			Double lCount = (Double)value.Count();
			return Math.Sqrt(lSumOfSquares / (lCount - 1));
		}

		private static Int32 ToInt(Int32 value)
		{
			return value;
		}

		public static Double Median(this IEnumerable<Int32> value)
		{
			List<Int32> sortedList = new List<Int32>(value.ToList());
			sortedList.Sort();

			Double median = 0;
			Int32 middleValue = 0;
			Decimal half = 0;

			half = (sortedList.Count - 1) / 2;
			if (sortedList.Count % 2 == 1)
			{
				median = sortedList[Convert.ToInt32(Math.Ceiling(half))];
			}
			else
			{
				middleValue = Convert.ToInt32(Math.Floor(half));
				median = (Double)(sortedList[middleValue] + sortedList[middleValue + 1]) / 2;
			}
			return median;
		}
	}
}
