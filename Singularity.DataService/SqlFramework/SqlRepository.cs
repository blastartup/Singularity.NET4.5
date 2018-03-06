using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;

namespace Singularity.DataService.SqlFramework
{
	public abstract class SqlRepository<TSqlEntity>
		where TSqlEntity : class
	{
		protected SqlEntityContext Context;

		protected SqlRepository(SqlEntityContext context)
		{
			Context = context;
		}

		public virtual List<TSqlEntity> GetList(String filter = "", SqlParameter[] filterParameters = null, String selectColumns = null,
			String orderBy = null, Paging paging = null)
		{
			if (paging != null && orderBy == null)
			{
				throw new ArgumentException("Paging can only be applied to an ordered result.");
			}

			if (filterParameters != null && filter == null)
			{
				throw new ArgumentException("FilterParameters can only be applied to a filtered result.");
			}

			selectColumns = selectColumns ?? SelectAllColunms();
			return AssembleClassList(SelectQuery(selectColumns, FromTables(), String.Empty, filter, filterParameters, orderBy, paging));
		}

		public List<TSqlEntity> GetListByIds<T>(IEnumerable<T> ids, String selectColumns = null, String orderBy = null, Paging paging = null)
		{
			selectColumns = selectColumns ?? SelectAllColunms();
			return AssembleClassList(SelectQuery(selectColumns, FromTables(), String.Empty, FilterIn(ids), null, orderBy, paging));
		}

		public virtual TSqlEntity GetEntity(String filter = "", SqlParameter[] filterParameters = null, String selectColumns = null)
		{
			selectColumns = selectColumns ?? SelectAllColunms();
			return ReadAndAssembleClass(SelectQuery(selectColumns, FromTables(), String.Empty, filter, filterParameters, null, new Paging(1)));
		}

		public TSqlEntity GetById(Object id, String selectColumns = null)
		{
			selectColumns = selectColumns ?? SelectAllColunms();
			return ReadAndAssembleClass(SelectQuery(selectColumns, FromTables(), String.Empty, WhereClause(), Parameters(id)));
		}

		//public virtual Boolean Exists(String filter = "", SqlParameter[] filterParameters = null, String selectColumns = null)
		//{
		//	selectColumns = selectColumns ?? SelectAllColunms();
		//	return SelectQuery(selectColumns, filter, filterParameters, null, new Paging(1)).HasRows;
		//}

		public void Insert(TSqlEntity sqlEntity)
		{
			if (SaveChangesTransactionally)
			{
				Context.BeginTransaction();
			}

			IModifiable modifiableEntity = sqlEntity as IModifiable;
			if (modifiableEntity != null)
			{
				modifiableEntity.CreatedDate = NowDateTime;
				modifiableEntity.ModifiedDate = NowDateTime;
			}
			else if (sqlEntity is ICreatable)
			{
				((ICreatable)sqlEntity).CreatedDate = NowDateTime;
			}

			InsertCore(sqlEntity, InsertColunms(), GetInsertValues(sqlEntity));
		}

		public void IdentityInsert(TSqlEntity sqlEntity)
		{
			if (SaveChangesTransactionally)
			{
				Context.BeginTransaction();
			}

			IModifiable modifiableEntity = sqlEntity as IModifiable;
			if (modifiableEntity != null)
			{
				modifiableEntity.CreatedDate = NowDateTime;
				modifiableEntity.ModifiedDate = NowDateTime;
			}
			else if (sqlEntity is ICreatable)
			{
				((ICreatable)sqlEntity).CreatedDate = NowDateTime;
			}

			IdentityInsertCore(sqlEntity, InsertColunms(), GetInsertValues(sqlEntity));
		}

		public virtual void Update(TSqlEntity sqlEntity)
		{
			if (SaveChangesTransactionally)
			{
				Context.BeginTransaction();
			}

			if (sqlEntity is IModifiable)
			{
				((IModifiable)sqlEntity).ModifiedDate = NowDateTime;
			}

			UpdateCore(sqlEntity, GetUpdateColumnValuePairs(sqlEntity), GetUpdateKeyColumnValuePair(sqlEntity));
		}

		public Int64 Count(String filter = "", SqlParameter[] filterParameters = null)
		{
			if (filterParameters != null && filter == null)
			{
				throw new ArgumentException("FilterParameters can only be applied to a filtered result.");
			}

			String query = null;

			if (!String.IsNullOrEmpty(filter))
			{
				filter = " where " + filter;
			}

			if (filterParameters == null)
			{
				filterParameters = new SqlParameter[] { };
			}

			query = "Select Count(*) from {0}{1}".FormatX(FromTables(), filter);
			return Context.ExecScalar(query, filterParameters).ToInt64();
		}

		public Boolean Any(String filter = "", SqlParameter[] filterParameters = null)
		{
			if (filterParameters != null && filter == null)
			{
				throw new ArgumentException("FilterParameters can only be applied to a filtered result.");
			}

			String query = null;

			if (!String.IsNullOrEmpty(filter))
			{
				filter = " where " + filter;
			}

			if (filterParameters == null)
			{
				filterParameters = new SqlParameter[] { };
			}

			query = "If Exists (Select * from {0}{1}) Then Print 1 Else Print 0".FormatX(FromTables(), filter);
			return Context.ExecuteNonQuery(query, filterParameters) == 1;
		}

		public void Reseed(Int32 newPrimaryKey = 0)
		{
			newPrimaryKey--;
			SqlCommand cmd = new SqlCommand($"DBCC CheckIdent ({TableName}, reseed, {newPrimaryKey})", Context.SqlConnection);
			cmd.ExecuteNonQuery();
		}

		public void IdentityInsertOn()
		{
			SqlCommand cmd = new SqlCommand($"Set Identity_Insert dbo.{TableName} On", Context.SqlConnection);
			cmd.ExecuteNonQuery();
		}

		public void IdentityInsertOff()
		{
			SqlCommand cmd = new SqlCommand($"Set Identity_Insert dbo.{TableName} Off", Context.SqlConnection);
			cmd.ExecuteNonQuery();
		}

		//public virtual void Deactivate(Object id)
		//{
		//	TEntity entityToDeactivate = DbSet.Find(id);
		//	Deactivate(entityToDeactivate);
		//}

		//public virtual void Deactivate(TEntity entityToDeactivate)
		//{
		//	var deletable = entityToDeactivate as IDeletable;
		//	if (deletable != null)
		//	{
		//		deletable.IsDeleted = true;
		//	}

		//	var modifiable = entityToDeactivate as IModifiable;
		//	if (modifiable != null)
		//	{
		//		modifiable.ModifiedDate = NowDateTime;
		//	}

		//	DbSet.Attach(entityToDeactivate);
		//	Context.Entry(entityToDeactivate).State = EntityState.Modified;
		//}

		public virtual Int32 Delete(Object id)
		{
			String query = "delete from {0} where {1} = @PrimaryKeyName".FormatX(TableName, PrimaryKeyName);
			return Context.ExecuteNonQuery(query, new SqlParameter[]
			{
				new SqlParameter("@PrimaryKeyName", id)
			});
		}

		public virtual Int32 Delete(String filter = "", SqlParameter[] filterParameters = null)
		{
			if (filterParameters != null && filter.IsEmpty())
			{
				throw new ArgumentException("FilterParameters can only be applied to a filtered result.");
			}

			if (filterParameters == null)
			{
				filterParameters = new SqlParameter[] { };
			}

			String query = $"delete from {TableName}";
			if (!filter.IsEmpty())
			{
				query += $" where {filter}";
			}

			return Context.ExecuteNonQuery(query, filterParameters);
		}

		public abstract void Delete(TSqlEntity entityToDelete);

		protected SqlDataReader SelectQuery(String selectColumns, String fromTables, String join = "", String filter = "", SqlParameter[] filterParameters = null, String orderBy = null,
			Paging paging = null)
		{
			String query = null;

			if (!join.IsEmpty())
			{
				join = join.Surround(ValueLib.Space.StringValue);
			}

			if (!String.IsNullOrEmpty(filter))
			{
				filter = " where " + filter;
			}

			if (filterParameters == null)
			{
				filterParameters = new SqlParameter[] { };
			}

			if (!String.IsNullOrEmpty(orderBy))
			{
				orderBy = " Order By " + orderBy;
			}
			else
			{
				orderBy = String.Empty;
			}

			String takeFilter = String.Empty;
			if (paging != null)
			{
				takeFilter = $"Top {paging.Take} ";
			}

			query = $"select {takeFilter}{selectColumns} from {fromTables}{join}{filter}{orderBy}";
			return Context.ExecDataReader(query, filterParameters);
		}

		protected void InsertCore(TSqlEntity sqlEntity, String insertColumns, String insertValues)
		{
			String insertStatement = InsertColumnsPattern.FormatX(TableName, insertColumns, insertValues);
			SetEntityPrimaryKey(sqlEntity, Context.ExecScalar(insertStatement, new SqlParameter[] { }));
		}

		private void IdentityInsertCore(TSqlEntity sqlEntity, String insertColumns, String insertValues)
		{
			insertColumns = $"{GetIdentityInsertColumns()},{insertColumns}";
			insertValues = $"{GetIdentityInsertValues(sqlEntity)},{insertValues}";
			String insertStatement = IdentityInsertColumnsPattern.FormatX(TableName, insertColumns, insertValues, TableName);
			Context.ExecScalar(insertStatement, new SqlParameter[] { });
		}

		protected void UpdateCore(TSqlEntity sqlEntity, String updateColumnValuePairs, String updateKeyColumValuePair)
		{
			// NB: Don't need to update the primary key because it doesn't change nor is returned from an Update SQL query.
			String updateStatement = UpdateColumnsPattern.FormatX(TableName, updateColumnValuePairs, updateKeyColumValuePair);
			Context.ExecScalar(updateStatement, new SqlParameter[] { });
		}

		protected virtual String SelectAllColunms()
		{
			return "*";
		}

		protected virtual String FromTables()
		{
			return TableName;
		}

		protected virtual String WhereClause()
		{
			return $"{PrimaryKeyName} = @pk";
		}

		protected virtual SqlParameter[] Parameters(Object primaryKeyValue)
		{
			return new SqlParameter[] { new SqlParameter("@pk", primaryKeyValue) };
		}

		private String FilterIn<T>(IEnumerable<T> ids)
		{
			return "{0} In ({1})".FormatX(PrimaryKeyName, String.Join(",", (from idItem in ids select ObtainValue<Object>(idItem)).ToArray()));
		}

		protected String ObtainValue<T>(T nativeValue)
		{
			String result = null;
			if (nativeValue == null || Convert.IsDBNull(nativeValue))
			{
				result = "Null";
			}
			else if (nativeValue is String)
			{
				result = String.Format(StringValuePattern, nativeValue.ToString().Replace("'", "''"));
			}
			else if (nativeValue is DateTime)
			{
				result = String.Format(StringValuePattern, ((DateTime)(Object)nativeValue).ToString(DateTimeFormat));
			}
			else if (nativeValue is TimeSpan)
			{
				result = String.Format(StringValuePattern, new DateTime(((TimeSpan)(Object)nativeValue).Ticks).ToShortTimeString());
			}
			else if (nativeValue is Boolean)
			{
				result = Convert.ToInt32((Boolean)(Object)nativeValue).ToString();
			}
			else if (nativeValue is Guid)
			{
				result = String.Format(StringValuePattern, nativeValue.ToString());
			}
			else if (nativeValue.GetType().IsEnum)
			{
				result = Convert.ToInt32(nativeValue).ToString();
			}
			else
			{
				result = nativeValue.ToString();
			}
			return result;
		}

		public Boolean SaveChangesTransactionally { get; set; }

		protected abstract List<TSqlEntity> AssembleClassList(SqlDataReader dataReader);
		protected abstract TSqlEntity ReadAndAssembleClass(SqlDataReader dataReader);
		protected abstract DateTime NowDateTime { get; }
		protected abstract String TableName { get; }
		protected abstract String PrimaryKeyName { get; }
		protected abstract String InsertColunms();

		protected virtual String GetIdentityInsertColumns()
		{
			return $"[{PrimaryKeyName}]";
		}

		protected virtual String GetIdentityInsertValues(TSqlEntity sqlEntity)
		{
			return String.Empty;
		}

		protected abstract String GetInsertValues(TSqlEntity sqlEntity);
		protected abstract String GetUpdateColumnValuePairs(TSqlEntity sqlEntity);
		protected abstract String GetUpdateKeyColumnValuePair(TSqlEntity sqlEntity);
		protected abstract void SetEntityPrimaryKey(TSqlEntity sqlEntity, Object newPrimaryKey);

		protected const String UpdateColumnValuePattern = "{0} = {1}";
		private const String IdentityInsertColumnsPattern = "Set Identity_Insert dbo.{3} On; Insert [{0}] ({1}) Values({2}); Set Identity_Insert dbo.{3} Off";
		private const String InsertColumnsPattern = "Insert [{0}] ({1}) Values({2}) SELECT @@IDENTITY";
		private const String UpdateColumnsPattern = "Update [{0}] Set {1} Where {2}";
		private const String StringValuePattern = "'{0}'";
		private const String DateTimeFormat = "yyyy-MM-dd HH:mm:ss.fff";
	}
}