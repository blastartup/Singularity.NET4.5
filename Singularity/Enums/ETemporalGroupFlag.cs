using System;

// ReSharper disable once CheckNamespace

namespace Singularity
{
	[Flags]
	public enum ETemporalGroupFlag
	{
		[TimeSpan("year", Days = 365)]
		Year = 1,
		[TimeSpan("month", Days = 30, Hours = 10)]
		Month = 2,
		[TimeSpan("week", Days = 7)]
		Week = 4,
		[TimeSpan("day", Days = 1)]
		Day = 8,
		[TimeSpan("hour", Hours = 1)]
		Hour = 16,
		[TimeSpan("minute", Minutes = 1)]
		Minute = 32,
		[TimeSpan("second", Seconds = 1)]
		Second = 64
	}

}
