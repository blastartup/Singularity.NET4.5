using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Singularity.DataService.DataServices
{
	public abstract class RowDataService
	{
		protected RowDataService(Object value)
		{
			_value = value;
		}

		// ReSharper disable once ConvertToAutoProperty
		public Object Value => _value;
		private readonly Object _value;

		public abstract void UpdateKey(Int64 seed);
	}
}
