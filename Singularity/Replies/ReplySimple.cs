using System;

// ReSharper disable once CheckNamespace

namespace Singularity
{
	/// <summary>
	/// Support for a simple Boolean conditional reply.
	/// </summary>
	public class ReplySimple : Reply<Boolean>
	{
		/// <summary>
		/// Initialise with the default condition of False.
		/// </summary>
		public ReplySimple() : this(false)
		{
		}

		/// <summary>
		/// Initialise with the given condition value of True or False.
		/// </summary>
		/// <param name="condition"></param>
		public ReplySimple(Boolean condition) : base(condition)
		{
		}
	}
}