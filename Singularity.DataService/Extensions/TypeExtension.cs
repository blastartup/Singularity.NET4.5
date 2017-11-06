using System;
using System.Data;
using Singularity.DataService.ReferenceType;

// ReSharper disable once CheckNamespace

namespace Singularity.DataService
{
	public static class TypeExtension
	{
		public static DbType ToNetType(this Type type)
		{
			return TypeConverter.ToDbType(type);
		}

		public static SqlDbType ToSqlDbType(Type type)
		{
			return TypeConverter.ToSqlDbType(type);
		}
	}
}
