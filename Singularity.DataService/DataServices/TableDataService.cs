using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Singularity.DataService.SqlFramework;

namespace Singularity.DataService.DataServices
{
	public abstract class TableDataService
	{
		protected TableDataService(SqlEntityContext sqlEntityContext)
		{
			_sqlEntityContext = sqlEntityContext;
		}

		public Int64 RowCount()
		{
			return (_rowCount ?? (_rowCount = ObtainRowCount())).GetValueOrDefault();
		}
		private Int64? _rowCount;

		protected SqlEntityContext SqlEntityContext => _sqlEntityContext;
		private readonly SqlEntityContext _sqlEntityContext;

		protected abstract Int64 ObtainRowCount();
		public abstract void IdentityInsert(RowDataService dataRow);
		public abstract IEnumerable<RowDataService> GetList();
		public abstract String Name { get; }
	}
}
