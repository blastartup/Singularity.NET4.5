using System;

// ReSharper disable once CheckNamespace

namespace Singularity
{
	/// <summary>
	/// Defines days of the week.
	/// </summary>
	[Flags]
	public enum EShortDaysOfWeekTypes : int
	{
		/// <summary>
		/// None.
		/// </summary>
		None = 0,

		/// <summary>
		/// Monday.
		/// </summary>
		Mon = 1,

		/// <summary>
		/// Tuesday.
		/// </summary>
		Tue = 2,

		/// <summary>
		/// Wednesday.
		/// </summary>
		Wed = 4,

		/// <summary>
		/// Thursday.
		/// </summary>
		Thu = 8,

		/// <summary>
		/// Friday.
		/// </summary>
		Fri = 16,

		/// <summary>
		/// Saturday.
		/// </summary>
		Sat = 32,

		/// <summary>
		/// Sunday.
		/// </summary>
		Sun = 64,

		/// <summary>
		/// Typical work days: Monday - Friday.
		/// </summary>
		WorkDays = Mon | Tue | Wed | Thu | Fri,

		/// <summary>
		/// Typical weekend: Saturday, Sunday.
		/// </summary>
		Weekend = Sat | Sun,

		/// <summary>
		/// All days of the week.
		/// </summary>
		All = WorkDays | Weekend
	}
}
