using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

// ReSharper disable once CheckNamespace

namespace Singularity.WebTime
{
	internal class HttpContextProvider : IContextProvider
	{
		public T GetItem<T>(String key) where T : class
		{
			if (key == null || HttpContext.Current == null || HttpContext.Current.Items.IsEmpty())
			{
				return null as T;
			}
			return (T)HttpContext.Current.Items[key];
		}

		public void SetItem<T>(String key, T value) where T : class
		{
			if (HttpContext.Current != null)
			{
				HttpContext.Current.Items[key] = value;
			}
		}
	}
}
