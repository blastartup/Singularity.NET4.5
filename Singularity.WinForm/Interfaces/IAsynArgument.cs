using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Singularity.WinForm.Interfaces
{
	interface IAsyncArgument
	{
		Int32 JobIdx { get; set; }
	}

	interface IAsyncArgument<T> : IAsyncArgument
	{
		T ViewModel { get; set; }
	}
}
