using System;
using System.Collections.Generic;
using System.Threading;

// ReSharper disable once CheckNamespace

namespace Singularity
{
	/// <summary>
	/// A Timeout or Counted Retry class.
	/// </summary>
	public abstract class Retry : ICommand
	{
		protected Retry(RetryStrategry retryStrategry) 
		{
			RetryStrategry = retryStrategry;
		}

		public IReply Execute()
		{
			ReplySimple result = new ReplySimple(true);
			RetryStrategry.Started = DateTime.UtcNow;
			while (CanContinue())
			{
				try
				{
					if ((result = ExecuteAction()).Condition)
					{
						break;
					}
					RetryStrategry.Attempts++;
					if (ShouldWait())
					{
						ExecuteDelay();
					}
				}
				catch (Exception ex)
				{
					Exceptions.Add(ex);
				}
			}
			return result;
		}

		private Boolean CanContinue()
		{
			return RetryStrategry.CanContinue();
		}

		private Boolean ShouldWait()
		{
			return RetryStrategry.ShouldWait();
		}

		protected abstract ReplySimple ExecuteAction();  // AKA Do something...

		protected virtual void ExecuteDelay()
		{
			Thread.Sleep(RetryStrategry.Delay);
		}

		public AggregateException AggregateException => _aggregateException ?? (_aggregateException = Exceptions.Count > 0 ? new AggregateException(Exceptions) : null);
		private AggregateException _aggregateException;

		protected List<Exception> Exceptions => _exceptions ?? (_exceptions = new List<Exception>());
		private List<Exception> _exceptions;

		protected readonly RetryStrategry RetryStrategry;
	}
}
