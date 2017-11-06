using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace Singularity.WinForm.Async
{
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public class StartAsyncJobEventArgs : DoWorkEventArgs
	{
		public StartAsyncJobEventArgs(Object argument) : base(argument)
		{
		}

		public StartAsyncJobEventArgs(Int32 jobIdx, Object argument) : this(argument)
		{
			JobIdx = jobIdx;
		}

		public Int32 JobIdx { get; set; }
	}
}
