using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// ReSharper disable once CheckNamespace

namespace Singularity
{
	public struct SortElement
	{
		public SortElement(String propertyName, Boolean descending = false)
		{
			PropertyName = propertyName;
			Descending = descending;
		}

		public String PropertyName { get; set; }
		public Boolean Descending { get; set; }

		public static IEnumerable<SortElement> ConvertToSortElements(String orderBys)
		{
			if (orderBys.IsEmpty())
			{
				return null;
			}

			List<SortElement> sortElements = new List<SortElement>();
			KeyValuePairs keyValuePairs = orderBys.ToKeyValuePairs(ValueLib.Space.CharValue, ValueLib.Comma.CharValue);
			foreach (KeyValuePair<String, String> keyValuePair in keyValuePairs)
			{
				sortElements.Add(new SortElement(keyValuePair.Key, keyValuePair.Value.StartsWith("desc", StringComparison.OrdinalIgnoreCase)));
			}
			return sortElements;
		}
	}
}
