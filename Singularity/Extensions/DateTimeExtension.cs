using System;
using System.Diagnostics;
using System.Globalization;

namespace Singularity
{
	[DebuggerStepThrough]
	public static class DateTimeExtension
	{
		public static DateTime AddWorkingDays(this DateTime dateFrom, Int32 numberOfDays)
		{
			// determine if we are increasing or decreasing the days
			Int32 nDirection = 1;
			if (numberOfDays < 0)
			{
				nDirection = -1;
			}

			// move ahead the day of week
			Int32 nWeekday = numberOfDays % 5;

			while (nWeekday != 0)
			{
				dateFrom = dateFrom.AddDays(nDirection);

				if (dateFrom.DayOfWeek != DayOfWeek.Saturday && dateFrom.DayOfWeek != DayOfWeek.Sunday)
				{
					nWeekday -= nDirection;
				}
			}

			// move ahead the number of weeks
			dateFrom = dateFrom.AddDays((numberOfDays / 5) * 7);

			return dateFrom;
		}

		/// <summary>
		/// Returns the minimum JavaScript date time value as a Microsoft date time.
		/// </summary>
		/// <remarks>
		/// According to the JavaScript <a href="http://ecma-international.org/ecma-262/5.1/#sec-15.9.1.1" >ECMA specifications</a> date time 0 is
		/// midnight 1 Janauray 1970, yet it can support dates 100,000,000 days less which is below the minimun Microsft DateTime value.
		/// So, in order to determine the differnece between a MS datatime min value and a JSON equivelant, I've declared that within the Microsft
		/// world, that the minimum JSON date time value with be the MS DateTime.MinValue + 1 millisecond.
		/// </remarks>
		public static readonly DateTime MinJsonValue = DateTime.MinValue.AddMilliseconds(1);

		[DebuggerStepThrough]
		public static Boolean IsRecent(this DateTime referenceDateTime, TimeSpan period)
		{
			return referenceDateTime.Add(period) > DateTime.Now;
		}

		[DebuggerStepThrough]
		public static Boolean ComparableWith(this DateTime referenceDateTime, DateTime targetDateTime)
		{
			return referenceDateTime.LessPrecise().Equals(targetDateTime.LessPrecise());
		}

		[DebuggerStepThrough]
		public static DateTime LessPrecise(this DateTime referenceDateTime)
		{
			return referenceDateTime.AddTicks(-(referenceDateTime.Ticks % TimeSpan.TicksPerSecond));
		}

		[DebuggerStepThrough]
		public static DateTime? ToSydneyTime(this DateTime? date)
		{
			if (!date.IsEmpty())
			{
				return date.Value.ToSydneyTime();
			}

			return null;
		}

		[DebuggerStepThrough]
		public static DateTime ToSydneyTime(this DateTime date)
		{
			if (!date.IsEmpty())
			{
				return TimeZoneInfo.ConvertTimeBySystemTimeZoneId(date, "UTC", "AUS Eastern Standard Time");
			}

			return date;
		}

		[DebuggerStepThrough]
		public static String ToCulture(this DateTime? nullableValue, CultureInfo culture)
		{
			return nullableValue == null ? String.Empty : nullableValue.Value.ToString(culture);
		}

		[DebuggerStepThrough]
		public static Int32 CurrentOffsetHours(this DateTime referenceDateTime)
		{
			return TimeZone.CurrentTimeZone.GetUtcOffset(referenceDateTime).Hours;
		}

		/// <summary>
		/// Retrieves hour, minute, and second from a 24-hour time string.
		/// </summary>
		/// <param name="time">
		/// Time string in a 24-hour format with required hour and minute parts
		/// and optional second (all separated by colons), 
		/// such as '18:30:56' or '18:30'.
		/// </param>
		/// <param name="hour">
		/// The hour part.
		/// </param>
		/// <param name="minute">
		/// The minutes part.
		/// </param>
		/// <param name="second">
		/// The second part.
		/// </param>
		/// <returns>
		/// <c>true</c> if the specified string corresponds to a valid time; 
		/// otherwise, <c>false</c>.
		/// </returns>
		[DebuggerStepThrough]
		public static Boolean ParseTime24(String time, out Int32 hour, out Int32 minute, out Int32 second)
		{
			// Initialize the output values.
			hour = minute = second = 0;

			// Make sure that the format of the time string is valid.
			if (time.IsTime24())
			{
				return false;
			}

			// Split the time parts.
			String[] digits = time.Split(':');

			// At the least, we must have hour and minute.
			if (digits.Length < 2)
			{
				return false;
			}

			try
			{
				// Make sure the hour part is between 0 and 23.
				Int32 temp = Int32.Parse(digits[0]);

				if (temp > 23)
					return false;

				hour = temp;

				// Make sure the minute part is between 0 and 59.
				temp = Int32.Parse(digits[1]);

				if (temp > 59)
					return false;

				minute = temp;

				// Make sure the second part is between 0 and 59.
				if (digits.Length == 3)
				{
					temp = Int32.Parse(digits[2]);

					if (temp > 59)
						return false;

					second = temp;
				}
			}
			catch (ArithmeticException)
			{
				return false;
			}

			return true;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="time"></param>
		/// <param name="roundingInterval"></param>
		/// <param name="roundingType"></param>
		/// <returns></returns>
		public static TimeSpan Round(this TimeSpan time, TimeSpan roundingInterval, MidpointRounding roundingType)
		{
			return new TimeSpan(
				 Convert.ToInt64(Math.Round(
					  time.Ticks / (Decimal)roundingInterval.Ticks,
					  roundingType
				 )) * roundingInterval.Ticks
			);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="time"></param>
		/// <param name="roundingInterval"></param>
		/// <returns></returns>
		public static TimeSpan Round(this TimeSpan time, TimeSpan roundingInterval)
		{
			return Round(time, roundingInterval, MidpointRounding.ToEven);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="datetime"></param>
		/// <param name="roundingInterval"></param>
		/// <returns></returns>
		public static DateTime Round(this DateTime datetime, TimeSpan roundingInterval)
		{
			return new DateTime((datetime - DateTime.MinValue).Round(roundingInterval).Ticks);
		}

		public static DateTime EndOfDay(this DateTime date)
		{
			return new DateTime(date.Year, date.Month, date.Day, 23, 59, 59, 999);
		}

		public static DateTime StartOfDay(this DateTime date)
		{
			return new DateTime(date.Year, date.Month, date.Day, 0, 0, 0, 0);
		}

		[DebuggerStepThrough]
		public static String ToStringSafe(this DateTime? value)
		{
			return value.HasValue ? value.Value.ToString("s") : String.Empty;
		}

	}
}
