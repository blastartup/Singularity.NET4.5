using System;

// ReSharper disable once CheckNamespace

namespace Singularity
{
	public interface IUnitOfMeasure
	{
		String UnitAbbreviation { get; }
		String Mask { get; }
		String Name { get; }
		String Description { get; }
	}
}
