using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Remoting.Messaging;
using System.Windows.Forms;
using Singularity.WinForm.Interfaces;

namespace Singularity.WinForm.Async
{
	// https://www.codeproject.com/Articles/42912/A-Simple-Way-To-Use-Asynchronous-Call-in-Your-Mult
	/// <summary>
	/// Asynchronously execute a single, cancellable, long running task, and be notified of progress change, busy status and completion.
	/// </summary>
	/// <remarks>AsyncWorker does a bettor job than BackgroundWorker for running asynchronous tasks.</remarks>
	/// <seealso cref="SingleExecutionAsyncActor{TSender,TResult}"/>
	/// <seealso cref="Coworker"/>
	/// <seealso cref="RepetitiveExecutionAsyncActor{TSender,TResult}"/>
	[Obsolete("Legacy hookup is too difficult so use AsyncAction instead.")]
	public class AsyncWorker
	{
		/// <summary>
		/// Occurs when [do work].
		/// </summary>
		public event DoWorkEventHandler DoWork;

		/// <summary>
		/// Occurs when [run worker completed].
		/// </summary>
		public event RunWorkerCompletedEventHandler RunWorkerCompleted;

		/// <summary>
		/// Occurs when [progress changed].
		/// </summary>
		public event ProgressChangedEventHandler ProgressChanged;

		public event CompletingEventHandler CompletingEventHandler;
		public event MilestoneAchievedEventHandler MilestoneAchievedEventHandler;

		/// <summary>
		/// Initializes a new instance of the <see cref="AsyncWorker"/> class.
		/// </summary>
		public AsyncWorker()
		{
			_workerCallback = OnWorkCompleted;
			_eventHandler = OnDoWork;
		}

		public AsyncWorker(AsyncJob asyncJob) : this()
		{
			_asyncJobs = new List<AsyncJob>
			{
				asyncJob
			};
		}

		/// <summary>
		/// Gets a value indicating whether this instance is busy.
		/// </summary>
		/// <value><c>true</c> if this instance is busy; otherwise, <c>false</c>.</value>
		public Boolean IsBusy
		{
			get
			{
				lock (CountProtector)
				{
					return (_isBusy);
				}
			}
		}

		/// <summary>
		/// Gets a value indicating whether [cancellation pending].
		/// </summary>
		/// <value><c>true</c> if [cancellation pending]; otherwise, <c>false</c>.</value>
		public Boolean CancellationPending => _cancelationPending;

		/// <summary>
		/// Runs the worker async.
		/// </summary>
		/// <param name="abortIfBusy">if set to <c>true</c> [abort if busy].</param>
		/// <param name="argument">The argument.</param>
		public Boolean Start(Boolean abortIfBusy, Object argument = null)
		{

			if (abortIfBusy && IsBusy)
			{
				return false;
			}
			_isBusy = true;

			_eventHandler.BeginInvoke(this, new DoWorkEventArgs(argument), _workerCallback, _eventHandler);
			return true;
		}

		/// <summary>
		/// Cancels the async.
		/// </summary>
		public void CancelAsync()
		{
			_cancelationPending = true;
		}

		/// <summary>
		/// Reports the progress.
		/// </summary>
		/// <param name="percentProgress">The percent progress.</param>
		public void ReportProgress(Int32 percentProgress)
		{
			OnProgressChanged(new ProgressChangedEventArgs(percentProgress, null));
		}
		/// <summary>
		/// Reports the progress.
		/// </summary>
		/// <param name="percentProgress">The percent progress.</param>
		/// <param name="userState">State of the user.</param>
		public void ReportProgress(Int32 percentProgress, Object userState)
		{
			OnProgressChanged(new ProgressChangedEventArgs(percentProgress, userState));
		}

		/// <summary>
		/// Called when [do work].
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The <see cref="System.ComponentModel.DoWorkEventArgs"/> instance containing the event data.</param>
		protected virtual void OnDoWork(Object sender, DoWorkEventArgs e)
		{
			if (e.Cancel)
			{
				return;
			}
			Console.WriteLine("Async started " + DateTime.UtcNow.ToString());
			DoWork?.Invoke(this, e);
		}

		/// <summary>
		/// Raises the <see cref="E:ProgressChanged"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.ComponentModel.ProgressChangedEventArgs"/> instance containing the event data.</param>
		protected virtual void OnProgressChanged(ProgressChangedEventArgs e)
		{
			ProgressChanged?.Invoke(this, e);
		}

		protected virtual void OnMilestoneAchieved(MilestoneAchievedEventArgs eventArgs)
		{
			MilestoneAchievedEventHandler?.Invoke(this, eventArgs);
		}

		protected virtual void OnCompleting(CompletingEventArgs eventArgs)
		{
			CompletingEventHandler?.Invoke(this, eventArgs);
		}

		/// <summary>
		/// Called when [run worker completed].
		/// </summary>
		/// <param name="ar">The ar.</param>
		protected virtual void OnWorkCompleted(IAsyncResult ar)
		{
			DoWorkEventHandler doWorkDelegate = (DoWorkEventHandler)((AsyncResult)ar).AsyncDelegate;
			RunWorkerCompleted?.Invoke(this, new RunWorkerCompletedEventArgs(ar, null, _cancelationPending));

			_isBusy = false;
		}

		public void AddAsyncJob(AsyncJob asyncJob)
		{
			_asyncJobs.Add(asyncJob);
		}
		private readonly List<AsyncJob> _asyncJobs;

		public void NextAsyncJob()
		{
			_currentJob++;
		}

		public void StartAsyncJob(Object sender, DoWorkEventArgs eventArgs)
		{
			IAsyncArgument argument = eventArgs.Argument as IAsyncArgument;
			if (argument != null)
			{
				_asyncJobs[argument.JobIdx].AsyncService.DoWork(sender, eventArgs);
			}
			else
			{
				CurrentJob.AsyncService.DoWork(sender, eventArgs);
			}
		}

		public Control CurrentControl => CurrentJob.Control;
		protected AsyncJob CurrentJob => _asyncJobs[_currentJob];

		private Boolean _cancelationPending;
		private Boolean _isBusy;
		private static readonly Object CountProtector = new Object();
		private readonly AsyncCallback _workerCallback;
		private readonly DoWorkEventHandler _eventHandler;
		private Int32 _currentJob;
	}

	public class CompletingEventArgs : EventArgs
	{
		public CompletingEventArgs(Object userState)
		{
			UserState = userState;
		}

		public Object UserState { get; }
	}

	public class MilestoneAchievedEventArgs : EventArgs
	{
		public MilestoneAchievedEventArgs(Object userState)
		{
			UserState = userState;
		}

		public Object UserState { get; }
	}

	public delegate void CompletingEventHandler(Object sender, CompletingEventArgs e);
	public delegate void MilestoneAchievedEventHandler(Object sender, MilestoneAchievedEventArgs e);
}
