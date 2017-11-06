
// ReSharper disable once CheckNamespace

namespace Singularity
{
	public enum EDateTimeIntervalTypes : byte
	{
		[EnumAdditional("Ticks")]
		Tick,
		[EnumAdditional("Seconds")]
		Second,
		[EnumAdditional("Minutes")]
		Minute,
		[EnumAdditional("Hours")]
		Hour,
		[EnumAdditional("Days")]
		Day,
		[EnumAdditional("Weeks")]
		Week,
		[EnumAdditional("Months")]
		Month
	}
}
