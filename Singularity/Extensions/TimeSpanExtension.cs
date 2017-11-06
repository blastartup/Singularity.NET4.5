using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Singularity
{
	[DebuggerStepThrough]
	public static class TimeSpanExtension
	{
		public static TimeSpan NewWeeks(this TimeSpan value, Int32 aWeeks)
		{
			Contract.Requires(value != null);
			return new TimeSpan(DaysPerWeek * aWeeks, 0, 0, 0);
		}

		public static TimeSpan NewDays(this TimeSpan value, Int32 dDays)
		{
			Contract.Requires(value != null);
			return new TimeSpan(dDays, 0, 0, 0);
		}

		public static TimeSpan NewYears(this TimeSpan value, Int32 years)
		{
			Contract.Requires(value != null);
			return new TimeSpan(DaysPerYear * years, 0, 0, 0);
		}

		public static TimeSpan NewHours(this TimeSpan value, Int32 hours)
		{
			Contract.Requires(value != null);
			return new TimeSpan(0, hours, 0, 0);
		}

		public static TimeSpan NewMinutes(this TimeSpan value, Int32 minutes)
		{
			Contract.Requires(value != null);
			return new TimeSpan(0, 0, minutes, 0);
		}

		public static TimeSpan NewSeconds(this TimeSpan value, Int32 seconds)
		{
			Contract.Requires(value != null);
			return new TimeSpan(0, 0, 0, seconds);
		}

		public static DateTime Ago(this TimeSpan value)
		{
			Contract.Requires(value != null);
			return DateTime.Now - value;
		}

		public static DateTime FromNow(this TimeSpan value)
		{
			Contract.Requires(value != null);
			return DateTime.Now + value;
		}

		public static DateTime AgoSince(this TimeSpan value, DateTime referenceDate)
		{
			Contract.Requires(value != null);
			Contract.Requires(!referenceDate.IsEmpty());
			return referenceDate - value;
		}

		public static DateTime From(this TimeSpan value, DateTime referenceDate)
		{
			Contract.Requires(value != null);
			Contract.Requires(!referenceDate.IsEmpty());
			return referenceDate + value;
		}

		public static String ToDescription(this TimeSpan value)
		{
			Contract.Requires(value != null);
			return TimeSpanArticulator.Articulate(value);
		}

		public static String ToDescription(this TimeSpan value, ETemporalGroupFlag accuracy)
		{
			Contract.Requires(value != null);
			return TimeSpanArticulator.Articulate(value, accuracy);
		}

		public static Int32 DaysPerWeek = 7;
		public static Int32 DaysPerYear = 365;

		public static Boolean IsEmpty(this TimeSpan timeSpan)
		{
			return timeSpan == TimeSpan.MinValue;
		}

		public static TimeSpan Midday => !_midday.IsEmpty() ? _midday : (_midday = new TimeSpan(12, 0, 0));

		[ThreadStatic]
		private static TimeSpan _midday;
	}

}
