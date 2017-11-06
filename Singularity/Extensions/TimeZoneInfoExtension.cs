using System;
using System.Diagnostics;

namespace Singularity
{
	[DebuggerStepThrough]
	public static class TimeZoneInfoExtension
	{
		public static DateTime CurrentLocalTime(this TimeZoneInfo timeZoneInfo)
		{
			return TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now, timeZoneInfo.Id);
		}
	}
}
