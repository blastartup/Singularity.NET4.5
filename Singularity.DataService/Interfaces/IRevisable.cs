using System;

// ReSharper disable once CheckNamespace

namespace Singularity.DataService
{
	public interface IRevisable : ICreatable
	{
		Boolean IsCurrent { get; set; }
	}
}
