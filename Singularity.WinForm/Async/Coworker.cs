using System;
using System.Reflection;
using System.Threading;
using System.Windows.Threading;

namespace Singularity.WinForm.Async
{
	// http://www.codeproject.com/Articles/378124/Keep-Your-User-Interface-Responsive-Easily-Using-a
	/// <summary>
	/// Quickly and easily switch between UI and non-UI tasks, to keep the UI responsive.
	/// </summary>
	/// <remarks>Good for executing short running, sub second, processing off the UI thread.</remarks>
	/// <seealso cref="SingleExecutionAsyncActor{TSender,TResult}"/>
	/// <seealso cref="AsyncWorker"/>
	/// <seealso cref="RepetitiveExecutionAsyncActor{TSender,TResult}"/>
	public class Coworker
	{
		public class AsyncBlockManager : IDisposable
		{
			public AsyncBlockManager()
			{
				runner = _currentRunner;
				if (runner != null && !runner.Yield())
				{
					// If already in asynchronous mode reset runner so ending/disposing this block
					// will not have any effect.
					runner = null;
				}
			}

			public void EndBlock()
			{
				Dispose();
			}

			#region IDisposable Members

			public void Dispose()
			{
				if (runner != null)
				{
					runner.Resume();
				}
				runner = null;
			}

			#endregion

			TaskRunner runner;
		}

		/// <summary>
		/// Starts a block of code asynchronously (use in a using statement).
		/// </summary>
		/// <returns>A IDisposable object to be disposed when to resume the synchronization context.</returns>
		public static AsyncBlockManager AsyncBlock()
		{
			return new AsyncBlockManager();
		}

		/// <summary>
		/// Runs the given action in asynchronous mode, i.e. releasing any waiting UI thread during its execution.
		/// </summary>
		/// <param name="actionToBeRunAsync">Action to be performed in asynchronous mode</param>
		public static void AsyncBlock(Action actionToBeRunAsync)
		{
			if (_currentRunner != null)
			{
				throw new InvalidOperationException("The version of Coworker.AsyncBlock taking an Action argument can " +
														  "only be called from a non-Coworker context. Use the AsyncBlock() " +
														  "in a 'using' statement instead if you want to run a block of code asynchronously.");
			}
			new TaskRunner().Run(actionToBeRunAsync, true);
		}


		public class SyncBlockManager : IDisposable
		{
			public SyncBlockManager()
			{
				_runner = _currentRunner;
				if (_runner != null && !_runner.Resume())
				{
					// If already in synchronous mode reset runner so that ending/disposing this block
					// will not have any effect.
					_runner = null;
				}
			}

			public void EndBlock()
			{
				Dispose();
			}

			#region IDisposable Members

			public void Dispose()
			{
				if (_runner != null)
				{
					_runner.Yield();
				}
				_runner = null;
			}

			#endregion

			TaskRunner _runner;
		}

		/// <summary>
		/// Starts a block of code to be run synchronously with UI thread (to be used in a using statement).
		/// </summary>
		/// <returns>A IDisposable object to be disposed when to resume asynchronous mode.</returns>
		public static SyncBlockManager SyncBlock()
		{
			return new SyncBlockManager();
		}

		/// <summary>
		/// Runs the given action in synchronous mode on a cooperative mode, 
		/// allowing blocks of code to run asynchronous using <see cref="AsyncBlock"/>.
		/// </summary>
		/// <param name="actionToBeRunSync">Action to be performed in synchronous mode</param>
		public static void SyncBlock(Action actionToBeRunSync)
		{
			if (_currentRunner != null)
			{
				if (_currentRunner != null)
				{
					throw new InvalidOperationException("The version of Coworker.SyncBlock taking an Action argument can " +
															  "only be called from a non-Coworker context. Use the AsyncBlock() " +
															  "in a 'using' statement instead.");

				}
			}
			else
			{
				TaskRunner runner = new TaskRunner();
				runner.Run(actionToBeRunSync, false);
			}
		}

		[ThreadStatic]
		private static TaskRunner _currentRunner;

		private class TaskRunner
		{
			private readonly Object _waitLock = new Object();
			private Boolean _block;
			private Action _task;
			private Exception _exception;
			private SynchronizationContext _context;
			private Dispatcher _dispatcher;

			public void Run(Action task, Boolean runAsync)
			{
				_block = !runAsync;
				_context = SynchronizationContext.Current;
				_dispatcher = Dispatcher.FromThread(Thread.CurrentThread);
				_task = task;
				if (runAsync)
				{
					ThreadPool.QueueUserWorkItem(new WaitCallback(RunTaskAsync));
				}
				else
				{
					ThreadPool.QueueUserWorkItem(new WaitCallback(RunTask));
					Wait();
				}
			}

			private static readonly FieldInfo dispatcherThreadField = typeof(Dispatcher).GetField("_dispatcherThread", BindingFlags.NonPublic | BindingFlags.Instance);

			private Object oldDispatcherThread;

			private void EnterSyncState()
			{
				SynchronizationContext.SetSynchronizationContext(_context);

				if (_dispatcher != null)
				{
					oldDispatcherThread = dispatcherThreadField.GetValue(_dispatcher);
					dispatcherThreadField.SetValue(_dispatcher, Thread.CurrentThread);
				}
			}

			private void ExitSyncState()
			{
				SynchronizationContext.SetSynchronizationContext(_context);

				if (_dispatcher != null)
				{
					dispatcherThreadField.SetValue(_dispatcher, oldDispatcherThread);
				}
			}

			private void RunTaskAsync(Object dummy)
			{

				_currentRunner = this;
				try
				{
					_task();
				}
				catch (Exception ex)
				{
					_exception = ex;
					if (_context != null)
					{
						_context.Post(new SendOrPostCallback(x =>
						{
							ThrowUncaughtException();
						}), null);
					}
					else
					{
						ThrowUncaughtException();
					}
				}
				finally
				{
					// Just in case we have an unfinished sync block, try to release ui thread
					Yield();
					_currentRunner = null;
				}
			}

			private void RunTask(Object dummy)
			{
				EnterSyncState();

				_currentRunner = this;
				try
				{
					_task();
				}
				catch (Exception ex)
				{
					_exception = ex;
				}
				finally
				{
					Yield();
					_currentRunner = null;
				}
			}


			/// <summary>
			/// Ends a synchronous block by releasing the waiting UI thread 
			/// </summary>
			public Boolean Yield()
			{
				lock (_waitLock)
				{
					if (!_block) return false;

					ExitSyncState();
					_block = false;
					Monitor.Pulse(_waitLock);
				}
				return true;
			}

			/// <summary>
			/// Resumes synchronous mode by forcing the ui thread in a waiting state.
			/// </summary>
			public Boolean Resume()
			{
				if (_context != null)
				{
					lock (_waitLock)
					{
						if (_block) return false;

						_context.Post(new SendOrPostCallback(o =>
						{
							lock (_waitLock)
							{
								_block = true;
								Monitor.Pulse(_waitLock);
							}
							Wait();
						}), null);

						// Wait until block is set to true by above code.
						while (!_block) Monitor.Wait(_waitLock);
					}

					EnterSyncState();

					return true;
				}
				return true;
			}

			/// <summary>
			/// Blocks the ui thread during synchronous mode.
			/// </summary>
			public void Wait()
			{
				// Block calling UI thread until asynchronous block are reached
				lock (_waitLock)
				{
					while (_block) Monitor.Wait(_waitLock);
				}
				if (_exception != null)
				{
					ThrowUncaughtException();
				}
			}

			private void ThrowUncaughtException()
			{
				throw new UncaughtCoworkerException("Uncaught exception in Coworker: " + _exception.Message, _exception);
			}


		}

	}

	[Serializable]
	public class UncaughtCoworkerException : Exception
	{

		public UncaughtCoworkerException() { }
		public UncaughtCoworkerException(String message) : base(message) { }
		public UncaughtCoworkerException(String message, Exception inner) : base(message, inner) { }
		protected UncaughtCoworkerException(
		  System.Runtime.Serialization.SerializationInfo info,
		  System.Runtime.Serialization.StreamingContext context)
			 : base(info, context) { }
	}
}
