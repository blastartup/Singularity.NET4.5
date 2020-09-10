using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Singularity.DataService.SqlFramework
{
	public abstract class SqlRepository<TSqlEntity> : ISqlGeneratable
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

			selectColumns = selectColumns ?? SelectAllColumns();
			return AssembleClassList(SelectQuery(selectColumns, FromTables(), String.Empty, filter, filterParameters, orderBy, paging));
		}

		public List<TSqlEntity> GetListByIds<T>(IEnumerable<T> ids, String selectColumns = null, String orderBy = null, Paging paging = null)
		{
			selectColumns = selectColumns ?? SelectAllColumns();
			return AssembleClassList(SelectQuery(selectColumns, FromTables(), String.Empty, FilterIn(ids), null, orderBy, paging));
		}

		public IEnumerable<String> GenerateInsertSql(Object sqlEntity)
		{
			return GenerateInsertSqlCore((TSqlEntity)sqlEntity);
		}

		private IEnumerable<String> GenerateInsertSqlCore(TSqlEntity sqlEntity)
		{
			return new[] { InsertColumnsPatternSansIdentity.FormatX(TableName, InsertColumns(), GetInsertValues(sqlEntity)), "GO", "" };
		}

		public virtual TSqlEntity GetEntity(String filter = "", SqlParameter[] filterParameters = null, String selectColumns = null, String orderBy = null)
		{
			selectColumns = selectColumns ?? SelectAllColumns();
			return ReadAndAssembleClass(SelectQuery(selectColumns, FromTables(), String.Empty, filter, filterParameters, orderBy, new Paging(1)));
		}

		public List<TSqlEntity> GetListByQuery(String sqlQuery)
		{
			return AssembleClassList(Context.ExecuteDataReader(sqlQuery));
		}

		public IEnumerable<String> GenerateInsertSql(TSqlEntity sqlEntity)
		{
			return new[] { InsertColumnsPattern.FormatX(TableName, InsertColumns(), GetInsertValues(sqlEntity)), "GO", "" };
		}

		public TSqlEntity GetById(Object id, String selectColumns = null)
		{
			selectColumns = selectColumns ?? SelectAllColumns();
			return ReadAndAssembleClass(SelectQuery(selectColumns, FromTables(), String.Empty, WhereClause(), Parameters(id)));
		}

		//public virtual Boolean Exists(String filter = "", SqlParameter[] filterParameters = null, String selectColumns = null)
		//{
		//	selectColumns = selectColumns ?? SelectAllColumns();
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

			InsertCore(sqlEntity, InsertColumns(), GetInsertValues(sqlEntity));
		}

		/// <summary>
		/// An immediate identity insert with identity_insert on just for this single entity, unless queued is true.  Queued identity insert entities are inserted if nothing given.
		/// </summary>
		/// <param name="sqlEntity"></param>
		public void IdentityInsert(TSqlEntity sqlEntity = null, Boolean queued = false)
		{
			if (sqlEntity == null)
			{
				FlushIdentityInserts();
				return;
			}

			// Queue identity insert...
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
			var insertColumns = $"{GetIdentityInsertColumns()},{InsertColumns()}";
			var insertValues = $"{GetIdentityInsertValues(sqlEntity)},{GetInsertValues(sqlEntity)}";
			QueuedIdentityInserts.Append(QueueIdentityInsertColumnsPattern.FormatX(TableName, insertColumns, insertValues));

			if (!queued)
			{
				FlushIdentityInserts();
			}
		}

		public void FlushIdentityInserts()
		{
			if (QueuedIdentityInserts.IsEmpty())
			{
				return;
			}

			if (SaveChangesTransactionally)
			{
				Context.BeginTransaction();
			}

			String insertStatement = IdentityInsertColumnsPattern.FormatX(QueuedIdentityInserts.ToString(), TableName);
			_queuedIdentityInserts = null;
			Context.ExecuteScalar(insertStatement, new SqlParameter[] { });
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
			return Context.ExecuteScalar(query, filterParameters).ToInt64();
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

		public List<TSqlEntity> GetDuplicates(String filter = "", SqlParameter[] filterParameters = null)
		{
			if (filterParameters != null && filter == null)
			{
				throw new ArgumentException("FilterParameters can only be applied to a filtered result.");
			}

			if (ColumnsOfUniqueness.IsEmpty())
			{
				throw new InvalidOperationException("Indeterminate uniqueness.  ColumnOfUniqueness repository property not set.");
			}

			String dupeToOuterJoin = String.Join(" and ", ColumnsOfUniqueness.Split(ValueLib.Comma.CharValue).Select(v => $"o.{v} = dupes.{v}"));
			String orderByWithPk = String.Join(", ", ColumnsOfUniqueness.Split(ValueLib.Comma.CharValue).Select(v => $"o.{v}")) + $", o.{PrimaryKeyName}";

			if (!String.IsNullOrEmpty(filter))
			{
				filter = " where " + filter;
			}

			if (filterParameters == null)
			{
				filterParameters = new SqlParameter[] { };
			}

			String query = $"select {orderByWithPk} from {FromTables()} o inner join (select {ColumnsOfUniqueness} from {FromTables()} group by {ColumnsOfUniqueness} having count(*) > 1) dupes on {dupeToOuterJoin} {filter} order by {orderByWithPk}";
			return AssembleClassList(Context.ExecuteDataReader(query, filterParameters));
		}

		public Boolean TableExists()
		{
			return Context.TableExists(FromTables());
		}

		protected SqlDataReader SelectQuery(String selectColumns, String fromTables, String join = "", String filter = "", SqlParameter[] filterParameters = null, String orderBy = null,
			Paging paging = null)
		{
			if (paging != null && orderBy == null)
			{
				throw new ArgumentException("Paging can only be applied to an ordered result.  Must provide an orderBy argument.");
			}

			String query = null;

			if (!join.IsEmpty())
			{
				join = join.Surround(ValueLib.Space.StringValue);
			}

			if (!String.IsNullOrEmpty(filter))
			{
				filter = " where " + filter;
			}

			List<SqlParameter> sqlParameters = new List<SqlParameter>();
			if (filterParameters == null)
			{
				sqlParameters = new List<SqlParameter>();
			}
			else
			{
				sqlParameters = new List<SqlParameter>(filterParameters);
			}

			orderBy = orderBy == null ? String.Empty : OrderBySubClause + orderBy;
			var pageBy = String.Empty;
			if (paging != null)
			{
				pageBy = PagingSubClause;
				sqlParameters.Add(new SqlParameter("@Skip", paging.Skip));
				sqlParameters.Add(new SqlParameter("@Take", paging.Take));
			}

			query = $"select {selectColumns} from {fromTables}{join}{filter}{orderBy}{pageBy}";
			return Context.ExecuteDataReader(query, sqlParameters.ToArray());
		}
		private const String OrderBySubClause = " Order By ";
		private const String PagingSubClause = " OFFSET (@Skip) ROWS FETCH NEXT (@Take) ROWS ONLY ";

		protected void InsertCore(TSqlEntity sqlEntity, String insertColumns, String insertValues)
		{
			String insertStatement = InsertColumnsPattern.FormatX(TableName, insertColumns, insertValues);
			SetEntityPrimaryKey(sqlEntity, Context.ExecuteScalar(insertStatement, new SqlParameter[] { }));
		}

		private StringBuilder QueuedIdentityInserts => _queuedIdentityInserts ?? (_queuedIdentityInserts = new StringBuilder());
		private StringBuilder _queuedIdentityInserts;

		protected void UpdateCore(TSqlEntity sqlEntity, String updateColumnValuePairs, String updateKeyColumnValuePair)
		{
			// NB: Don't need to update the primary key because it doesn't change nor is returned from an Update SQL query.
			String updateStatement = UpdateColumnsPattern.FormatX(TableName, updateColumnValuePairs, updateKeyColumnValuePair);
			Context.ExecuteScalar(updateStatement, new SqlParameter[] { });
		}

		protected virtual String SelectAllColumns()
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

		protected virtual String ColumnsOfUniqueness => String.Empty;

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
			else if (nativeValue is Byte[])
			{
				result = (nativeValue as Byte[]).ToHexString();
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
		protected abstract String InsertColumns();

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
		private const String QueueIdentityInsertColumnsPattern = "Insert [{0}] ({1}) Values({2}); ";
		private const String IdentityInsertColumnsPattern = "Set Identity_Insert dbo.{1} On; {0} Set Identity_Insert dbo.{1} Off";
		private const String InsertColumnsPattern = "Insert {0} ({1}) Values({2}) SELECT @@IDENTITY";
		private const String InsertColumnsPatternSansIdentity = "Insert [{0}] ({1}) Values({2})";
		private const String UpdateColumnsPattern = "Update {0} Set {1} Where {2}";
		private const String StringValuePattern = "'{0}'";
		private const String DateTimeFormat = "yyyy-MM-dd HH:mm:ss.fff";
	}
}