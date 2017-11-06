using System.Diagnostics;
using System.Net;

// ReSharper disable once CheckNamespace

namespace Singularity
{
	/// <summary>
	/// A loose generic means to dynamically return a simple or complex condition.
	/// </summary>
	/// <typeparam name="TCondition">Any type or class.</typeparam>
	[DebuggerStepThrough]
	public abstract class Reply<TCondition> : IReply<TCondition>
	{
		/// <summary>
		/// Setup the default condition.
		/// </summary>
		protected Reply()
		{
			Condition = default(TCondition);
		}

		/// <summary>
		/// Store the given condition.
		/// </summary>
		/// <param name="condition">A generic condition.</param>
		protected Reply(TCondition condition)
		{
			Condition = condition;
		}

		/// <summary>
		/// Obtain or set the condition.
		/// </summary>
		public TCondition Condition { get; set; }
	}
}
