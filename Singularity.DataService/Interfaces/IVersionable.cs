using System;

// ReSharper disable once CheckNamespace

namespace Singularity.DataService
{
	public interface IVersionable : IRevisable
	{
		Int32 Version { get; set; }
	}
}
