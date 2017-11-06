using System;

// ReSharper disable once CheckNamespace

namespace Singularity
{
	public class ExceptionMemo : ErrorMemo, IExceptionLog
	{
		public ExceptionMemo(String host, Exception exception, String description, String methodName = null, Int32? statusCode = null)
			: base(host, description, methodName, statusCode)
		{
			Exception = exception;
		}

		public Exception Exception { get; set; }
	}
}
