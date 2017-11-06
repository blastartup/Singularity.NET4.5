using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Singularity;

// ReSharper disable once CheckNamespace

namespace Singularity.DataService
{
	public class ReplyDataTable : ReplyErrorMessage
	{
		public ReplyDataTable() : this(false)
		{
		}

		public ReplyDataTable(Boolean condition) : base(condition)
		{
		}

		public DataTable Value { get; set; }

	}
}
