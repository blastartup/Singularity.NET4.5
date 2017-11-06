using System;

// ReSharper disable once CheckNamespace

namespace Singularity
{
	public abstract class RetryStrategry
	{
		public RetryStrategry() { }

		public DateTime Started
		{
			get { return InternalStarted; }
			set { InternalStarted = value; }
		}
		protected DateTime InternalStarted;

		public Int32 Attempts
		{
			get { return InternalAttempts; }
			set { InternalAttempts = value; }
		}
		protected Int32 InternalAttempts;

		public Int32 Delay
		{
			get { return InternalDelay; }
			set { InternalDelay = value; }
		}
		protected Int32 InternalDelay;

		public abstract Boolean CanContinue();
		public abstract Boolean ShouldWait();

		protected Int32 TimeoutInSeconds, Frequency, MaxRetries;
	}
}
