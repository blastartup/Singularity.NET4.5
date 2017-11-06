using System;

// ReSharper disable once CheckNamespace

namespace Singularity
{
	public interface IDateTimeProvider
	{
		DateTime CurrentLocalDate { get; }
		DateTime CurrentLocalDateTime { get; }
		DateTime CurrentUtcDateTime { get; }
		DateTime CurrentDbServerDateTime { get; }
		String FormatSqlDateTime(DateTime dateTime);
		DateTime FromSqlDateTime(String sqlDateTime);
	}
}
