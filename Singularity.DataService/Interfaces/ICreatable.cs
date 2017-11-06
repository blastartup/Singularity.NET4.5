using System;

// ReSharper disable once CheckNamespace

namespace Singularity.DataService
{
	public interface ICreatable
	{
		DateTime CreatedDate { get; set; }
	}
}
