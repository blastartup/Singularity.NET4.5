using System;

// ReSharper disable once CheckNamespace

namespace Singularity
{
	/// <summary>
	/// Cultural Rounding Provider Interface
	/// </summary>
	public interface IRoundingProvider
	{
		Decimal Round(Decimal value, Int32 decimalPlaces);
	}
}
