using System;

// ReSharper disable once CheckNamespace

namespace Singularity.DataService
{
	public interface IRevisable : IModifiable
	{
		Boolean IsCurrent { get; set; }
	}
}
