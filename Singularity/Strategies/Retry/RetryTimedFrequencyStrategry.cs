using System;

// ReSharper disable once CheckNamespace

namespace Singularity
{
	public class RetryTimedFrequencyStrategry : RetryStrategry
	{
		public RetryTimedFrequencyStrategry(Int32 timeoutInSeconds, Int32 frequency) : base()
		{
			this.TimeoutInSeconds = timeoutInSeconds.LimitInRange(10, 120);
			this.Frequency = frequency.LimitInRange(2, 120);
			InternalDelay = (timeoutInSeconds / frequency) * 1000;
		}

		public override Boolean CanContinue()
		{
			return (TimeoutInSeconds > 0 && !IsTimedOut());
		}

		public override Boolean ShouldWait()
		{
			return (TimeoutInSeconds > 0 && !WillTimeOut());
		}

		private Boolean IsTimedOut()
		{
			return (DateTime.UtcNow - InternalStarted).TotalSeconds > TimeoutInSeconds;
		}

		private Boolean WillTimeOut()
		{
			return (DateTime.UtcNow - InternalStarted).TotalSeconds > TimeoutInSeconds + InternalDelay;
		}

	}
}
