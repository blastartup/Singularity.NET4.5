using System.Net;

// ReSharper disable once CheckNamespace

namespace Singularity
{
	/// <summary>
	/// A generic inteface so methods can reply a value and/or a condition.
	/// </summary>
	public interface IReply
	{
	}

	public interface IReply<T> : IReply
	{
		T Condition { get; set; }
	}

}
