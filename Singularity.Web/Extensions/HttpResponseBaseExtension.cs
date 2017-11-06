using System.Web;
using System.Web.Caching;

// ReSharper disable once CheckNamespace

namespace Singularity.Web
{
	public static class HttpResponseBaseExtension
	{
		public static void SetDefaultImageHandlers(this HttpResponseBase response)
		{
			response.Cache.SetCacheability(HttpCacheability.Public);
			response.Cache.SetExpires(Cache.NoAbsoluteExpiration);
			response.Cache.SetLastModifiedFromFileDependencies();
		}

	}
}
