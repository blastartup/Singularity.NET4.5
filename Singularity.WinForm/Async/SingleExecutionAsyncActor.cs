using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace Singularity.WinForm.Async
{
	// https://www.codeproject.com/Tips/1168027/Helper-Class-for-Calling-Asynchronous-Methods-usin
	/// <summary>
	/// Execute a long running task asynchronously returning a response.
	/// </summary>
	/// <remarks>Do not use this when executing the same long running task repetitively in succession.  nDuration = (nSeconds * nOccurrence)</remarks>
	/// <typeparam name="TSender">Owner or input argument type.</typeparam>
	/// <typeparam name="TResult">Response from job type.</typeparam>
	/// <seealso cref="AsyncWorker"/>
	/// <seealso cref="Coworker"/>
	/// <seealso cref="RepetitiveExecutionAsyncActor{TSender,TResult}"/>
	public class SingleExecutionAsyncActor<TSender, TResult>
	{
		// Optional delegate to pass back the result...
		public delegate void After(Object sender, TResult result);

		/// <summary>
		/// Initialise the actor passing in the job.
		/// </summary>
		/// <param name="job">Long running task.</param>
		public SingleExecutionAsyncActor(Func<TSender, TResult> job)
		{
			_job = job;
		}

		/// <summary>
		/// Execute the job and trigger a completion event when complete.
		/// </summary>
		/// <param name="sender">Owner or input argument to job.</param>
		/// <param name="after">Completion event.</param>
		/// <returns>A Task class containing the job result.</returns>
		public virtual async Task Act(TSender sender, After after)
		{
			TResult result = await Task.Run(() => _job(sender));

			after?.Invoke(this, result);
		}

		/// <summary>
		/// Exeucte the job.
		/// </summary>
		/// <param name="sender">Owner or input argument to job.</param>
		/// <returns>A Task class containing the job result.</returns>
		public virtual async Task<TResult> Act(TSender sender)
		{
			return await Task.Run(() => _job(sender));
		}

		private readonly Func<TSender, TResult> _job;
	}
}
