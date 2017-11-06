using System;
using System.Diagnostics;
using System.Text;

// ReSharper disable once CheckNamespace

namespace Singularity
{
	public class StrayPrefixedCodeStrategy : StrayStrategy
	{
		public StrayPrefixedCodeStrategy(Int32 length, String prefix)
			: base(length)
		{
			if (length < 4 || length > 20)
			{
				throw new InvalidOperationException("Length exceeds boundary requires 4 =< length =< 30.");
			}
			Length = length;
			Prefix = prefix;
		}

		[DebuggerStepThrough]
		public override IReply Execute()
		{
			ReplyMessage reply = new ReplyMessage();
			StringBuilder codeBuilder = new StringBuilder();
			for (Int32 idx = 0; idx < Length - Prefix.Length; idx++)
			{
				codeBuilder.Append(ValueLib.AlphaNumeric[Random.Next(0, 36)]);
			}
			reply.Message = codeBuilder.ToString();
			reply.Condition = true;
			return reply;
		}

		protected readonly String Prefix;
		protected readonly Int32 Length;
	}
}
