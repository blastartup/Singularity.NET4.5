using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

// ReSharper disable once CheckNamespace

namespace Singularity.Web.Extensions
{
	public static class NameValueCollectionExtension
	{
		public static String GetUrl(this NameValueCollection parameters, String baseMask = "{0}")
		{
			return baseMask.FormatX(String.Join("&", parameters.AllKeys.Select(x => "{0}={1}".FormatX(HttpUtility.UrlEncode(x), HttpUtility.UrlEncode(parameters[x])))));
		}
	}
}
