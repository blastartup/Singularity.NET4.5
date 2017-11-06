using System;

// ReSharper disable once CheckNamespace

namespace Singularity
{
	public class RetryCountedStrategy : RetryStrategry
	{
		public RetryCountedStrategy(Int32 maxRetries, Int32 delay)
		{
			this.MaxRetries = maxRetries.LimitInRange(1, 5);
			this.InternalDelay = delay.LimitInRange(1, 120);
		}

		public override Boolean CanContinue()
		{
			return (MaxRetries > 0 && InternalAttempts < MaxRetries);
		}

		public override Boolean ShouldWait()
		{
			return (MaxRetries > 0 && InternalAttempts < MaxRetries);
		}

	}
}
