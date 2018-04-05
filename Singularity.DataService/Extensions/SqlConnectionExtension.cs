using System;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Threading;

namespace Singularity.DataService.Extensions
{
	public static class SqlConnectionExtension
	{
		public static void OpenEx(this SqlConnection conn, Int32 timeout)
		{
			if (timeout == 0)
			{
				return;
			}

			// We'll use a Stopwatch here for simplicity. A comparison to a stored DateTime.Now value could also be used
			Stopwatch sw = new Stopwatch();
			Boolean connectSuccess = false;

			// Try to open the connection, if anything goes wrong, make sure we set connectSuccess = false
			Thread t = new Thread(delegate()
			{
				sw.Start();
				conn.Open();
				connectSuccess = true;
			}) {IsBackground = true};

			// Make sure it's marked as a background thread so it'll get cleaned up automatically
			t.Start();

			// Keep trying to join the thread until we either succeed or the timeout value has been exceeded
			while (timeout > sw.ElapsedMilliseconds)
			{
				if (t.Join(1))
				{
					break;
				}
			}

			// If we didn't connect successfully, throw an exception
			if (!connectSuccess)
			{
				throw new TimeoutException($"Waited for ({timeout / 1000}) seconds and subsequently timed out while trying to connect to [{conn.DataSource}].[{conn.Database}].");
			}
		}
	}
}
