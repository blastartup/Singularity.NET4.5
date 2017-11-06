using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Routing;

// ReSharper disable once CheckNamespace

namespace Singularity.Web
{
	public static class ExpandoExtention
	{
		public static ExpandoObject ToExpando(this Object anonymousObject)
		{
			IDictionary<String, Object> anonymousDictionary = new RouteValueDictionary(anonymousObject);
			IDictionary<String, Object> expando = new ExpandoObject();
			foreach (KeyValuePair<String, Object> item in anonymousDictionary)
			{
				expando.Add(item);
			}
			return (ExpandoObject)expando;
		}
	}
}