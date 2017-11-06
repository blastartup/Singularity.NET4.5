using System;

// ReSharper disable once CheckNamespace

namespace Singularity
{
	/// <summary>
	/// Defines days of the week.
	/// </summary>
	[Flags]
	public enum EDaysOfWeekTypes
	{
		/// <summary>
		/// None.
		/// </summary>
		None = 0,

		/// <summary>
		/// Monday.
		/// </summary>
		Monday = 1,

		/// <summary>
		/// Tuesday.
		/// </summary>
		Tuesday = 2,

		/// <summary>
		/// Wednesday.
		/// </summary>
		Wednesday = 4,

		/// <summary>
		/// Thursday.
		/// </summary>
		Thursday = 8,

		/// <summary>
		/// Friday.
		/// </summary>
		Friday = 16,

		/// <summary>
		/// Saturday.
		/// </summary>
		Saturday = 32,

		/// <summary>
		/// Sunday.
		/// </summary>
		Sunday = 64,

		/// <summary>
		/// Typical work days: Monday - Friday.
		/// </summary>
		WorkDays = Monday | Tuesday | Wednesday | Thursday | Friday,

		/// <summary>
		/// Typical weekend: Saturday, Sunday.
		/// </summary>
		Weekend = Saturday | Sunday,

		/// <summary>
		/// All days of the week.
		/// </summary>
		All = WorkDays | Weekend
	}
}
