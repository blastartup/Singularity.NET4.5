using System;

// ReSharper disable once CheckNamespace

namespace Singularity
{
	public class ReplyMessage : ReplySimple
	{
		public ReplyMessage() : this(false)
		{
		}

		public ReplyMessage(Boolean condition) : base(condition)
		{
		}

		public String Message { get; set; }
	}
}