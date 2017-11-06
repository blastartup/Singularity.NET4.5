using System;
using System.Data;

// ReSharper disable once CheckNamespace

namespace Singularity.DataService
{
	public static class DataRowExtension
	{
		public static Object GetValue(this DataRow row, Int32? fieldIdx)
		{
			Object result = null;
			if (fieldIdx != null)
			{
				result = row[fieldIdx.ValueOnNull(0)];
			}
			return result;
		}
	}
}
