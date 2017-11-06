using System;

// ReSharper disable once CheckNamespace

namespace Singularity
{
	public interface IFaultManager
	{
		Boolean LogTrace(ITraceLog traceLog);
		Boolean LogException(IExceptionLog exceptionLog);
	}
}
