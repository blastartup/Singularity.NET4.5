using System;

// ReSharper disable once CheckNamespace

namespace Singularity.DataService
{
	public interface ICompletable : ICreatable
	{
		DateTime CompletedDate { get; set; }
	}
}
