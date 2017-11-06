using System;

// ReSharper disable once CheckNamespace

namespace Singularity.DataService
{
	public interface IModifiable : ICreatable
	{
		DateTime ModifiedDate { get; set; }
	}
}
