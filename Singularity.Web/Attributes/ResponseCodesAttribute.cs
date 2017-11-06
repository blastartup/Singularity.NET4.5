using System;
using System.Collections.Generic;
using System.Net;

// ReSharper disable once CheckNamespace

namespace Singularity.Web
{
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
	public class ResponseCodesAttribute : Attribute
	{
		public ResponseCodesAttribute(params HttpStatusCode[] statusCodes)
		{
			ResponseCodes = statusCodes;
		}

		public IEnumerable<HttpStatusCode> ResponseCodes { get; private set; }
	}
}
