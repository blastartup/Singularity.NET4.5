using System;
using System.Data.OleDb;

namespace Singularity.DataService.OleDbFramework.SelectStrategy
{
	public class VfpSelectStrategy : CommonSelectStrategy
	{
		public VfpSelectStrategy(OleDbEntityContext context) : base(context)
		{
		}

		public override OleDbDataReader SelectQuery(String selectColumns, String fromTables, String join, String filter, OleDbParameter[] filterParameters, String orderBy, Paging paging)
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

			// Visual FoxPro doesn't support Top #.  It has a recno() instead that is included as part of the Where clause...

			query = $"select {selectColumns} from {fromTables}{join}{filter}{orderBy}";
			return Context.ExecDataReader(query, filterParameters);
		}

	}
}
