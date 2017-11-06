using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

// ReSharper disable once CheckNamespace

namespace Singularity
{
	public static class NameValueCollectionExtension
	{
		public static IEnumerable<KeyValuePair<String, String>> ToKeyValuePairs(this NameValueCollection collection)
		{
			List<KeyValuePair<String, String>> result = new List<KeyValuePair<String, String>>();
			if (!collection.IsEmpty())
			{
				result.AddRange(collection.AllKeys.Select(key => new KeyValuePair<String, String>(key, collection[key])));
			}
			return result;
		}
	}
}
