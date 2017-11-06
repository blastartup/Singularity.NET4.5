using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Singularity.DataService.ReferenceType;

// ReSharper disable once CheckNamespace

namespace Singularity.DataService
{
	public static class DbTypeExtension
	{
		public static Type ToNetType(this DbType dbType)
		{
			return TypeConverter.ToNetType(dbType);
		}

		public static SqlDbType ToSqlDbType(DbType dbType)
		{
			return TypeConverter.ToSqlDbType(dbType);
		}
	}
}
