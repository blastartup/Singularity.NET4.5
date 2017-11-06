using System.Windows.Forms;

namespace Singularity.WinForm.Async
{
	public class AsyncJob
	{
		public AsyncJob() { }

		public AsyncJob(IAsyncService asyncService, Control control)
		{
			AsyncService = asyncService;
			Control = control;
		}

		public IAsyncService AsyncService { get; set; }
		public Control Control { get; set; }
	}
}
