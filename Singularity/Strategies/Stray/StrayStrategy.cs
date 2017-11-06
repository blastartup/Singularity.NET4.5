
 // ReSharper disable once CheckNamespace

using System;

// ReSharper disable once CheckNamespace

namespace Singularity
{
	public abstract class StrayStrategy : ICommand
	{
		protected StrayStrategy(Int32 completeLength)
		{
			CompleteLength = completeLength;
			Random = new RandomProvider();
		}

		public Int32 CompleteLength { get; }

		public abstract IReply Execute();

		protected readonly RandomProvider Random;
	}
}
