using System;
using System.Collections;
using System.Data;

//Convert .Net Type to SqlDbType or DbType and vise versa
//This class can be useful when you make conversion between types .The class supports conversion between .Net Type , SqlDbType and DbType .
namespace Singularity.DataService.ReferenceType
{
	/// <summary>
	/// Convert a base data type to another base data type
	/// </summary>
	internal sealed class TypeConverter
	{
		#region Constructors

		static TypeConverter()
		{
			DbTypeMapEntry dbTypeMapEntry = new DbTypeMapEntry(typeof(Boolean), DbType.Boolean, SqlDbType.Bit);
			_DbTypeList.Add(dbTypeMapEntry);

			dbTypeMapEntry = new DbTypeMapEntry(typeof(Byte), DbType.Double, SqlDbType.TinyInt);
			_DbTypeList.Add(dbTypeMapEntry);

			dbTypeMapEntry = new DbTypeMapEntry(typeof(Byte[]), DbType.Binary, SqlDbType.Image);
			_DbTypeList.Add(dbTypeMapEntry);

			dbTypeMapEntry = new DbTypeMapEntry(typeof(DateTime), DbType.DateTime, SqlDbType.DateTime);
			_DbTypeList.Add(dbTypeMapEntry);

			dbTypeMapEntry = new DbTypeMapEntry(typeof(Decimal), DbType.Decimal, SqlDbType.Decimal);
			_DbTypeList.Add(dbTypeMapEntry);

			dbTypeMapEntry = new DbTypeMapEntry(typeof(Double), DbType.Double, SqlDbType.Float);
			_DbTypeList.Add(dbTypeMapEntry);

			dbTypeMapEntry = new DbTypeMapEntry(typeof(Guid), DbType.Guid, SqlDbType.UniqueIdentifier);
			_DbTypeList.Add(dbTypeMapEntry);

			dbTypeMapEntry = new DbTypeMapEntry(typeof(Int16), DbType.Int16, SqlDbType.SmallInt);
			_DbTypeList.Add(dbTypeMapEntry);

			dbTypeMapEntry = new DbTypeMapEntry(typeof(Int32), DbType.Int32, SqlDbType.Int);
			_DbTypeList.Add(dbTypeMapEntry);

			dbTypeMapEntry = new DbTypeMapEntry(typeof(Int64), DbType.Int64, SqlDbType.BigInt);
			_DbTypeList.Add(dbTypeMapEntry);

			dbTypeMapEntry = new DbTypeMapEntry(typeof(Object), DbType.Object, SqlDbType.Variant);
			_DbTypeList.Add(dbTypeMapEntry);

			dbTypeMapEntry = new DbTypeMapEntry(typeof(String), DbType.String, SqlDbType.VarChar);
			_DbTypeList.Add(dbTypeMapEntry);

		}

		#endregion

		#region Methods

		/// <summary>
		/// Convert db type to .Net data type
		/// </summary>
		/// <param name="dbType"></param>
		/// <returns></returns>
		public static Type ToNetType(DbType dbType)
		{
			DbTypeMapEntry entry = Find(dbType);
			return entry.Type;
		}

		/// <summary>
		/// Convert TSQL type to .Net data type
		/// </summary>
		/// <param name="sqlDbType"></param>
		/// <returns></returns>
		public static Type ToNetType(SqlDbType sqlDbType)
		{
			DbTypeMapEntry entry = Find(sqlDbType);
			return entry.Type;
		}

		/// <summary>
		/// Convert .Net type to Db type
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public static DbType ToDbType(Type type)
		{
			DbTypeMapEntry entry = Find(type);
			return entry.DbType;
		}

		/// <summary>
		/// Convert TSQL data type to DbType
		/// </summary>
		/// <param name="sqlDbType"></param>
		/// <returns></returns>
		public static DbType ToDbType(SqlDbType sqlDbType)
		{
			DbTypeMapEntry entry = Find(sqlDbType);
			return entry.DbType;
		}

		/// <summary>
		/// Convert .Net type to TSQL data type
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public static SqlDbType ToSqlDbType(Type type)
		{
			DbTypeMapEntry entry = Find(type);
			return entry.SqlDbType;
		}

		/// <summary>
		/// Convert DbType type to TSQL data type
		/// </summary>
		/// <param name="dbType"></param>
		/// <returns></returns>
		public static SqlDbType ToSqlDbType(DbType dbType)
		{
			DbTypeMapEntry entry = Find(dbType);
			return entry.SqlDbType;
		}

		private static DbTypeMapEntry Find(Type type)
		{
			Object retObj = null;
			for (Int32 i = 0; i < _DbTypeList.Count; i++)
			{
				DbTypeMapEntry entry = (DbTypeMapEntry)_DbTypeList[i];
				if (entry.Type == (Nullable.GetUnderlyingType(type) ?? type))
				{
					retObj = entry;
					break;
				}
			}
			if (retObj == null)
			{
				throw
				new ApplicationException("Referenced an unsupported Type");
			}

			return (DbTypeMapEntry)retObj;
		}

		private static DbTypeMapEntry Find(DbType dbType)
		{
			Object retObj = null;
			for (Int32 i = 0; i < _DbTypeList.Count; i++)
			{
				DbTypeMapEntry entry = (DbTypeMapEntry)_DbTypeList[i];
				if (entry.DbType == dbType)
				{
					retObj = entry;
					break;
				}
			}
			if (retObj == null)
			{
				throw
				new ApplicationException("Referenced an unsupported DbType");
			}

			return (DbTypeMapEntry)retObj;
		}

		private static DbTypeMapEntry Find(SqlDbType sqlDbType)
		{
			Object retObj = null;
			for (Int32 i = 0; i < _DbTypeList.Count; i++)
			{
				DbTypeMapEntry entry = (DbTypeMapEntry)_DbTypeList[i];
				if (entry.SqlDbType == sqlDbType)
				{
					retObj = entry;
					break;
				}
			}
			if (retObj == null)
			{
				throw
				new ApplicationException("Referenced an unsupported SqlDbType");
			}

			return (DbTypeMapEntry)retObj;
		}

		#endregion

		private struct DbTypeMapEntry
		{
			public DbTypeMapEntry(Type type, DbType dbType, SqlDbType sqlDbType)
			{
				this.Type = type;
				this.DbType = dbType;
				this.SqlDbType = sqlDbType;
			}

			public Type Type;
			public DbType DbType;
			public SqlDbType SqlDbType;
		};

		private static ArrayList _DbTypeList = new ArrayList();

	}
}