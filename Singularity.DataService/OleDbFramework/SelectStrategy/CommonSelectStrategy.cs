using System;
using System.Data.OleDb;

namespace Singularity.DataService.OleDbFramework.SelectStrategy
{
	public class CommonSelectStrategy
	{
		public CommonSelectStrategy(OleDbEntityContext context)
		{
			Context = context;
		}

		public virtual OleDbDataReader SelectQuery(String selectColumns, String fromTables, String join, String filter, OleDbParameter[] filterParameters, String orderBy, Paging paging)
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
				filterParameters = new OleDbParameter[] { };
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

		protected readonly OleDbEntityContext Context;
	}
}
