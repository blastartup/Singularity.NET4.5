using System;
using System.Collections.Generic;
using System.Diagnostics;

// ReSharper disable once CheckNamespace
namespace Singularity
{
	[DebuggerStepThrough]
	public static class ArrayExtension
	{
		public static List<T> ToListSafe<T>(this Array array)
		{
			List<T> result = new List<T>();
			if (array != null)
			{
				foreach (Object item in array)
				{
					try
					{
						result.Add((T)item);
					}
					catch (InvalidCastException) { }
					catch (NullReferenceException) { }
				}
			}
			return result;
		}
	}
}
