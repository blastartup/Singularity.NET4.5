using System;

// ReSharper disable once CheckNamespace

namespace Singularity
{
	public interface IExceptionLog : ITraceLog
	{
		Exception Exception { get; set; }
	}

	public interface IExceptionLogged : IExceptionLog
	{
		Int32 Id { get; set; }
	}

	public class ExceptionLogged : IExceptionLogged
	{
		public ExceptionLogged(Int32 id, IExceptionLog exceptionLog)
		{
			Id = id;
			Exception = exceptionLog.Exception;
			Description = exceptionLog.Description;
			MethodName = exceptionLog.MethodName;
			StatusCode = exceptionLog.StatusCode;
			Host = exceptionLog.Host;
		}

		public Int32 Id { get; set; }
		public Exception Exception { get; set; }
		public String Description { get; set; }
		public String MethodName { get; set; }
		public Int32? StatusCode { get; set; }
		public String Host { get; set; }
	}
}