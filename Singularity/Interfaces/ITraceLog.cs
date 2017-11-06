using System;

// ReSharper disable once CheckNamespace

namespace Singularity
{
	public interface ITraceLog
	{
		String Description { get; set; }
		String MethodName { get; set; }
		Int32? StatusCode { get; set; }
		String Host { get; set; }
	}
}