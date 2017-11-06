using System;

// ReSharper disable once CheckNamespace

namespace Singularity
{
	public class ErrorMemo : ITraceLog
	{
		public ErrorMemo(String host, String description, String methodName = null, Int32? statusCode = null)
		{
			Description = description;
			MethodName = methodName;
			StatusCode = statusCode;
			Host = host;
		}

		public String Description { get; set; }
		public String MethodName { get; set; }
		public Int32? StatusCode { get; set; }
		public String Host { get; set; }
	}
}
