using System;

namespace Singularity.WinForm
{
	public sealed class Clock : SystemClock, IDisposable
	{
		private Clock()
		{
		}

		// Public implementation of Dispose pattern callable by consumers. 
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		public static Clock Now
		{
			get { return DisposableFactory.RequestDisposableObject(() => new Clock(), "Clock"); }
		}



		// Protected implementation of Dispose pattern. 
		void Dispose(Boolean disposing)
		{
			if (_disposed)
			{
				return;
			}

			if (disposing)
			{
				// Clean up here...
			}

			_disposed = true;
		}

		// Flag: Has Dispose already been called? 
		Boolean _disposed = false;
	}
}
