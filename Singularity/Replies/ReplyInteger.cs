using System;

// ReSharper disable once CheckNamespace

namespace Singularity
{
	public class ReplyInteger : ReplySimple
	{
		public ReplyInteger() : this(false)
		{
		}

		public ReplyInteger(Boolean condition) : base(condition)
		{
		}

		public Int32 Value { get; set; }
	}
}