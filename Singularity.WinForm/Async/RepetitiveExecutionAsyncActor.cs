using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Singularity.WinForm.Async
{
	/// <summary>
	/// Repetitively execute a long running task asynchronously returning a response.
	/// </summary>
	/// <remarks>
	/// This Actor class can be used to call a function which has one parameter, object (TSender) and returns the result, object (TResult). The 
	/// result is returned in a delegate.
	/// </remarks>
	/// <typeparam name="TSender">Input type</typeparam>
	/// <typeparam name="TResult">Output type</typeparam>
	/// <seealso cref="AsyncWorker"/>
	/// <seealso cref="Coworker"/>
	/// <seealso cref="SingleExecutionAsyncActor{TSender,TResult}"/>
	public class RepetitiveExecutionAsyncActor<TSender, TResult>
	{
		public delegate void WhenComplete(Object sender, State e);

		public virtual void Perform(Func<TSender, TResult> job, TSender parameter, WhenComplete done)
		{
			/* ThreadPool.QueueUserWorkItem takes an object which represents the data to be used by the queued method in WaitCallback.  I'm using 
			 * an anonymous delegate as the method in WaitCallback, and passing the variable state as the data to use.  When a thread becomes 
			 * available the method will execute. */
			ThreadPool.QueueUserWorkItem(new WaitCallback((x) =>
			{
				State state = x as State;
				state.Run();

				// If the calling application neglected to provide a WhenComplete delegate check if null before attempting to invoke.
				done?.Invoke(this, state);
			}), new State(job, parameter));
		}

		// Waitcallback requires an object, lets create one.
		public class State
		{
			/// <summary>
			/// This is the parameter which is passed to the function
			/// defined as job.
			/// </summary>
			public TSender Parameter { get; private set; }

			/// <summary>
			/// This will be the response and will be sent back to the 
			/// calling thread using the delegate (a).
			/// </summary>
			public TResult Result { get; private set; }

			/// <summary>
			/// Actual method to run.
			/// </summary>
			private Func<TSender, TResult> _job;

			/// <summary>
			/// Capture any errors and send back to the calling thread.
			/// </summary>
			public Exception Error { get; private set; }

			public State(Func<TSender, TResult> j, TSender param)
			{
				_job = j;
				Parameter = param;
			}

			/// <summary>
			///  Set as an internal types void so only the Actor class can  
			///  invoke this method.
			/// </summary>
			internal void Run()
			{
				try
				{
					// I think I should check if the method or parameter is null, and react accordingly.  I can check both values at once and 
					// throw a null reference exception.
					if (_job == null | Parameter == null)
					{
						throw new NullReferenceException
						("A value passed to execute is null. Check the response to determine the value.");
					}
					Result = _job(Parameter);
				}
				catch (Exception e)
				{
					Error = e;
					Result = default(TResult);
				}
			}
		}
	}
}
