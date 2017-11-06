using System;

// ReSharper disable once CheckNamespace

namespace Singularity.DataService
{
	public interface IConfiguration
	{
		Guid ConfigurationId { get; set; }
		String Name { get; set; }
		String Value { get; set; }
	}
}
