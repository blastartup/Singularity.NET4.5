using System;

// ReSharper disable once CheckNamespace

namespace Singularity.DataService
{
	public interface IDeletable
	{
		DateTime? DeletedDate { get; set; }
	}
}
