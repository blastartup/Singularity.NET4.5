using System;
using System.Data;
using Singularity.DataService.ReferenceType;

// ReSharper disable once CheckNamespace

namespace Singularity.DataService
{
	public static class SqlDbTypeExtension
	{
		public static Type ToNetType(this SqlDbType sqlDbType)
		{
			return TypeConverter.ToNetType(sqlDbType);
		}

		public static DbType ToDbType(this SqlDbType sqlDbType)
		{
			return TypeConverter.ToDbType(sqlDbType);
		}
	}
}
