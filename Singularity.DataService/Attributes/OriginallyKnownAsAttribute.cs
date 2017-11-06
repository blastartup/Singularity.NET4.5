using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// ReSharper disable once CheckNamespace
namespace Singularity.DataService
{
	public sealed class OriginallyKnownAsAttribute : Attribute
	{
		public OriginallyKnownAsAttribute(String name)
		{
			this.Name = name;
		}

		public String Name
		{
			get;
			private set;
		}

	}

}
