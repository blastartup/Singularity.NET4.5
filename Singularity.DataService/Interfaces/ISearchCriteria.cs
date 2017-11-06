using System;

// ReSharper disable once CheckNamespace

namespace Singularity.DataService
{
	public interface ISearchCriteria
	{
		Int32? TakeMaximum { get; set; }
		Paging Paging { get; set; }
	}
}
