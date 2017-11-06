using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Singularity.WinForm
{
	public interface IAsyncService
	{
		void DoWork(Object sender, DoWorkEventArgs eventArgs);
	}
}
