using System;

// ReSharper disable once CheckNamespace

namespace Singularity.DataService
{
	public interface IEnumberable
	{
		Int32 ProxyId { get; set; }
		String Name { get; set; }
	}
}
