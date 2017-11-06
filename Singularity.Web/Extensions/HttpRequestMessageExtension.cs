using System.Web;
using System.Web.Caching;
using System.Net.Http;
using System;

// ReSharper disable once CheckNamespace

namespace Singularity.Web
{
	public static class HttpRequestMessageExtension
	{
		/// <summary>
		/// Returns the URL for a resource in the API, built using the uriMask and parameter values
		/// </summary>
		/// <param name="request"></param>
		/// <param name="uriMask">A string format mask with parameter value placeholders. E.g. "{0}/OtherEntities/{1}"</param>
		/// <param name="values"></param>
		public static Uri GetApiUri(this HttpRequestMessage request, String uriMask, params String[] values)
		{
			String scheme = request.RequestUri.Scheme;
			String authority = request.RequestUri.Authority;
			String fullHost = _mask.FormatX(scheme, authority);

			return new Uri(fullHost + uriMask.FormatX(values));
		}

		private static String _mask = "{0}://{1}/api/";
	}
}
